using Godot;
using System;

public partial class InGameLabel : HBoxContainer
{
    private string text;

    [Export(PropertyHint.MultilineText)]
    public string Text { get => text; set { text = value; UpdateText(); } }

    public void UpdateText()
    {
        GetNode<Label>("Label").Text = text;
    }
}
