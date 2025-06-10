using Godot;
using System;

public partial class PlanetVisual : Node3D
{
	[Export] public int Resolution = 32;
	[Export] public float ElevationAmplitude = 2f;
	[Export] public PackedScene QuadtreePatchScene;

	public float Radius = 10f;
	private TerrainNoiseProfile terrainProfile;
	private IPatchMeshGenerator meshGenerator = new DefaultPatchGenerator();

	public override void _Ready()
	{
		// DEBUG
		//GD.Print("[PlanetVisual] _Ready() called");

		terrainProfile = TerrainProfiles.Rocky;
		terrainProfile.Initialize();
		
		GenerateSurface(Radius);
		
		// DEBUG
		//GD.Print($"[PlanetVisual] is ready");
	}

	public void GenerateSurface(float radius)
	{
		Radius = radius;
		
		foreach (var faceNode in GetChildren())
		{
			if (faceNode is Node3D face)
			{
				GeneratePatchesForFace(face.Name, face);
			}
		}
	}
	
	private void GeneratePatchesForFace(string faceName, Node3D parent)
	{
		Vector3 localUp = faceName switch
		{
			"PlanetFace_Up" => Vector3.Up,
			"PlanetFace_Down" => Vector3.Down,
			"PlanetFace_Left" => Vector3.Left,
			"PlanetFace_Right" => Vector3.Right,
			"PlanetFace_Forward" => Vector3.Forward,
			"PlanetFace_Back" => Vector3.Back,
			_ => Vector3.Up
		};

		Vector3 axisA = new Vector3(localUp.Y, localUp.Z, localUp.X);
		Vector3 axisB = localUp.Cross(axisA);

		//parent.CallDeferred("clear_surfaces");

		int patchCount = 2;
		for (int py = 0; py < patchCount; py++)
		{
			for (int px = 0; px < patchCount; px++)
			{
				// DEBUG
				if (QuadtreePatchScene == null)
				{
					GD.PrintErr("[PlanetVisual] ERROR: QuadtreePatchScene is null!");
					return;
				} else  {
					
				}

				string patchName = $"Patch_{px}_{py}";
				float patchScale = 1f / patchCount;
				Vector2 offset = new Vector2(px, py) * patchScale;
				
				var patchNode = QuadtreePatchScene.Instantiate<QuadtreePatch>();

				patchNode.Name = patchName;
				patchNode.MeshGenerator = meshGenerator;
				patchNode.TerrainProfile = terrainProfile;
				patchNode.Radius = Radius;
				patchNode.LocalUp = localUp;
				patchNode.AxisA = axisA;
				patchNode.AxisB = axisB;
				patchNode.PatchOffset = offset;
				patchNode.PatchScale = patchScale;
				//patchNode.CurrentDepth = 0;
				patchNode.MaxDepth = 5;
				patchNode.QuadtreePatchScene = QuadtreePatchScene;

				// DEBUG
				//GD.Print($"[PlanetVisual] Instantiating patch {patchName} for face {faceName}");
				//GD.Print($"[PlanetVisual] Assigned profile to patch {patchName}? {patchNode.TerrainProfile != null}");

				patchNode.Initialize(); 
				parent.AddChild(patchNode);
			}
		}
	}
}
