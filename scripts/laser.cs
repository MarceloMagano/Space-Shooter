using Godot;

public partial class Laser : Area2D
{
  [Export]
  public const float Speed = 600;


  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    // Updating the position of the laser in the Y axis, based on the speed.
    // It travels from the bottom to top of the screen.
    GlobalPosition = GlobalPosition with { Y = GlobalPosition.Y + -Speed * (float)delta };
  }

  /// <summary>
  /// Makes the Laser disappear when it exits the screen.
  /// </summary>
  private void _on_visible_on_screen_enabler_2d_screen_exited()
  {
    QueueFree();
  }

  /// <summary>
  /// When the laser collides with an enemy, it will die and the Laser disappears
  /// </summary>
  private void _on_area_entered(Area2D area)
  {
    if (area is Enemy enemy)
    {
      enemy.Die();
      QueueFree();
    }
  }
}



