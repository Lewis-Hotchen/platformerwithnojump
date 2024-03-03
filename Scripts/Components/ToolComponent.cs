using Godot;

namespace PlatformerWithNoJump;

public partial class ToolComponent : Node2D
{
    [Export]
    public Tools ToolType { get; set; }

    [Export]
    public bool IsBuildable { get; set; }
    
    [Export]
    public bool IsActive { get; set; }

    [Export]
    public bool CanFall { get; set; }

    public void SetDirection(float rotation) { 
        GetParent<Node2D>().Call("SetDirection", rotation);
    }
}