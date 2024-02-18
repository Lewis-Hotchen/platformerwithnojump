using System;
using PlatformerWithNoJump;

namespace PlatformerWithNoJump;
public class ToolFailedEventArgs : EventArgs
{
    public ToolFailedEventArgs(Tools tool, FailedToolReason failedToolReason)
    {
        Tool = tool;
        FailedToolReason = failedToolReason;
    }

    public Tools Tool { get; }
    public FailedToolReason FailedToolReason {get;}
}

public enum FailedToolReason {
    RESOURCE_EMPTY = 0,
    NO_REVERT = 1
}