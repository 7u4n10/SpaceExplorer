using Godot;
using System;

public partial class AtmosphereShell : MeshInstance3D
{
	[Export] public Node3D PlanetBody;
	[Export] public float ScaleFactor = 2.5f;
	[Export] public DirectionalLight3D SunLight;
	private float _planetRadius = 100f;
	
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		ApplyScale();
	}

	public override void _Process(double delta)
	{
		if (PlanetBody != null)
		{
			ApplyScale();
		}
		
		if (SunLight != null)
		{
			// Get light direction as unit vector
			Vector3 lightDir = -SunLight.GlobalTransform.Basis.Z;
			var material = (ShaderMaterial)Mesh.SurfaceGetMaterial(0);
			material.SetShaderParameter("light_dir", lightDir);
		}
	}
	
	public void SetPlanetRadius(float radius)
	{
		_planetRadius = radius;
		ApplyScale();
	}
	
	private void ApplyScale()
	{
		float atmosphereRadius = _planetRadius * ScaleFactor;
		Scale = new Vector3(atmosphereRadius, atmosphereRadius, atmosphereRadius);
	}
}
