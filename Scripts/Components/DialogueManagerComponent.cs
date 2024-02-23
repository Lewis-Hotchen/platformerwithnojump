using System;
using System.Text;
using Godot;
namespace PlatformerWithNoJump;

public partial class DialogueManagerComponent : Control
{
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }

    [Export]
    public Timer TextTimeout;

    [Export]
    public string NextTextAction { get; set; }

    [Export]
    public string DialogueEntry { get; set; }

    public DialogueManager DialogueManager { get; set; }

    [Export]
    public string DialoguePath { get; set; }

    [Export]
    public bool CanSkip { get; set; }

    [Export]
    public bool CanTimeOut { get; set; }

    [Export]
    public AnimationPlayer ContinueAnim { get; set; }

    public event EventHandler<DialogueCompleteArgs> DialogueComplete;

    public override void _Ready()
    {
        TextTimeout.Timeout += OnTimeout;
        using var file = FileAccess.Open(PWNJConstants.DialogueFilePath(DialoguePath), FileAccess.ModeFlags.Read);
        
        if(DialogueEntry != null) {
            DialogueManager = new(file.GetPathAbsolute(), DialogueEntry);
        }

        base._Ready();
    }

    private void OnTimeout()
    {
        Visible = false;
    }

    public override void _Process(double delta)
    {
        GetNode<RichTextLabel>("DialogueBox/SkipText").Visible = CanSkip;
        if (GetNode<RichTextLabel>("DialogueBox/SkipText").Visible && !ContinueAnim.IsPlaying())
        {
            ContinueAnim.Play("Animations/SkipIndicator");
        }

        if (CanSkip && !string.IsNullOrEmpty(NextTextAction))
        {
            if (Input.IsActionJustPressed(NextTextAction))
            {
                TextTimeout.Stop();
                AnimationPlayer.GetAnimation("dialogue_painter/text_paint").Clear();
                CycleDialogue();
            }
        }

        base._Process(delta);
    }

    private void CycleDialogue()
    {
        if (SetNextDialogueStep())
        {
            SetDialogueOnBox();
        }
        else
        {
            Visible = false;
        }
    }

    public double SetDialogueOnBox()
    {
        if(DialogueManager == null) {
            OneShotDialog("It would appear my dialogue has not loaded correctly....This is a bug.");
        } 

        AnimationPlayer.GetAnimation("dialogue_painter/text_paint").Clear();
        Visible = true;
        GetNode<RichTextLabel>("DialogueBox/Text").Text = DialogueManager.GetStep();
        GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Playing = false;

        var textPaintAnimation = AnimationPlayer.GetAnimation("dialogue_painter/text_paint");

        textPaintAnimation.AddTrack(Animation.TrackType.Value, 0);
        textPaintAnimation.ValueTrackSetUpdateMode(0, Animation.UpdateMode.Discrete);
        textPaintAnimation.TrackSetPath(0, "DialogueBox/Text:text");

        textPaintAnimation.AddTrack(Animation.TrackType.Value, 1);
        textPaintAnimation.ValueTrackSetUpdateMode(1, Animation.UpdateMode.Discrete);
        textPaintAnimation.TrackSetPath(1, "AudioStreamPlayer2D:playing");

        var index = 0;
        StringBuilder sb = new();
        textPaintAnimation.Length = DialogueManager.GetStep().Length * 0.05f;

        foreach (var character in DialogueManager.GetStep())
        {
            sb.Append(character);
            textPaintAnimation.TrackInsertKey(0, 0.05f * index, sb.ToString());

            if (DialogueManager.GetStep().IndexOf(character) % 2 == 0)
            {
                textPaintAnimation.TrackInsertKey(1, 0.05f * index, true);
            }

            index++;
        }

        AnimationPlayer.Play("dialogue_painter/text_paint");

        if (CanTimeOut)
        {
            TextTimeout.WaitTime = textPaintAnimation.Length * 2;
            TextTimeout.Start();
        }

        DialogueComplete?.Invoke(this, new DialogueCompleteArgs(DialogueManager.GetStep()));
        return TextTimeout.WaitTime;
    }

    public double OneShotDialog(string dialogue)
    {
        AnimationPlayer.GetAnimation("dialogue_painter/text_paint").Clear();
        Visible = true;
        GetNode<RichTextLabel>("DialogueBox/Text").Text = dialogue;
        GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Playing = false;

        var textPaintAnimation = AnimationPlayer.GetAnimation("dialogue_painter/text_paint");

        textPaintAnimation.AddTrack(Animation.TrackType.Value, 0);
        textPaintAnimation.ValueTrackSetUpdateMode(0, Animation.UpdateMode.Discrete);
        textPaintAnimation.TrackSetPath(0, "DialogueBox/Text:text");

        textPaintAnimation.AddTrack(Animation.TrackType.Value, 1);
        textPaintAnimation.ValueTrackSetUpdateMode(1, Animation.UpdateMode.Discrete);
        textPaintAnimation.TrackSetPath(1, "AudioStreamPlayer2D:playing");

        var index = 0;
        StringBuilder sb = new();
        textPaintAnimation.Length = dialogue.Length * 0.05f;

        foreach (var character in dialogue)
        {
            sb.Append(character);
            textPaintAnimation.TrackInsertKey(0, 0.05f * index, sb.ToString());

            if (dialogue.IndexOf(character) % 2 == 0)
            {
                textPaintAnimation.TrackInsertKey(1, 0.05f * index, true);
            }

            index++;
        }

        AnimationPlayer.Play("dialogue_painter/text_paint");

        if (CanTimeOut)
        {
            TextTimeout.WaitTime = textPaintAnimation.Length + 1;
            TextTimeout.Start();
        }

        DialogueComplete?.Invoke(this, new DialogueCompleteArgs(dialogue));
        return TextTimeout.WaitTime;
    }

    public bool SetNextDialogueStep()
    {
        return DialogueManager.NextStep();
    }
}
