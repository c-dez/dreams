using Godot;
using System;

public partial class Timer : Godot.Timer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Timeout += OnTimerTimeOutSignal;
    }

    private void OnTimerTimeOutSignal()
    {
        GD.Print("timer timeout");
    }

}
