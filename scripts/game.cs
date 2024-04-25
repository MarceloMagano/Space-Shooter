using System;
using System.IO;
using System.Threading.Tasks;
using Godot;

public partial class Game : Node
{
  // using the value to deacrease the spawn timer of the enemies over time to make the game more dificult
  private const double EnemySpawnTimerDecrease = 0.005;
  // minimum time in seconds between enemy spawns
  private const double MinSpawnTimer = 0.25;
  private const float ScrollSpped = 100f;

  Player _player;
  Marker2D _playerSpawnPos;
  Node2D _laserContainer;
  Node2D _enemyContainer;
  Hud _hud;
  GameOverScreen _gameOverScreen;
  Timer _spawnTimer;
  ParallaxBackground _parallaxBackground;
  [Export]
  PackedScene[] _enemyScenes; // an array of enemy scenes defined in the editor

  AudioStreamPlayer _laserSound;
  AudioStreamPlayer _hitSound;
  AudioStreamPlayer _explodeSound;

  private int _score = 0;
  public int Score
  {
    get => _score;
    set
    {
      _score = value;
      _hud.Score = _score;
    }
  }

  private uint _highScore = 0;
  public uint HighScore
  {
    get => _highScore;
    set { _highScore = value; }
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _playerSpawnPos = GetNode<Marker2D>("PlayerSpawnPos");
    _laserContainer = GetNode<Node2D>("LaserContainer");

    _player = GetNode<Player>("Player");
    _player!.GlobalPosition = _playerSpawnPos.GlobalPosition;
    _player.LaserShot += OnPlayerLaserShot;
    _player.PlayerDied += OnPlayerDiedAsync;

    _enemyContainer = GetNode<Node2D>("EnemyContainer");
    _hud = GetNode("UILayer").GetNode<Hud>("HUD");
    Score = _score;

    _gameOverScreen = GetNode("UILayer").GetNode<GameOverScreen>("GameOverScreen");

    LoadGame();
    _spawnTimer = GetNode<Timer>("EnemySpawnTimer");
    _parallaxBackground = GetNode<ParallaxBackground>("ParallaxBackground");

    _laserSound = GetNode("SFX").GetNode<AudioStreamPlayer>("LaserSound");
    _hitSound = GetNode("SFX").GetNode<AudioStreamPlayer>("HitSound");
    _explodeSound = GetNode("SFX").GetNode<AudioStreamPlayer>("ExplodeSound");

  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (Input.IsActionPressed("quit"))
      GetTree().Quit();
    else if (Input.IsActionPressed("reset"))
      GetTree().ReloadCurrentScene();

    DecreaseSpawnTimer(delta);

    _parallaxBackground.ScrollOffset = _parallaxBackground.ScrollOffset with { Y = _parallaxBackground.ScrollOffset.Y + (float)delta * ScrollSpped };
    if (_parallaxBackground.ScrollOffset.Y > 960)
    {
      _parallaxBackground.ScrollOffset = new Vector2(0, 0);
    }
  }

  private void LoadGame()
  {
    var saveFile = Godot.FileAccess.Open("user://save.data", Godot.FileAccess.ModeFlags.Read);
    if (saveFile != null)
    {
      HighScore = saveFile.Get32();
      saveFile.Close();
    }
    else
    {
      // if file not found this is 1st time runing
      SaveGame();
    }
  }

  private void SaveGame()
  {
    var saveFile = Godot.FileAccess.Open("user://save.data", Godot.FileAccess.ModeFlags.Write);
    saveFile.Store32(HighScore);
    // need to close the file otherwise it will not be saved
    // if retry multiple times on 1st run the highscore will allwas same as the score
    saveFile.Close();
  }


  private void OnPlayerLaserShot(PackedScene laserScene, Vector2 location)
  {
    var laser = laserScene.Instantiate() as Laser;
    laser!.GlobalPosition = location;
    _laserContainer.AddChild(laser);
    _laserSound.Play();
  }

  private void _on_enemy_spawn_timer_timeout()
  {
    var enemyScene = _enemyScenes[GD.Randi() % _enemyScenes.Length];
    var enemy = enemyScene.Instantiate() as Enemy;
    enemy!.GlobalPosition = new Vector2(GD.RandRange(50, 500), -50);
    enemy.EnemyKilled += OnEnemyKilled;
    enemy.EnemyHit += () => _hitSound.Play();
    _enemyContainer.AddChild(enemy);
  }

  private void OnEnemyKilled(int score)
  {
    _hitSound.Play();
    Score += score;

    if (Score > HighScore)
      HighScore = (uint)Score;
  }

  private async void OnPlayerDiedAsync()
  {
    _explodeSound.Play();
    _gameOverScreen.SetScore(Score.ToString());
    _gameOverScreen.SetHighScore(HighScore.ToString());
    SaveGame();
    await ToSignal(GetTree().CreateTimer(0.5), "timeout");
    _gameOverScreen.Visible = true;
  }

  /// <summary>
  /// Decreases the spawn timer over time to make the game more difficult
  /// </summary>
  /// <param name="delta">the frame/delta.</param>
  private void DecreaseSpawnTimer(double delta)
  {
    // decreases the spawn timer over time to make the game more difficult
    if (_spawnTimer.WaitTime > MinSpawnTimer)
      _spawnTimer.WaitTime -= delta * EnemySpawnTimerDecrease;
    else
      _spawnTimer.WaitTime = MinSpawnTimer;
  }
}
