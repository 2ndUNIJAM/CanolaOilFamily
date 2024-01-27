using System;

public class RentCostLv3Upgrade : Upgrade
{
    public override string Name => "RentCostLv3Upgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(RentCostLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(rentCostDecrement: 60);
    public override string ImagePath => "Sprites/upgrade_rent_cost_lv3_image";
    public override string IconPath => "Sprites/upgrade_rent_icon";
}