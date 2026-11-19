using Godot;

public partial class Barrel : StaticBody2D
{
    public void DamageReceiverDamageRecived(int damage)
    {
        GD.Print(damage);
        QueueFree();
    }
}
