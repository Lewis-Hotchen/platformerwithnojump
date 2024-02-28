using Godot;
using System;

public partial class ChumDeath : Node2D
{
    [Export]
    public CpuParticles2D ChumLeftLeg { get; set; }

    [Export]
    public CpuParticles2D ChumRightLeg { get; set; }

    [Export]
    public CpuParticles2D ChumBody { get; set; }

    [Export]
    public TimerTrackerComponent TimerTrackerComponent { get; set; }

    private Timer timer;
    public override void _Ready()
    {
        timer = TimerTrackerComponent.AddTimer(1f, "LegTimer");
        timer.Timeout += OnLegTimeout;
        base._Ready();
    }

    public void Play(bool delay = false)
    {
        if (delay)
        {
            timer.Start();
        }
        else
        {
            ChumLeftLeg.Emitting = true;
            ChumRightLeg.Emitting = true;
            ChumBody.Emitting = true;
        }
    }

    private void OnLegTimeout()
    {
        ChumLeftLeg.Emitting = true;
        ChumRightLeg.Emitting = true;
        ChumBody.Emitting = true;
    }

}
