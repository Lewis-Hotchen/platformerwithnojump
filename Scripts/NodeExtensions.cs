using System.Runtime.CompilerServices;
using Godot;

namespace PlatformerWithNoJump;

public static class NodeExtensions {
    public static T GetSibling<T>(this Node2D n, string nodeName) where T : class{
        return n.GetNode<T>($"../{nodeName}");
    }
}