using System;
namespace PlatformerWithNoJump;

public class DialogueCompleteArgs : EventArgs
{
    public string CompletedStep { get; set; }

    public DialogueCompleteArgs(string step)
    {
        CompletedStep = step;
    }
}