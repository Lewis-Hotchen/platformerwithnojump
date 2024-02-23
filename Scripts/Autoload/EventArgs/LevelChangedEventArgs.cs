using System;

namespace PlatformerWithNoJump;

public class LevelChangedEventArgs : EventArgs
{
    public LevelChangedEventArgs(string prevLevel, string newLevel)
    {
        PrevLevel = prevLevel;
        NewLevel = newLevel;
    }

    public string PrevLevel { get; }
    public string NewLevel { get; }
}