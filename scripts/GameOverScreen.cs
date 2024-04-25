using Godot;
using System;

public partial class GameOverScreen : Control
{
  Label _score;
  Label _highScore;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    _score = GetNode("Panel").GetNode<Label>("Score");
    _highScore = GetNode("Panel").GetNode<Label>("HighScore");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }

  public void OnButtonPressed()
  {
    GetTree().ReloadCurrentScene();
  }

  public void SetScore(string score)
  {
    _score.Text = "Score: " + score;
  }

  public void SetHighScore(string score)
  {
    _highScore.Text = "High Score: " + score;
  }
}
