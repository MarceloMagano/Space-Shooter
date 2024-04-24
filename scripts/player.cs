using Godot;
using System;

public partial class player : CharacterBody2D
{
  [Export]
  public const float Speed = 300.0f;
  public const float JumpVelocity = -400.0f;

  // createing a signal/event that will allow to shot the laser

  [Signal]
  public delegate void LaserShotEventHandler(PackedScene laserScene, Vector2 location);

  // Get the gravity from the project settings to be synced with RigidBody nodes.
  public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


  PackedScene laserScene = GD.Load<PackedScene>("res://scene/laser.tscn");
  Marker2D muzzle;

  private bool shootCooldown = false;
  private const double rateOfFire = 0.25;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    base._Ready();
    muzzle = GetNode<Marker2D>("Muzzle");
  }

  public override async void _Process(double delta)
  {

    if (Input.IsActionPressed("shoot"))
    {
      if (!shootCooldown)
      {
        shootCooldown = true;
        Shoot();
        await ToSignal(GetTree().CreateTimer(rateOfFire), "timeout");
        shootCooldown = false;
      }
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    var direction = new Vector2(Input.GetAxis("move_left", "move_right"), Input.GetAxis("move_up", "move_down"));
    Velocity = Velocity with
    {
      X = direction.X * Speed,
      Y = direction.Y * Speed
    };
    MoveAndSlide();
  }

  public void Shoot()
  {
    // emits the signal/event
    EmitSignal(SignalName.LaserShot, laserScene, muzzle.GlobalPosition);
  }
}
