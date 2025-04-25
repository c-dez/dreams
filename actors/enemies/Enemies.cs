using Godot;

namespace Actors
{
    public partial class Enemies : CharacterBody3D
    {

        public CharacterBody3D player;

        [ExportGroup("Base Stats")]
        [Export] private int Health = 100;
        [Export] private float speed = 5.0f;
        [Export] private int attackDamage = 5;

        [ExportGroup("Detection Radius")]
        [Export] private float noticeRadius = 10;
        [Export] private float attackRadius = 3;

        [ExportGroup("Nodes")]
        [Export] private MeshInstance3D skin;
        [Export] private AnimationTree animationTree;

        public override void _Ready()
        {
            player = (CharacterBody3D)GetTree().GetFirstNodeInGroup("Player");
        }


        public override void _PhysicsProcess(double delta)
        {
            MoveToPlayer((float)delta);
            Gravity();
        }

        public virtual void MoveToPlayer(float delta)
        {
            // en este caso si puedo modificar Velocity, dejo comentarios de la forma "vieja" como recordatorio
            // Vector3 velocity = Velocity;
            if (IsPlayerInNoticeRadius())
            {
                Vector3 targetDirection = (player.Position - Position).Normalized();
                Vector2 TargetVector = new(targetDirection.X, targetDirection.Z);
                float targetAngle = -TargetVector.Angle() + Mathf.Pi / 2;
                Vector3 rotation = Rotation;
                rotation.Y = Mathf.RotateToward(Rotation.Y, targetAngle, delta * 6.0f);
                Rotation = rotation;

                if (Position.DistanceTo(player.Position) > attackRadius)
                {
                    // velocity = new Vector3(TargetVector.X, 0, TargetVector.Y) * speed;
                    Velocity = new Vector3(TargetVector.X, 0, TargetVector.Y) * speed;
                }
                else
                {
                    // velocity = Vector3.Zero;
                    Velocity = Vector3.Zero;

                }
                // Velocity = velocity;
                MoveAndSlide();
            }
        }

        private void Gravity()
        {
            if (!IsOnFloor())
            {
                Vector3 velocity = Velocity;
                velocity.Y -= 10f;
                Velocity = velocity;
                MoveAndSlide();
            }
        }

        public void OnDead()
        {
            if (Health <= 0)
            {
                GD.Print($"{Name} is dead");
            }
        }

        public virtual void RangeAttack()
        {
            GD.Print("range attack");
        }

        public virtual void AttackMelee()
        {
            GD.Print($"attack for: {attackDamage} ");
        }


        public virtual bool IsPlayerInNoticeRadius()
        {
            return Position.DistanceTo(player.Position) < noticeRadius;
        }



    }
}
