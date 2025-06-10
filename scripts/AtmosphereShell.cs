using Godot;
using System;

public partial class AtmosphereShell : MeshInstance3D
{
	[Export] public Node3D PlanetBody;
	[Export] public float ScaleFactor = 1.025f;
	[Export] public DirectionalLight3D SunLight;
	
	public override void _Process(double delta)
	{
		if (PlanetBody != null)
		{
			GlobalPosition = PlanetBody.GlobalPosition;

			// Match terrain radius and scale
			float terrainRadius = PlanetBody.Scale.X;
			Scale = new Vector3(terrainRadius, terrainRadius, terrainRadius) * ScaleFactor;
		}
		
		if (SunLight != null)
		{
			// Get light direction as unit vector
			Vector3 lightDir = -SunLight.GlobalTransform.Basis.Z;
			var material = (ShaderMaterial)Mesh.SurfaceGetMaterial(0);
			material.SetShaderParameter("light_dir", lightDir);
		}
	}
}
