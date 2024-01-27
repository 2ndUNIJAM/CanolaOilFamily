public class FreeDeliveryUpgrade : Upgrade
{
    public override string Name => "FreeDeliveryUpgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(freeDeliveryDistance: 1);
    public override string ImagePath => "Sprites/upgrade_free_delivery_image";
    public override string IconPath => "Sprites/upgrade_delivery_icon";
}