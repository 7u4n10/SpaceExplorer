using Godot;
using System;
using System.Collections.Generic;

public partial class MainScene : Node3D
{
	[Export] public float G = 1f; //6.67430e-11f;
	private List<PlanetBody> _bodies = new();

	// version 1
	// Spawn a planet and a moon
	//public override void _Ready()
	//{
		//float centerMass = 2000f;
		//float orbitMass = 50f;
		//float r = 20f;
		//float orbitSpeed = Mathf.Sqrt(G * centerMass / r);
		//SpawnPlanet(new Vector3(0, 0, 0), new Vector3(0, 0, 0), centerMass); // Central planet
		//SpawnPlanet(new Vector3(20, 0, 0), new Vector3(0, 0, orbitSpeed), orbitMass); // Orbiting planet
	//}
	//
	// version 1
	// Spawn planet and moon
	//private void SpawnPlanet(Vector3 position, Vector3 initialVelocity, float mass)
	//{
		//var planetScene = GD.Load<PackedScene>("res://scenes/PlanetBody.tscn");
		//var planet = planetScene.Instantiate<PlanetBody>();
//
		//planet.GlobalPosition = position;
		//planet.Mass = mass;
		//planet.Velocity = initialVelocity;
//
		//// Randomize appearance
		//float radius = Mathf.Lerp(0.8f, 1.5f, GD.Randf());
		//planet.Scale = new Vector3(radius, radius, radius);
		//planet.SetColor(new Color(GD.Randf(), GD.Randf(), GD.Randf()));
//
		//AddChild(planet);
		//_bodies.Add(planet);
	//}
	
	// version 2
	public override void _Ready()
	{
		var center = SpawnPlanet(Vector3.Zero, Vector3.Zero, 2000f);

		SpawnOrbitingBody(center, 10f, 30f);
		SpawnOrbitingBody(center, 20f, 50f);
		SpawnOrbitingBody(center, 15f, 60f);
	}

	private PlanetBody SpawnPlanet(Vector3 position, Vector3 initialVelocity, float mass)
	{
		var planetScene = GD.Load<PackedScene>("res://scenes/PlanetBody.tscn");
		var planet = planetScene.Instantiate<PlanetBody>();

		planet.GlobalPosition = position;
		planet.Mass = mass;
		planet.Velocity = initialVelocity;

		// Appearance
		float radius = Mathf.Lerp(0.8f, 1.5f, GD.Randf());
		planet.Scale = new Vector3(radius, radius, radius);
		planet.SetColor(new Color(GD.Randf(), GD.Randf(), GD.Randf()));

		AddChild(planet);
		_bodies.Add(planet);
		
		planet.Connect("PlanetClicked", new Callable(this, nameof(OnPlanetClicked)));
		return planet;
	}

	public override void _PhysicsProcess(double delta)
	{
		for (int i = 0; i < _bodies.Count; i++)
		{
			PlanetBody A = _bodies[i];
			Vector3 totalForce = Vector3.Zero;

			for (int j = 0; j < _bodies.Count; j++)
			{
				if (i == j) continue;

				PlanetBody B = _bodies[j];
				Vector3 direction = B.GlobalPosition - A.GlobalPosition;
				float distance = direction.Length();

				if (distance < 1f) continue; // avoid divide-by-zero

				float forceMag = G * A.Mass * B.Mass / (distance * distance);
				Vector3 force = direction.Normalized() * forceMag;

				totalForce += force / A.Mass;
			}

			A.ApplyGravity(totalForce);
		}
	}
	
	private void SpawnOrbitingBody(PlanetBody center, float distance, float satelliteMass)
	{
		// Calculate orbit position: place satellite on +X axis
		Vector3 position = center.GlobalPosition + new Vector3(distance, 0, 0);

		// Calculate orbital velocity magnitude
		float orbitSpeed = Mathf.Sqrt(G * center.Mass / distance);

		// Direction: perpendicular to radius (in Z+ direction here)
		Vector3 velocity = new Vector3(0, 0, orbitSpeed);

		// Add central body's velocity if it's moving
		velocity += center.Velocity;

		SpawnPlanet(position, velocity, satelliteMass);
	}

	private void OnPlanetClicked(PlanetBody planet)
	{
		var camera = GetNode<Camera3D>("MainCamera");
		
		if (camera is DebugCamera debugCamera)
			debugCamera.IsFreeFly = false;

		Vector3 targetPosition = planet.GlobalPosition + new Vector3(0, 5, 10); // Adjust as needed
		camera.GlobalPosition = targetPosition;
		camera.LookAt(planet.GlobalPosition, Vector3.Up);
	}
}
