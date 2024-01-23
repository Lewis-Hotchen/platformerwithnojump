using Godot;

namespace PlatformerWithNoJump;

public partial class ToolComponent : Node2D {
    [Export]
    public Tools ToolType { get; set; }

    [Export]
    public bool IsBuildable { get; set; }
}