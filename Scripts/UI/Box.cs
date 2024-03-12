using Godot;
using System;
namespace PlatformerWithNoJump;

public partial class Box : Node2D
{
    public override void _Draw()
    {
        var viewPortRect = GetNode<CanvasLayer>("../../Game").GetViewport().GetVisibleRect().Size;
        var containerRect = new Rect2I(new Vector2I(Convert.ToInt32(viewPortRect.X - Constants.CellSize * 4), 0), new Vector2I(Constants.CellSize * 4, Convert.ToInt32(viewPortRect.Y)));
        DrawRect(containerRect, Colors.White, false, 1);
        this.GetSibling<ColorRect>("ColorRect").Position = new Vector2I(Convert.ToInt32(viewPortRect.X - Constants.CellSize * 4), 0);
        this.GetSibling<ColorRect>("ColorRect").Size = new Vector2I(Constants.CellSize * 4, Convert.ToInt32(viewPortRect.Y));
        base._Draw();
    }
}
