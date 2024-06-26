using System;
using Godot;

public partial class Enemy : Area2D
{
  [Export]
  internal float Speed; // defined in the editor
  [Export]
  internal float HP; // defined in the editor
  [Export]
  internal int Score = 100;

  [Signal]
  public delegate void EnemyKilledEventHandler(int score);
  [Signal]
  public delegate void EnemyHitEventHandler();

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _PhysicsProcess(double delta)
  {
    // Updating the position of the enemy in the Y axis, based on the speed.
    // It travels from the top to the bottom of the screen.
    GlobalPosition = GlobalPosition with { Y = GlobalPosition.Y + (Speed * (float)delta) };
  }

  /// <summary>
  /// Make the enemy disappear.
  /// </summary>
  internal void Die() => QueueFree();

  /// <summary>
  /// When the enemy collides with the player, it will die and the player also dies
  /// </summary>
  private void _on_body_entered(Node2D body)
  {
    if (body is Player player)
    {
      player.Die();
      Die();
    }
  }

  /// <summary>
  /// Makes the enemy disappear when it exits the screen.
  /// </summary>
  private void _on_visible_on_screen_notifier_2d_screen_exited() => QueueFree();

  internal void TakeDamage(int amount)
  {
    HP -= amount;
    if (HP <= 0)
    {
      EmitSignal(SignalName.EnemyKilled, Score);
      Die();
    }
    else
    {
      EmitSignal(SignalName.EnemyHit);
    }
  }
}
