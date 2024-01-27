using System;

public class RentCostLv2Upgrade : Upgrade
{
    public override string Name => "RentCostLv2Upgrade";
    public override string Title => "임대료 절감 Lv.2";
    public override string Description => "임대료가 50$ 감소한다";
    public override decimal Price => 200m;
    public override int LvConstraint => 2;
    public override Type UpgradeConstraint => typeof(RentCostLv1Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(rentCostDecrement: 40);
    public override string ImagePath => "Sprites/upgrade_rent_image";
}