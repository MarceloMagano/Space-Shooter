using Godot;

public partial class Game : Node
{
  Player _player;
  Marker2D _playerSpawnPos;
  Node2D _laserContainer;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _player = GetTree().GetFirstNodeInGroup("player") as Player;
    _playerSpawnPos = GetNode<Marker2D>("PlayerSpawnPos");
    _laserContainer = GetNode<Node2D>("LaserContainer");
    _player!.GlobalPosition = _playerSpawnPos.GlobalPosition;
    _player.LaserShot += OnPlayerLaserShot;
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

  private void OnPlayerLaserShot(PackedScene laserScene, Vector2 location)
  {
    var laser = laserScene.Instantiate() as Laser;
    laser!.GlobalPosition = location;
    _laserContainer.AddChild(laser);
  }
}
