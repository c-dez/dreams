using Godot;

namespace Actors
{
    public partial class Enemies : CharacterBody3D
    {
        //player quiero que se detecte por medio de grupo "Player" pero esta fallando, corregir en el futuro, por ahora tengo que poner el objeto player en inspector manualmente
        [Export] private CharacterBody3D player;

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
            //no detecta grupo "Player"
            // player = (CharacterBody3D)GetTree().GetFirstNodeInGroup("Player");
        }


        public override void _PhysicsProcess(double delta)
        {
            MoveToPlayer((float)delta);
        }

        public virtual void MoveToPlayer(float delta)
        {
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
                    Velocity = new Vector3(TargetVector.X, 0, TargetVector.Y) * speed;
                }
                else
                {
                    Velocity = Vector3.Zero;
                }
                // Velocity = velocity;
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
