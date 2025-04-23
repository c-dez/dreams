using Godot;
using System;

public partial class Actions : Node
{
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	private void Attack()
	{
		if (Input.IsActionJustPressed("mb1"))
		{
			GD.Print("mn1");
		}
	}
}
