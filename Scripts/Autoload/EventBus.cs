using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Godot;

namespace PlatformerWithNoJump;

public partial class EventBus : Node
{
    private static readonly BindingFlags staticFlags = BindingFlags.Instance | BindingFlags.NonPublic;

    public event EventHandler<StateChangedEventArgs> StateChanged;

    public event EventHandler<ToolBuiltEventArgs> ToolBuilt;

    public event EventHandler<ToolFailedEventArgs> ToolFailed;

    public event EventHandler<ToolsBuiltChangedEventArgs> ToolsBuiltChanged;

    public void RaiseEvent<T>(string @event, object sender, T eventArgs) where T : EventArgs
    {
        var sb = new StringBuilder();

        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(eventArgs))
        {
            string name = descriptor.Name;
            object value = descriptor.GetValue(eventArgs);
            sb.Append($"\t{name} : {value} \n");
        }

        GD.Print(
           $"[Event Rasied!]\n{@sender.GetType().Name} sent event {@event} \nArgs: [\n{sb}]\n-------------------------------------"
           );

        var type = GetType();
        var eventField = type.GetField(@event, staticFlags) ?? throw new Exception($"Event with name {@event} could not be found.");
        if (eventField.GetValue(this) is not MulticastDelegate multicastDelegate)
            return;

        var invocationList = multicastDelegate.GetInvocationList();

        foreach (var invocationMethod in invocationList)
            invocationMethod.DynamicInvoke(new object[] { sender, eventArgs });
    }
}