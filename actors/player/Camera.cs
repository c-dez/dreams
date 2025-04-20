using Godot;
using System;

public partial class Camera : Node3D
{
    [Export] public Node3D skin;
    private float mouseSensitivity = 0.25f / 3;
    private Vector2 cameraInputDirection = Vector2.Zero;


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            cameraInputDirection = mouseMotion.Relative * mouseSensitivity;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        HideSkinOnCameraTooClose();
        float dt = (float)delta;
        Vector3 rot = Rotation;
        rot.X -= cameraInputDirection.Y * dt;
        rot.Y -= cameraInputDirection.X * dt;
        rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(-45), Mathf.DegToRad(30));
        Rotation = rot;
        cameraInputDirection = Vector2.Zero;
    }

    private void HideSkinOnCameraTooClose()
    {
        if (GetNode<SpringArm3D>("SpringArm3D").GetHitLength() < 1.5f)
        {
            skin.Visible = false;
        }
        else
        {
            skin.Visible = true;
        }
    }
}
