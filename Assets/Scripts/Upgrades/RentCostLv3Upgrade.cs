using System;

public class RentCostLv3Upgrade : Upgrade
{
    public override string Name => "RentCostLv3Upgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(RentCostLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(rentCostDecrement: 60);
}