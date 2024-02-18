using System;
using Godot;
namespace PlatformerWithNoJump;
public class StateChangedEventArgs : EventArgs
{
    public StateChangedEventArgs(string state, Variant value)
    {
        State = state;
        Value = value;
    }

    public string State { get; }
    public Variant Value { get; }
}