using Godot;
using System;

public partial class Camera : Node3D
{
    private Node3D skin;
    private float mouseSensitivity = 0.25f / 3;
    private Vector2 cameraInputDirection = Vector2.Zero;


    public async override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        skin = GetNode<Node3D>("../Skin");
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
        CameraRotation((float)delta);
        SkinRotation((float)delta);
    }

    private void CameraRotation(float delta)
    {
        Vector3 rot = Rotation;
        rot.X -= cameraInputDirection.Y * delta;
        rot.Y -= cameraInputDirection.X * delta;
        rot.X = Mathf.Clamp(rot.X, Mathf.DegToRad(-45), Mathf.DegToRad(30));
        Rotation = rot;
        // skin.RotateY((cameraInputDirection.X * delta));

        cameraInputDirection = Vector2.Zero;
    }

    private void SkinRotation(float delta)
    {
        Vector3 moveDirection = GetNode<UserInputs>("../UserInputs").moveDirection;
        if (moveDirection.Length() < 0.2f)
        {
            skin.RotateY(cameraInputDirection.X * delta);
        }
        else
        {
            moveDirection.Y = 0;
            Vector3 targetPos = skin.GlobalTransform.Origin - moveDirection;
            skin.LookAt(targetPos, Vector3.Up);
        }
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
