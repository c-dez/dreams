using Godot;
using System;

public partial class Actions : Node
{
	private AnimationNodeOneShot attackOneShot;
	public override void _Ready()
	{
		// attackOneShot =(AnimationNodeOneShot) GetNode<AnimationNodeOneShot>("../AnimationTree").Get("parameters/AttackOneShot/request");
	}

	public override void _PhysicsProcess(double delta)
	{
		Attack();
	}

	private void Attack()
	{
		if (Input.IsActionJustPressed("mb1"))
		{
			GD.Print("mb1");
			GetNode<AnimationTree>("../AnimationTree").Set("parameters/AttackOneShot/request",  (int)AnimationNodeOneShot.OneShotRequest.Fire);


		}
	}
}
