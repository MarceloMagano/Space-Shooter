using Godot;
using System;

public partial class game : Node
{
  player player;
  Marker2D playerSpawnPos;
  Node2D laserContainer;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    player = GetTree().GetFirstNodeInGroup("player") as player;
    playerSpawnPos = GetNode<Marker2D>("PlayerSpawnPos");
    laserContainer = GetNode<Node2D>("LaserContainer");
    player.GlobalPosition = playerSpawnPos.GlobalPosition;
    player.LaserShot += OnPlayerLaserShot;
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

  private void OnPlayerLaserShot(PackedScene laser_scene, Vector2 location)
  {
    var laser = laser_scene.Instantiate() as laser;
    laser.GlobalPosition = location;
    laserContainer.AddChild(laser);
  }
}
