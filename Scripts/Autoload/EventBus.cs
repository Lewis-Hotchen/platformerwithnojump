using System.Linq;
using System.Runtime.CompilerServices;
using Godot;

public partial class EventBus : Node
{
    [Signal]
    public delegate void StateChangedEventHandler(Node sender, string state, Variant value);

    [Signal]
    public delegate void ToolBuiltEventHandler(Node sender, Node2D tool, Vector2 globalPosition);

    [Signal]
    public delegate void ToolFailedEventHandler();

    public void RaiseEvent(string @event, params Variant[] args) {
        GD.Print(
            $"[Event Rasied!]\n{@event} sent signal \nArgs: [{string.Join(", ", args.Select(x => x.ToString()))}]"
            );
        EmitSignal(@event, args.ToArray());
    }
}
