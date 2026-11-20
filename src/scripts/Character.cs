
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

    private EnumState _state = EnumState.IDLE;

    private AnimationPlayer _animationPlayer;
    private Sprite2D _characterSprite;
    private Area2D _damageEmitter;

    // Guarda a direção para a qual o personagem está olhando.
    private Vector2 _facingDirection = Vector2.Right;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _characterSprite = GetNode<Sprite2D>("CharacterSprite");
        _damageEmitter = GetNode<Area2D>("DamageEmitter");
    }

    public override void _Process(double delta)
    {
        HandleInput();
        HandleMoviment();
        HandleAnimations();
        FlipSprites();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    // Verifica o estado de movimentação.
    public void HandleMoviment()
    {
        if (_canMove())
        {
            if (Velocity.Length() <= 0)
                _state = EnumState.IDLE;
            else
                _state = EnumState.WALK;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }

    // Captura os inputs do jogador.
    public void HandleInput()
    {
        var direction = Input.GetVector(
            "ui_left",
            "ui_right",
            "ui_up",
            "ui_down"
        ).Normalized();

        if (Input.IsActionJustPressed("attack") && _canAttack())
            _state = EnumState.ATTACK;

        if (_canMove())
            Velocity = direction * Speed;
    }

    // Controla as animações.
    public void HandleAnimations()
    {
        switch (_state)
        {
            case EnumState.IDLE:
                _animationPlayer.Play("idle");
                break;

            case EnumState.WALK:
                _animationPlayer.Play("walk");
                break;

            case EnumState.ATTACK:
                _animationPlayer.Play("punch");
                break;
        }
    }

    // Atualiza a direção do personagem.
    public void FlipSprites()
    {
        Vector2 scaleCollision = _damageEmitter.Scale;

        if (Velocity.X > 0)
        {
            _facingDirection = Vector2.Right;
            _characterSprite.FlipH = false;
            scaleCollision.X = 1;
        }
        else if (Velocity.X < 0)
        {
            _facingDirection = Vector2.Left;
            _characterSprite.FlipH = true;
            scaleCollision.X = -1;
        }

        _damageEmitter.Scale = scaleCollision;
    }

    // Verifica se pode movimentar.
    private bool _canMove() =>
        _state == EnumState.WALK ||
        _state == EnumState.IDLE;

    // Verifica se pode atacar.
    private bool _canAttack() =>
        _state == EnumState.IDLE ||
        _state == EnumState.WALK;

    // Retorna para IDLE após terminar uma ação.
    public void OnCompletedAction()
    {
        _state = EnumState.IDLE;
    }

    // Emite o dano e a direção do ataque.
    public void OnDamageEmitterAreaEntered(DamageReceiver damageEmitter)
    {
        damageEmitter.EmitSignal(
            DamageReceiver.SignalName.DamageRecivedEmmiter,
            Damage,
            _facingDirection
        );

        GD.Print($"Direção do ataque: {_facingDirection}");
    }
}
