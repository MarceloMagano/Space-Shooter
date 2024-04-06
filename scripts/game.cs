using Godot;
using System;

public partial class game : Node
{
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
	GD.Print(Input.IsActionJustPressed("quit"));

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
