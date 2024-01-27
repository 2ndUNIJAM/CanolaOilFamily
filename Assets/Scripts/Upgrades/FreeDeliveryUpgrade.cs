public class FreeDeliveryUpgrade : Upgrade
{
    public override string Name => "FreeDeliveryUpgrade";
    public override string Title => "무료 배달";
    public override string Description => "거리 1 이내 배달비 무료";
    public override decimal Price => 50m;
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(freeDeliveryDistance: 1);
    public override string ImagePath => "Sprites/upgrade_delivery_image";
    public override string IconPath => "Sprites/upgrade_delivery_image";
}