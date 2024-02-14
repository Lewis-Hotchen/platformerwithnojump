using Godot;
using System;

public partial class ToolComponent : Node2D
{
    [Export]
    public bool IsActive { get; set; }

    [Export]
    public bool CanFall { get; set; }

    public void SetDirection(float rotation) { 
        GetParent<Node2D>().Call("SetDirection", rotation);
    }
}
