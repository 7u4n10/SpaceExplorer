//using Godot;
//using System;
//
//public partial class PlanetBody : RigidBody3D
//{
	//[Export] public new float Mass = 100f;
	//[Export] public Vector3 Velocity = Vector3.Zero;
//
	//public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	//{
		//state.LinearVelocity = Velocity;
	//}
//
	//public void ApplyGravity(Vector3 force)
	//{
		//Velocity += force * (float)GetPhysicsProcessDeltaTime();
	//}
//
	//public void SetColor(Color color)
	//{
		//var material = new StandardMaterial3D();
		//material.AlbedoColor = color;
		//MeshInstance3D visual = GetNode<MeshInstance3D>("Visual");
		//visual.SetSurfaceOverrideMaterial(0, material);
	//}
	//
	//public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	//{
		//if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		//{
			//GD.Print(Name + " was clicked.");
			//EmitSignal("PlanetClicked", this);
		//}
	//}
//
//}


using Godot;
using System;

public partial class PlanetBody : Node3D
{
	[Export] public float Mass = 1000f;
	[Export] public float Radius = 10f;
	[Export] public Vector3 Velocity = Vector3.Zero;

	private PlanetVisual _visual;

	public override void _Ready()
	{
		_visual = GetNode<PlanetVisual>("PlanetVisual");
		_visual.GenerateSurface(Radius);
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
