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
    GlobalPosition = GlobalPosition with { Y = GlobalPosition.Y + -Speed * (float)delta };
  }

  private void _on_visible_on_screen_enabler_2d_screen_exited()
  {
    QueueFree();
    // Replace with function body.
  }
}



