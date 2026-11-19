using Godot;

public partial class Character : CharacterBody2D
{
  [Export]
  public int Health { get; set; } 
  [Export]
  public int Damage { get; set; }
  
  [Export]
  public float Speed { get; set; }


  public override void _Process(double delta)
  {
    var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down")
      .Normalized();

    Velocity = direction * Speed;
    
    MoveAndSlide();
  }
}
