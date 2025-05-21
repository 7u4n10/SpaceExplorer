using Godot;
using System;
using System.Collections.Generic;

public partial class PlanetSystemManager : Node
{
	public static PlanetSystemManager Instance { get; private set; }

	private List<PlanetBody> planets = new();

	[Export] public float GravitationalConstant = 0.1f;

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddPlanet(PlanetBody planet)
	{
		if (!planets.Contains(planet))
			planets.Add(planet);
	}

	public override void _PhysicsProcess(double delta)
	{
		ApplyGravity((float)delta);
		UpdatePlanetPositions((float)delta);
	}

	private void ApplyGravity(float delta)
	{
		for (int i = 0; i < planets.Count; i++)
		{
			PlanetBody a = planets[i];
			Vector3 totalForce = Vector3.Zero;

			for (int j = 0; j < planets.Count; j++)
			{
				if (i == j) continue;

				PlanetBody b = planets[j];
				Vector3 direction = b.GlobalPosition - a.GlobalPosition;
				float distance = direction.Length();
				if (distance < 1f) continue;

				float forceMagnitude = GravitationalConstant * a.Mass * b.Mass / (distance * distance);
				Vector3 force = direction.Normalized() * forceMagnitude;
				totalForce += force;
			}

			a.ApplyForce(totalForce, delta);
		}
	}

	private void UpdatePlanetPositions(float delta)
	{
		foreach (var planet in planets)
		{
			planet.PhysicsStep(delta);
		}
	}

	public void RemovePlanet(PlanetBody planet)
	{
		if (planets.Contains(planet))
			planets.Remove(planet);
	}

	public void ClearAll()
	{
		planets.Clear();
	}
}
