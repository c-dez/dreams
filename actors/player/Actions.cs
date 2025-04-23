using Godot;
using System;

public partial class Actions : Node
{
    private AnimationTree animationTree;
    private bool isAttacking = false;
    public override void _Ready()
    {
        animationTree = GetNode<AnimationTree>("../AnimationTree");
    }

    public override void _PhysicsProcess(double delta)
    {
        Attack();
    }

    private void Attack()
    {
        if (Input.IsActionJustPressed("mb1") && !isAttacking)
        {
            animationTree.Set("parameters/AttackOneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }
    }


    private void AttackingToggle(bool value)
    {
        isAttacking = value;
    }
}
