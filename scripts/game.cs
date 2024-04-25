using System;
using Godot;

public partial class Game : Node
{
  Player _player;
  Marker2D _playerSpawnPos;
  Node2D _laserContainer;
  Node2D _enemyContainer;
  Hud _hud;

  [Export]
  PackedScene[] _enemyScenes; // an array of enemy scenes defined in the editor

  private int _score = 0;
  public int Score { get => _score; set => SetScore(value); }

  private void SetScore(int score)
  {
    _score = score;
    _hud.Score = _score;
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _player = GetNode<Player>("Player");
    _playerSpawnPos = GetNode<Marker2D>("PlayerSpawnPos");
    _laserContainer = GetNode<Node2D>("LaserContainer");
    _player!.GlobalPosition = _playerSpawnPos.GlobalPosition;
    _player.LaserShot += OnPlayerLaserShot;
    _enemyContainer = GetNode<Node2D>("EnemyContainer");
    _hud = GetNode("UILayer").GetChild<Hud>(0);
    Score = _score;
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

  private void _on_enemy_spawn_timer_timeout()
  {
    var enemyScene = _enemyScenes[GD.Randi() % _enemyScenes.Length];
    var enemy = enemyScene.Instantiate() as Enemy;
    enemy!.GlobalPosition = new Vector2(GD.RandRange(50, 500), -50);
    enemy.EnemyKilled += OnEnemyKilled;
    _enemyContainer.AddChild(enemy);
  }

  private void OnEnemyKilled(int score)
  {
    Score += score;
    GD.Print("Score: " + _score);
  }

}
