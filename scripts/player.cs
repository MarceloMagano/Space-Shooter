using Godot;
using System;

public partial class player : CharacterBody2D
{
  public const float Speed = 300.0f;
  public const float JumpVelocity = -400.0f;

  // Get the gravity from the project settings to be synced with RigidBody nodes.
  public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

  public override void _PhysicsProcess(double delta)
  {
    // // Add the gravity.
    // if (!IsOnFloor())
    //   velocity.Y += gravity * (float)delta;

    // // Handle Jump.
    // if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
    //   velocity.Y = JumpVelocity;

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    var direction = new Vector2(Input.GetAxis("move_left", "move_right"), Input.GetAxis("move_up", "move_down"));
    var velocity = new Vector2
    {
      X = direction.X * Speed,
      Y = direction.Y * Speed
    };
    // var velocity = new Vector2();
    // if (direction != Vector2.Zero)
    // {
    //   velocity.X = direction.X * Speed;
    //   GD.Print("if");
    // }
    // else
    // {
    //   velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
    //   GD.Print("else");
    // }



    Velocity = velocity;
    MoveAndSlide();
  }
}
