using Godot;

namespace PlatformerWithNoJump;

internal interface ILevel
{
    ScreenCamera Camera { get; }
    TileMap TileMap { get; }
    DeployedToolsComponent DeployedTools { get; }
   // StateTrackerComponent StateTrackerComponent { get; }
   //BuildModeComponent BuildModeComponent { get; }
}