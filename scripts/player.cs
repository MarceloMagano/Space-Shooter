using System;
using Godot;

public partial class Player : CharacterBody2D
{
  [Export]
  internal float Speed; // defined in the editor

  // createing a signal/event that will allow to shot the laser
  [Signal]
  public delegate void LaserShotEventHandler(PackedScene laserScene, Vector2 location);

  // loads the laser scene
  PackedScene _laserScene = GD.Load<PackedScene>("res://scene/laser.tscn");
  Marker2D _muzzle;

  private bool _shootCooldown;
  private const double RateOfFire = 0.25;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _muzzle = GetNode<Marker2D>("Muzzle");
  }

  public override async void _Process(double delta)
  {
    if (Input.IsActionPressed("shoot"))
    {
      // it keeps shooting the laser with some cooldown between each shot until the key/action is released
      if (!_shootCooldown)
      {
        _shootCooldown = true;
        Shoot();
        await ToSignal(GetTree().CreateTimer(RateOfFire), "timeout");
        _shootCooldown = false;
      }
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    // updates the direction of the player based on the input
    var direction = new Vector2(Input.GetAxis("move_left", "move_right"), Input.GetAxis("move_up", "move_down"));
    Velocity = Velocity with
    {
      X = direction.X * Speed,
      Y = direction.Y * Speed
    };
    MoveAndSlide();

    GlobalPosition = GlobalPosition.Clamp(Vector2.Zero, GetViewportRect().Size);
  }

  private void Shoot()
  {
    // emits the signal/event
    EmitSignal(SignalName.LaserShot, _laserScene, _muzzle.GlobalPosition);
  }

  /// <summary>
  /// Make the player disappear.
  /// </summary>
  internal void Die() => QueueFree();

}
