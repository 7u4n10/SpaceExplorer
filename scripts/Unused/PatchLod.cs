using Godot;
using System;

// Deprecated
// Move onto QuadtreePatch.cs
public partial class PatchLOD : MeshInstance3D
{
	//[Export] public float[] LODDistances = new float[] { 150f, 75f, 30f };
	[Export] public int[] LODResolutions = new int[] { 8, 16, 32, 64 };
	public float[] LodDistanceThresholds { get; set; } = new float[] {200f, 100f, 50f, 25f};

	public Vector3 LocalUp;
	public Vector3 AxisA;
	public Vector3 AxisB;
	public Vector2 PatchOffset;
	public float PatchScale;
	public float Radius;
	//public TerrainNoiseProfile Profile;

	//private IPatchMeshGenerator generator = new DefaultPatchGenerator();
	private int currentLODIndex = -1;
	private int lodCheckFrameInterval = 10;
	private int currentFrameCounter = 0;

	public IPatchMeshGenerator MeshGenerator { get; set; }
	public TerrainNoiseProfile TerrainProfile { get; set; }

	public override void _Process(double delta)
	{
		// DEBUG
		GD.Print($"[PatchLOD] Generating mesh.");
		
		// Update cooldown
		currentFrameCounter++;
		if (currentFrameCounter < lodCheckFrameInterval)
			return;

		currentFrameCounter = 0;
		
		if (TerrainProfile  == null || Radius <= 0f)
			return;
			
		// Check camera distance and update LOD
		Camera3D camera = GetViewport().GetCamera3D();
		if (camera == null) return;

		float distance = GlobalPosition.DistanceTo(camera.GlobalPosition);
		int lodIndex = GetLODLevel(distance);

		if (lodIndex != currentLODIndex)
		{
			Mesh = MeshGenerator.GeneratePatchMesh(
				LocalUp,
				AxisA,
				AxisB,
				PatchOffset,
				PatchScale,
				Radius,
				TerrainProfile,
				LODResolutions[lodIndex]
			);
			currentLODIndex = lodIndex;
			
			// DEBUG
			GD.Print($"[PatchLOD] LOD switched to resolution {lodIndex}");
			
			if (TerrainProfile == null)
			{
				GD.PrintErr("[PatchLOD] TerrainProfile is null!");
				Mesh = new BoxMesh(); 
				return;
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
		return LodDistanceThresholds.Length;
	}
}
