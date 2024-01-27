using System;

public class RentCostLv3Upgrade : Upgrade
{
    public override string Name => "RentCostLv3Upgrade";
    public override string Title => "임대료 절감 Lv.3";
    public override string Description => "임대료가 70$ 감소한다";
    public override decimal Price => 300m;
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(RentCostLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(rentCostDecrement: 60);
    public override string ImagePath => "Sprites/upgrade_rent_image";
    public override string IconPath => "Sprites/upgrade_rent_image";
}