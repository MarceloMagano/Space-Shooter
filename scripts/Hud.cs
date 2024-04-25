using Godot;
using System;

public partial class Hud : Control
{
  Label _score;

  public int Score { set => _score.Text = "Score: " + value; }
  // Called when the node enters the scene tree for the first time.

  public override void _Ready()
  {
    _score = GetNode<Label>("Score");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }
}
