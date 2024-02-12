using Godot;
using System;

public partial class ScreenCamera : Camera2D
{
    [Export]
    public float RandomStrength { get; set; } = 30.0f;

    [Export]
    public float ShakeFade { get; set; } = 5;

    private RandomNumberGenerator rng = new();

    private float shakeStrength = 0;

    [Export]
    public Vector2 Chunk { get => chunk; set { chunk = value; SetChunk(); } }

    private FastNoiseLite fastNoiseLite;

    [Export]
    public Vector2 MaxOffset { get; set; }

    [Export]
    public float MaxRoll { get; set; }

    private float trauma = 0.0f;
    private float traumaPower = 2f;
    private float noiseY = 0f;
    private Vector2 chunk;

    private void SetChunk()
    {
        var viewportX = GetViewportRect().Size.X / 2;
        var viewportY = GetViewportRect().Size.Y / 2;

        if (Chunk == Vector2.Zero)
        {
            Zoom = new(1, 1);
            Position = new(viewportX, viewportY);
            return;
        }

        Position = new Vector2(Chunk.X * viewportX, Chunk.Y * viewportY);
        Zoom = new(2, 2);
    }

    public override void _Process(double delta)
    {
        if (shakeStrength > 0)
        {
            shakeStrength = Mathf.Lerp(shakeStrength, 0, ShakeFade * (float)delta);
        }

        Offset = RandomOffset();
        base._Process(delta);
    }

    public void ApplyShake()
    {
        shakeStrength = RandomStrength;
    }

    public Vector2 RandomOffset()
    {
        return new Vector2(rng.RandfRange(-shakeStrength, shakeStrength), rng.RandfRange(-shakeStrength, shakeStrength));
    }
}