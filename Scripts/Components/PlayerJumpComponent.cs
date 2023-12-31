using Godot;

public partial class PlayerJumpComponent : Node2D
{
	[Export]
	public Player Player { get; set; }

	[Export]
	public Vector2 Direction { get; set; }

	[Export]
	public float ForceY { get; set; }

	[Export]
	public float ForceX { get; set; }

	public void Jump() {
		Player.ApplyForce(Direction * new Vector2(ForceX, ForceY));
	}
}
