using Godot;
using System;

public partial class QuadtreePatch : Node3D
{
	[Export] public PackedScene QuadtreePatchScene;
	public IPatchMeshGenerator MeshGenerator;
	public TerrainNoiseProfile TerrainProfile;

	[Export] public int MaxDepth = 5;
	[Export] public int CurrentDepth = 0;
	[Export] public int[] LODResolutions = new int[] { 8, 16, 32, 64, 128 };
	[Export] public float[] LodDistanceThresholds = new float[] { 400f, 200f, 100f, 50f, 25f };

	public Vector3 LocalUp;
	public Vector3 AxisA;
	public Vector3 AxisB;
	public Vector2 PatchOffset;
	public float PatchScale;
	public float Radius;

	private int currentLODIndex = -1;
	private int frameCounter = 0;
	private int lodCheckFrameInterval = 60;
	private QuadtreePatch[] children = null;
	private bool isInitialized = false;

	public override void _Ready()
	{
			
		LODResolutions = new int[] { 8, 16, 32, 64, 128 };
		LodDistanceThresholds = new float[] { 400f, 200f, 100f, 50f, 25f };
		// DEBUG
		//if (TerrainProfile != null && MeshGenerator != null)
		//{
			//GD.Print("[QuadtreePatch] _Ready generating test mesh");
			//var camera = GetViewport().GetCamera3D();
			//float distance = GlobalPosition.DistanceTo(camera.GlobalPosition);
			//int lodIndex = GetLODLevel(distance);
			//Mesh = MeshGenerator.GeneratePatchMesh(
					//LocalUp,
					//AxisA,
					//AxisB,
					//PatchOffset,
					//PatchScale,
					//Radius,
					//TerrainProfile,
					//LODResolutions[lodIndex]
				//);
		//}
	}
	
	public void Initialize()
	{
		isInitialized = true;
		//GD.Print("[QuadtreePatch] Initialize() called");
	}

	public override void _Process(double delta)
	{
		// DEBUG
		//GD.Print($"[QuadtreePatch] Processing patch at depth {CurrentDepth}, scale {PatchScale}, offset {PatchOffset}");
		if (!isInitialized) {
			return;
		}

		if (TerrainProfile == null)
		{
			GD.PrintErr($"[QuadtreePatch] ERROR: TerrainProfile is null");
			return;
		}

		if (MeshGenerator == null)
		{
			GD.PrintErr($"[QuadtreePatch] ERROR: MeshGenerator is null");
			return;
		}

		frameCounter++;
		if (frameCounter < lodCheckFrameInterval) 
			return;
		frameCounter = 0;

		var camera = GetViewport().GetCamera3D();
		if (camera == null || Radius <= 0f || TerrainProfile == null || MeshGenerator == null) 
			return;

		float distance = GlobalPosition.DistanceTo(camera.GlobalPosition);
		int lodIndex = GetLODLevel(distance);

		// Subdivide if needed
		// Temporary remove due to infinite calls
		//if (lodIndex > CurrentDepth && CurrentDepth < MaxDepth)
		//{
			//if (children == null) {
				//GD.Print($"[QuadtreePatch] Calling subdividing...");
				//Subdivide();
			//}
		//}
		//else if (children != null && CurrentDepth > 0)
		//{
			//GD.Print($"[QuadtreePatch] Collapsing...");
			//Collapse();
		//}
		
		if (children == null && lodIndex != currentLODIndex)
		{
				GD.Print($"[QuadtreePatch] Generating mesh at LOD index {lodIndex}");
				GD.Print(LODResolutions);
				var  resolution = LODResolutions[lodIndex];
				GD.Print($"[QuadtreePatch] Generating mesh at LOD {resolution}");
				GD.Print($"[QuadtreePatch] MeshGenerator is set? {MeshGenerator != null}");

				var mesh = MeshGenerator.GeneratePatchMesh(
					LocalUp,
					AxisA,
					AxisB,
					PatchOffset,
					PatchScale,
					Radius,
					TerrainProfile,
					LODResolutions[lodIndex]
				);
				
				var material = new ShaderMaterial();
				material.Shader = GD.Load<Shader>("res://Shaders/TerrainColorShader.gdshader"); // Adjust path as needed

				// Set shader uniform
				material.SetShaderParameter("max_height", Radius);

				// Apply the material to the mesh
				mesh.SurfaceSetMaterial(0, material);

				currentLODIndex = lodIndex;
				
				var meshInstance = GetNode<MeshInstance3D>("MeshInstance3D");
				meshInstance.Mesh = mesh;
				
				// DEBUG 
				//GD.Print($"[DEBUG] Camera position: {camera.GlobalPosition}");
//
				//var aabb = mesh.GetAabb();
				//GD.Print($"[Quadtree] Mesh AABB: {aabb.Position}, {aabb.Size}");
				
				// Add materials
				//var mat = new StandardMaterial3D();
				//mat.AlbedoColor = new Color(0.4f, 0.9f, 0.6f); 
				//mesh.SurfaceSetMaterial(0, mat);
			try
			{				
				// Add collision
				var collision = GetNode<CollisionShape3D>("RigidBody3D/CollisionShape3D");
				var shape = new ConcavePolygonShape3D();
				shape.Data = mesh.GetFaces();
				collision.Shape = shape;
			}
			catch (Exception ex)
			{
				GD.PrintErr($"[QuadtreePatch] CRASHED during mesh gen: {ex.Message}");
			}
		}
	}

	private int GetLODLevel(float distance)
	{
		for (int i = 0; i < LodDistanceThresholds.Length; i++)
		{
			if (distance > LodDistanceThresholds[i])
				return i;
		}
		return LodDistanceThresholds.Length-1;
	}

	private void Subdivide()
	{
		children = new QuadtreePatch[4];
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				var child = QuadtreePatchScene.Instantiate<QuadtreePatch>();
				if (TerrainProfile == null)
				{
					GD.PrintErr($"[QuadtreePatch] TerrainProfile is null at offset {PatchOffset}");
					return;
				}

				float half = PatchScale * 0.5f;
				child.PatchScale = half;
				child.PatchOffset = this.PatchOffset + new Vector2(i * half, j * half);
				child.LocalUp = this.LocalUp;
				child.AxisA = this.AxisA;
				child.AxisB = this.AxisB;
				child.Radius = this.Radius;
				child.MeshGenerator = this.MeshGenerator;
				child.TerrainProfile = this.TerrainProfile;
				child.CurrentDepth = this.CurrentDepth + 1;
				child.MaxDepth = this.MaxDepth;
				child.LODResolutions = this.LODResolutions;
				child.LodDistanceThresholds = this.LodDistanceThresholds;
				children[i * 2 + j] = child;
				
				child.QuadtreePatchScene = this.QuadtreePatchScene;

				child.Initialize();
				AddChild(child);
				// DEBUG
				//GD.Print($"[Subdivide] Creating child patch at depth {child.CurrentDepth}, offset {child.PatchOffset}");
			}
		}

		// Clear the mesh on the local MeshInstance3D
		var meshInstance = GetNode<MeshInstance3D>("MeshInstance3D");
		meshInstance.Mesh = null;

		// Clear collision
		if (HasNode("CollisionShape3D"))
		{
			var collision = GetNode<CollisionShape3D>("CollisionShape3D");
			collision.Shape = null;
		}
	}

	private void Collapse()
	{
		foreach (var child in children)
		{
			if (child != null && child.IsInsideTree())
				child.QueueFree();
		}
		children = null;
	}
}
