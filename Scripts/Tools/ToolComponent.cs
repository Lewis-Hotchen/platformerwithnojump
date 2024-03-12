using Godot;
using PlatformerWithNoJump;
using System;

public partial class ToolComponent : Node2D
{
    [Export]
    public bool IsActive { get; set; }

    [Export]
    public bool CanFall { get; set; }

    [Export]
    public Tools ToolType { get; set; }

    public void SetDirection(float rotation)
    {
        if(GetParent<Node2D>().HasMethod("SetDirection")) {
            GetParent<Node2D>().Call("SetDirection", rotation);
        }
    }
}
