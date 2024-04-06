using Godot;
using System;

public partial class game : Node
{
  Node2D player;
  Marker2D playerSpawnPos;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    player = GetTree().GetFirstNodeInGroup("player") as Node2D;
    playerSpawnPos = GetNode<Marker2D>("PlayerSpawnPos");
    player.GlobalPosition = playerSpawnPos.GlobalPosition;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Input.IsActionPressed("quit"))
    {
      GetTree().Quit();
    }
    else if (Input.IsActionPressed("reset"))
    {
      GetTree().ReloadCurrentScene();
    }
  }
}
