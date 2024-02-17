using Godot;
using Godot.Collections;
namespace PlatformerWithNoJump;
public class ToolsBuiltChangedEventArgs
{

    public ToolsBuiltChangedEventArgs(Array<Node2D> tools)
    {
        Tools = tools;
    }

    public Array<Node2D> Tools { get; }
}