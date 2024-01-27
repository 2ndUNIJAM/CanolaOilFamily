using System;

public class HalfDeliveryCostUpgrade : Upgrade
{
    public override string Name => "HalfDeliveryCostUpgrade";
    public override string Title => "배달비 할인";
    public override string Description => "거리 당 배달비가 $0.5 줄어든다.";
    public override decimal Price => 1000m;
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(FreeDeliveryUpgrade);
    public override UpgradeStat Stat => new(deliveryCostDecrement: 0.5m);
    public override string ImagePath => "Sprites/upgrade_delivery_image";
}
