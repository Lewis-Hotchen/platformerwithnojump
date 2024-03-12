using Godot;
using PlatformerWithNoJump;
using System;

public partial class ResetComponent : Node2D
{
    [Export]
    public ResetLevelComponent ResetLevelComponent { get; set; }

    [Export]
    public PlayerKillBoxComponent PlayerKillBoxComponent { get; set; }

    [Export]
    public TimerTrackerComponent TimerTrackerComponent { get; set; }

    [Export]
    public DialogueManagerComponent DialogueManagerComponent { get; set; }
    
    [Export]
    public bool SaySomething { get; set; }

    [Export]
    public string[] SnarkyComments { get; set; }

    private int snarkyCommentsCounter = 0;
    private EventBus eventBus;


    public override void _Ready()
    {
        eventBus = GetNode<EventBus>("/root/EventBus");

        if(PlayerKillBoxComponent != null) {
            PlayerKillBoxComponent.OnPlayerFell += OnPlayerFell;
        }

        eventBus.RevertAll += OnRevert;
        base._Ready();
    }

    private void OnRevert(object sender, EventArgs e)
    {
        ResetLevelComponent.ResetLevel();
    }

    private void OnPlayerFell(object sender, EventArgs e)
    {
        var resetTimer = TimerTrackerComponent.AddTimer(2, "reset", () =>
        {
            ResetLevelComponent.ResetLevel();
        });

        GetNode<ScreenCamera>("../../../Camera").ApplyShake();

        TimerTrackerComponent.OneShot(3, () =>
        {
            if(SaySomething) {
                DialogueManagerComponent.OneShotDialog(SnarkyComments[snarkyCommentsCounter]);
                if(snarkyCommentsCounter == SnarkyComments.Length - 1) {
                    snarkyCommentsCounter = 0;
                } else {
                    snarkyCommentsCounter++;
                }
            }
            
            resetTimer.Start();
        });
    }
}
