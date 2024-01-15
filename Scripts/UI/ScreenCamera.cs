using Godot;
using System;

public partial class ScreenCamera : Camera2D
{
    [Export]
    public float Decay { get; set; }

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


    public override void _Ready()
    {
        fastNoiseLite = new()
        {
            Seed = (int)GD.Randi(),
            FractalOctaves = 2
        };

        Decay = 0.3f;
        MaxOffset = new Vector2(24, 24);
        MaxRoll = 0.1f;

        SetChunk();

        base._Ready();
    }

    private void SetChunk()
    {
        var viewportX = GetViewportRect().Size.X / 2;
        var viewportY = GetViewportRect().Size.Y / 2;

        if(Chunk == Vector2.Zero){
            Zoom = new (1,1);
            Position = new(viewportX, viewportY);
            return;
        }

        Position = new Vector2(Chunk.X * viewportX, Chunk.Y * viewportY);
        Zoom = new(2,2);
    }


    public override void _Process(double delta)
    {
        // if(trauma > 0.0f) {
        //     trauma = (float) Mathf.Max(trauma - Decay * delta, 0);
        //     Shake();
        // }

        base._Process(delta);
    }

    public void AddTrauma(float amount) {
        trauma = Mathf.Min(trauma + amount, 1.0f);
    }

    public void Shake() {
        var amount = Mathf.Pow(trauma, traumaPower);
        noiseY += 1;
        Rotation = MaxRoll * amount * fastNoiseLite.GetNoise2D(fastNoiseLite.Seed, noiseY);
        Offset = new Vector2(MaxOffset.X * amount * fastNoiseLite.GetNoise2D(fastNoiseLite.Seed*2, noiseY),
        MaxOffset.Y * amount * fastNoiseLite.GetNoise2D(fastNoiseLite.Seed*2, noiseY));
    }
}