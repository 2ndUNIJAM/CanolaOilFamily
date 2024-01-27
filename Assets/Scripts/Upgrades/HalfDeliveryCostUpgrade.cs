using System;

public class HalfDeliveryCostUpgrade : Upgrade
{
    public override string Name => "HalfDeliveryCostUpgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(FreeDeliveryUpgrade);
    public override UpgradeStat Stat => new(deliveryCostFactor: 0.5m);
    public override string ImagePath => "Sprites/upgrade_delivery_image";
    public override string IconPath => "Sprites/upgrade_delivery_image";
}
