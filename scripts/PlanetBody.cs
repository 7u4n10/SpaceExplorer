using Godot;
using System;

public partial class PlanetBody : Node3D
{
	[Export] public float Mass = 15000;
	[Export] public float Radius = 25;
	[Export] public Vector3 Velocity = Vector3.Zero;

	private PlanetVisual _visual;

	public override void _Ready()
	{
		// DEBUG
		//GD.Print("[PlanetBody] Calling PlanetSystemManager");

		PlanetSystemManager.Instance?.AddPlanet(this);
		
		// DEBUG
		//GD.Print("[PlanetBody] Calling PlanetVisual().GenerateSurface");

		try {
			var visScene = GD.Load("res://scenes/PlanetVisual.tscn") as PackedScene;
			_visual = visScene.Instantiate() as PlanetVisual;
			_visual.Radius = Radius;
			if (_visual == null)
			{
				GD.PrintErr("[PlanetBody] ERROR: PlanetVisual not found!");
				return;
			}
			AddChild(_visual);
			_visual.SetOwner(this);

			//_visual.GenerateSurface(Radius);
		} catch (Exception e) {
			GD.PrintErr($"[PlanetBody] CRASHED: {e.Message}");
		}
		
		// DEBUG
		GD.Print($"[PlanetBody] is ready with Radius {Radius}, Mass {Mass}");
	}
	
	public override void _ExitTree()
	{
		// DEBUG
		GD.Print("[PlanetBody/_ExitTree()] ...");
		PlanetSystemManager.Instance?.RemovePlanet(this);
	}

	public void ApplyForce(Vector3 force, float delta)
	{
		Velocity += force / Mass * delta;
	}

	public void PhysicsStep(float delta)
	{
		GlobalPosition += Velocity * delta;
	}

	public void MergeWith(PlanetBody other)
	{
		float totalMass = Mass + other.Mass;
		Vector3 newVelocity = (Velocity * Mass + other.Velocity * other.Mass) / totalMass;

		Mass = totalMass;
		Velocity = newVelocity;
		Radius = Mathf.Pow(totalMass, 1f / 3f);

		other.QueueFree();
		_visual.GenerateSurface(Radius);
	}
}
