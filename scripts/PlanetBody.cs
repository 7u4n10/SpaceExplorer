using Godot;
using System;

public partial class PlanetBody : RigidBody3D
{
	[Export] public new float Mass = 100f;
	[Export] public Vector3 Velocity = Vector3.Zero;

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		state.LinearVelocity = Velocity;
	}

	public void ApplyGravity(Vector3 force)
	{
		Velocity += force * (float)GetPhysicsProcessDeltaTime();
	}

	public void SetColor(Color color)
	{
		var material = new StandardMaterial3D();
		material.AlbedoColor = color;
		MeshInstance3D visual = GetNode<MeshInstance3D>("Visual");
		visual.SetSurfaceOverrideMaterial(0, material);
	}
	
	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GD.Print(Name + " was clicked.");
			EmitSignal("PlanetClicked", this);
		}
	}

}
