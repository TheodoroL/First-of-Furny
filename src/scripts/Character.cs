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
  private EnumState  _state =   EnumState.IDLE;
  
  private AnimationPlayer _animationPlayer;
  private Sprite2D _characterSprite;
  private Area2D _damageEmitter;
  
  public override void _Ready()
  {
    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    _characterSprite = GetNode<Sprite2D>("CharacterSprite");
    _damageEmitter = GetNode<Area2D>("DamageEmitter");
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
    if(_canMove())
      if (Velocity.Length() <= 0)
        _state = EnumState.IDLE;
      else
        _state = EnumState.WALK;
    else 
      Velocity = Vector2.Zero;
  }
// método que verifica os inputs do jogador 
  public void HandleInput()
  {
    var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down")
      .Normalized();
    
    if(Input.IsActionJustPressed("attack") &&  _canAttack())
      _state = EnumState.ATTACK;
      
    Velocity = direction * Speed;
    
  }
  //Verifica o Estado para aplicar a animação que estado que é 
  public void HandleAnimations()
  {
    switch (_state)
    {
      case  EnumState.IDLE:
        _animationPlayer.Play("idle");
        break;
      
      case   EnumState.WALK:
        _animationPlayer.Play("walk");
        break;
      
      case EnumState.ATTACK:
        _animationPlayer.Play("punch");
        break;
    }     
  }

  //verifica a direção do Character para ele redirecionar o sprite
  public void FlipSprites()
  {
    Vector2 scaleCollision = _damageEmitter.Scale;
    if (Velocity.X > 0)
    {
      _characterSprite.FlipH = false;
      scaleCollision.X = 1;
      
    }
    else if (Velocity.X < 0)
    {
      _characterSprite.FlipH = true;
      scaleCollision.X = -1;
    }
    _damageEmitter.Scale = scaleCollision;
  }

  private bool _canMove() => _state == EnumState.WALK || _state == EnumState.IDLE;
  
  private bool _canAttack() => _state == EnumState.IDLE ||  _state == EnumState.WALK;


  //função que faz que o jogador após terminar o soco ele volte no modo IDLE
  public void OnCompletedAction() => _state = EnumState.IDLE;

  public void OnDamageEmitterAreaEntered(DamageReceiver damageEmitter)
  {
    damageEmitter.EmitSignal(DamageReceiver.SignalName.DamageRecivedEmmiter, Damage);
    GD.Print(damageEmitter);
  }
}
