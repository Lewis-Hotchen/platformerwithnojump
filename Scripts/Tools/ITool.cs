namespace PlatformerWithNoJump;

public interface ITool
{
	public Tools ToolType { get; }
	public bool IsPlaceable { get; }
}