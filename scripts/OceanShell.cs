using Godot;
using System;

public partial class OceanShell : MeshInstance3D
{
	[Export] public Node3D PlanetBody;
	[Export] public float ScaleFactor = 2.0f; // Slightly above terrain
	private float _planetRadius = 100f;
	
	public override void _Ready()
	{		
		ProcessMode = ProcessModeEnum.Always;
		ApplyScale();
	}
	
	public void SetPlanetRadius(float radius)
	{
		_planetRadius = radius;
		ApplyScale();
	}
	
	private void ApplyScale()
	{
		float oceanRadius = _planetRadius * ScaleFactor;
		Scale = new Vector3(oceanRadius, oceanRadius, oceanRadius);
	}
}
