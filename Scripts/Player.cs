using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D
{
    public float fallingSpeed = 300f; 
    
    [Export]
    public int Speed { get; set; } = 400;
    public float Gravity { get; set; } = 9.8f;

    public void GetInput()
    {

        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!IsOnFloor()) {
            Velocity += Vector2.Down * Gravity * (float)delta;
        }

        GetInput();
        MoveAndSlide();
    }

}
