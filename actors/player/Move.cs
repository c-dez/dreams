using Godot;
using System;
using System.ComponentModel.Design;
using static Godot.GD;
namespace Dreams.Actors.Players
{


    public partial class Move : Node
    {
        [Export] public CharacterBody3D player;
        [Export] public Node3D camera;
        [Export] public Node3D skin;
        private AnimationNodeStateMachinePlayback moveStateMachine;
        private float speedModifier = 1.0f;
        private Vector3 lastMovementDirection = Vector3.Back;
        private float rotationSpeed = 12.0f;
        [Export] public float speed = 8.0f;

        //jump
        [Export] float jumpHeight; //8.0
        [Export] float jumpTimeToPeak; //0.6
        [Export] float jumpTimeToDecend; //0.4

        float coyoteTimeMax = 0.3f;
        float jumpBufferMax = 0.5f;

        float coyoteTimeCounter = 0f;
        float jumpBufferCounter = 0f;

        float jumpVelocity;
        float jumpGravity;
        float fallGravity;

        Vector3 lastMoveDirection = Vector3.Zero;
        public override void _Ready()

        {
            moveStateMachine = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("../AnimationTree").Get("parameters/MoveStateMachine/playback");
            jumpVelocity = (2.0f * jumpHeight) / jumpTimeToPeak;
            jumpGravity = (-2.0f * jumpHeight) / (jumpTimeToPeak * jumpTimeToPeak);
            fallGravity = (-2.0f * jumpHeight) / (jumpTimeToDecend * jumpTimeToDecend);

        }

        public override void _PhysicsProcess(double delta)
        {
            MoveLogic((float)delta);
        }

        private void MoveLogic(float _delta)
        {
            Vector3 velocity = player.Velocity;
            float _speed = speed * speedModifier;

            LastMoveDirection(GetMoveDirection());


            MoveOnFloor(GetMoveDirection(), _speed, velocity);
            Jump(velocity, _delta);
            CameraRotation(GetMoveDirection(), _delta);
        }


        private Vector3 GetMoveDirection()
        {
            Vector2 rawInput = Input.GetVector("left", "right", "forward", "backwards");
            Vector3 forward = camera.GlobalBasis.Z;
            Vector3 right = camera.GlobalBasis.X;
            Vector3 moveDirection = forward * rawInput.Y + right * rawInput.X;
            moveDirection = moveDirection.Normalized();
            return moveDirection;


        }
        private void MoveOnFloor(Vector3 moveDirection, float _speed, Vector3 velocity)
        {
            if (player.IsOnFloor())
            {
                if (moveDirection.Length() > 0.2f)
                {
                    velocity.X = moveDirection.X * _speed;
                    velocity.Z = moveDirection.Z * _speed;
                    if (player.IsOnFloor())
                    {
                        moveStateMachine.Travel("run");
                    }
                }
                else
                {
                    velocity.X = Mathf.MoveToward(player.Velocity.X, 0, _speed);
                    velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, _speed);

                    if (player.IsOnFloor())
                    {
                        moveStateMachine.Travel("idle");
                    }
                }

            }
            // bloquear direccion de salto en el aire, no me gusta como se siente
            else
            {
                {
                    // EN AIRE TEST
                    // velocity.X = lastMoveDirection.X * _speed;
                    // velocity.Z = lastMoveDirection.Z * _speed;

                    // EN SUELO TEST
                    velocity.X = moveDirection.X * _speed;
                    velocity.Z = moveDirection.Z * _speed;




                }

            }
            /////////////////////////////////////////////////////////////////
            player.Velocity = velocity;
            player.MoveAndSlide();
        }

        private void MoveOnAir(Vector3 lastMoveDirection, float _speed, Vector3 velocity)
        {
            if (!player.IsOnFloor())
            {
                velocity.X = lastMoveDirection.X * _speed;
                velocity.Z = lastMoveDirection.Z * _speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(player.Velocity.X, 0, _speed);
                velocity.Z = Mathf.MoveToward(player.Velocity.Z, 0, _speed);

                if (player.IsOnFloor())
                {
                    moveStateMachine.Travel("idle");
                }
            }

            player.Velocity = velocity;
            player.MoveAndSlide();
        }


        private void LastMoveDirection(Vector3 moveDirection)
        {
            if (player.IsOnFloor())
            {
                lastMoveDirection = moveDirection;
            }
            else
            {
                GD.Print(lastMoveDirection);
            }
        }


        private void Jump(Vector3 velocity, float _delta)
        {
            if (Input.IsActionJustPressed("space") && coyoteTimeCounter > 0f)
            {


                velocity.Y = jumpVelocity;
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            //on air
            if (!player.IsOnFloor())
            {
                moveStateMachine.Travel("falling");
                if (velocity.Y < 0.0f)
                {
                    velocity.Y += fallGravity * _delta;
                }
                else
                {
                    velocity.Y += jumpGravity * _delta;
                }
            }

            //coyote time
            coyoteTimeCounter = player.IsOnFloor() ? coyoteTimeMax : coyoteTimeCounter - _delta;
            //jump buffer
            // jumpBufferCounter = Input.IsActionJustPressed("space") ? jumpBufferMax : - _delta;

            player.Velocity = velocity;
            player.MoveAndSlide();
        }


        private void CameraRotation(Vector3 moveDirection, float _delta)
        {
            if (moveDirection.Length() > 0.2f)
            {
                lastMovementDirection = moveDirection;
            }
            float targetAngle = Vector3.Back.SignedAngleTo(lastMovementDirection, Vector3.Up);
            Vector3 globalRotation = skin.GlobalRotation;
            globalRotation.Y = Mathf.LerpAngle(globalRotation.Y, targetAngle, rotationSpeed * _delta);
            skin.GlobalRotation = globalRotation;
        }

    }
}