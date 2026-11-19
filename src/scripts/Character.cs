using FIrstofFurny.scripts;
using Godot;

public partial class Character : CharacterBody2D
{
  [Export]
  public int Health { get; set; } 
  [Export]
  public int Damage { get; set; }
  
  [Export]
  public float Speed { get; set; }
  
  //Criando a máquina de estado 
  public EnumState  State { get; set; } =  EnumState.IDLE;
  
  private AnimationPlayer _animationPlayer;
  private Sprite2D _characterSprite;
  
  public override void _Ready()
  {
    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    _characterSprite = GetNode<Sprite2D>("CharacterSprite");
  }
  
  public override void _Process(double delta)
  {
    //separando as lógicas em funções
    HandleInput();
    HandleMoviment();
    HandleAnimations();
    FlipSprites();
    MoveAndSlide();
  }
  //Aqui verifica a movimentação do Character para mudar de Estado
  public void HandleMoviment()
  {
    if (Velocity.Length() <= 0)
      State = EnumState.IDLE;
    else
      State = EnumState.WALK;
  }
// método que verifica os inputs do jogador 
  public void HandleInput()
  {
    var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down")
      .Normalized();
    Velocity = direction * Speed;
    
  }
  //Verifica o Estado para aplicar a animação que estado que é 
  public void HandleAnimations()
  {
    switch (State)
    {
      case  EnumState.IDLE:
        _animationPlayer.Play("idle");
        break;
      
      case   EnumState.WALK:
        _animationPlayer.Play("walk");
        break;
    }     
  }

  //verifica a direção do Character para ele redirecionar o sprite
  public void FlipSprites()
  {
    if (Velocity.X > 0)
      _characterSprite.FlipH = false;
    
    else if(Velocity.X < 0)
      _characterSprite.FlipH = true;
  }
}
