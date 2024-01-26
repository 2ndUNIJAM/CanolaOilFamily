public class FreeDeliveryUpgrade : Upgrade
{
    public override string Name => "FreeDeliveryUpgrade";
    public override int LvConstraint => 1;
    public override int FreeDeliveryDistance => 1;
}