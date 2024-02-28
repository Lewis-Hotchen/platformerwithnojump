using System;
using System.Linq;
using Godot;

public partial class TimerTrackerComponent : Node2D
{
    [Export]
    public Timer[] Timers { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public bool GetTimerRunning(string name)
    {
        return !GetNode<Timer>(name).IsStopped();
    }

    public Timer GetTimer(string name)
    {
        return GetNode<Timer>(name);
    }

    public void StartTimer(string name)
    {
        GetNode<Timer>(name).Start();
    }

    public Timer AddTimer(float waitTime, string name, bool isOneShot = true)
    {
        var timer = new Timer()
        {
            WaitTime = waitTime,
            Name = name,
            OneShot = isOneShot
        };

        Timers = Timers.Concat(new Timer[] { timer }).ToArray();

        AddChild(timer);
        return timer;
    }

    public Timer AddTimer(float waitTime, string name, Action timeout, bool isOneShot = true)
    {
        var timer = new Timer()
        {
            WaitTime = waitTime,
            Name = name,
            OneShot = isOneShot
        };

        Timers = Timers.Concat(new Timer[] { timer }).ToArray();
        timer.Timeout += timeout;
        AddChild(timer);
        return timer;
    }

    internal void OneShot(float time, Action onTimeout)
    {
        // int max = 0;
        // var regex = new RegEx();
        // regex.Compile("/\\d+$/");
        // foreach(var name in Timers.Select(x => x.Name)) {
        //     if(name.ToString().Contains("oneshot")) {
        //         var res = regex.Search(name);
        //         if(res != null) {
        //             var num = Convert.ToInt32(res.GetString());
        //             if(max < num) max = num;
        //         }
        //     }
        // }

        // var newName = "oneshot"+max+1;

        var timer = AddTimer(time, "oneshot");
        timer.Timeout += onTimeout;
        timer.Timeout += OnTimeoutCleanup;
        timer.Start();
    }

    private void OnTimeoutCleanup()
    {
        GetTimer("oneshot").QueueFree();
        GetTimer("oneshot").Dispose();
    }

    internal void Subscribe(string timer, Action onTimeout)
    {
        GetTimer(timer).Timeout += onTimeout;
    }
}