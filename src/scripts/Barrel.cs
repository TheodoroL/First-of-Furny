using FIrstofFurny.scripts;
using Godot;

public partial class Barrel : StaticBody2D
{
    [Export] 
    public float KnockbacIntenalty { get;set; } 
    
    private Vector2 _velocity =  Vector2.Zero;
    private EnumState _state = EnumState.IDLE;
    private float _height = 6;
    private float _speedHeight = 6;
    private Sprite2D _sprite;

    const float Gravity = 600f;
    
    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }
    
    public override void _Process(double delta)
    {
        Position += _velocity*(float)delta;
        _sprite.Position = Vector2.Up * _height;
        HandleDirTime((float)delta);
    }

    public void DamageReceiverDamageRecived(int damage, Vector2 direction)
    {
        if (_state == EnumState.IDLE)
        {
            _sprite.Frame = 1;
            _state = EnumState.DESTROY;
            _speedHeight = KnockbacIntenalty*2;
            _velocity = direction *KnockbacIntenalty;
            
            GD.Print(damage);
            GD.Print(direction);
            
        }
    }

    public void HandleDirTime(float delta)
    {
        if (_state == EnumState.DESTROY)
        {
            var modulate = Modulate;
            
            _height += _speedHeight * delta;
            
            modulate.A -= delta;
            Modulate = modulate;
            
            if (_height < 0)
            {
                _height = 0;
                QueueFree();
            }
            else _speedHeight-= Gravity * delta;
        }

    }
}
