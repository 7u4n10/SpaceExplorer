using Godot;
using System;

// version 2
public partial class DebugCamera : Camera3D
{
	[Export] public float MoveSpeed = 10f;
	[Export] public float LookSpeed = 0.003f;
	public Vector2 _rotation = Vector2.Zero;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			_rotation.X = Mathf.Clamp(_rotation.X - motion.Relative.Y * LookSpeed, -Mathf.Pi / 2f, Mathf.Pi / 2f);
			_rotation.Y -= motion.Relative.X * LookSpeed;
			Rotation = new Vector3(_rotation.X, _rotation.Y, 0);
		}

		if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = Vector3.Zero;

		if (Input.IsActionPressed("ui_up")) direction -= Transform.Basis.Z;
		if (Input.IsActionPressed("ui_down")) direction += Transform.Basis.Z;
		if (Input.IsActionPressed("ui_left")) direction -= Transform.Basis.X;
		if (Input.IsActionPressed("ui_right")) direction += Transform.Basis.X;
		if (Input.IsActionPressed("move_up")) direction += Transform.Basis.Y;
		if (Input.IsActionPressed("move_down")) direction -= Transform.Basis.Y;

		Position += direction.Normalized() * MoveSpeed * (float)delta;
	}
}

// version 1
//public partial class DebugCamera : Camera3D
//{
	//[Export] public float MoveSpeed = 30f;
	//[Export] public float LookSpeed = 0.003f;
	//[Export] public bool IsFreeFly = true;
	//
	//private Vector2 _rotation = Vector2.Zero;
//
	//public override void _Ready()
	//{
		//Input.MouseMode = Input.MouseModeEnum.Captured;
		//GD.Print("DebugCamera ready");
	//}
//
	//public override void _Input(InputEvent @event)
	//{
		//if (@event is InputEventMouseMotion motion)
		//{
			//_rotation.X -= motion.Relative.Y * LookSpeed;
			//_rotation.Y -= motion.Relative.X * LookSpeed;
			//Rotation = new Vector3(_rotation.X, _rotation.Y, 0);
		//}
//
		//if (@event.IsActionPressed("ui_cancel"))
		//{
			//Input.MouseMode = Input.MouseModeEnum.Visible;
		//}
		//
		//if (@event.IsActionPressed("toggle_freecam"))
		//{
			//IsFreeFly = true;
			//Input.MouseMode = Input.MouseModeEnum.Captured;
		//}
	//}
//
	//public override void _PhysicsProcess(double delta)
	//{
		//if (!IsFreeFly)
			//return;
			//
		//Vector3 direction = Vector3.Zero;
//
		//if (Input.IsActionPressed("ui_up")) direction -= Transform.Basis.Z;
		//if (Input.IsActionPressed("ui_down")) direction += Transform.Basis.Z;
		//if (Input.IsActionPressed("ui_left")) direction -= Transform.Basis.X;
		//if (Input.IsActionPressed("ui_right")) direction += Transform.Basis.X;
		//if (Input.IsActionPressed("move_up")) direction += Transform.Basis.Y;
		//if (Input.IsActionPressed("move_down")) direction -= Transform.Basis.Y;
//
		//Position += direction.Normalized() * MoveSpeed * (float)delta;
	//}
//}
