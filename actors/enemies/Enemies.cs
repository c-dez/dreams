using Godot;
using System;

namespace Enemies
{
    public partial class Enemies : Node
    {
        private int Health = 100;

        public override void _Ready()
        {
        }

        public void OnDead()
        {
			if (Health <= 0)
			{
				GD.Print($"{this} is dead");
			}

        }


    }
}
