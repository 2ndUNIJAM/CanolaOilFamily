using System;

public class RentCostLv2Upgrade : Upgrade
{
    public override string Name => "RentCostLv2Upgrade";
    public override int LvConstraint => 2;
    public override Type UpgradeConstraint => typeof(RentCostLv1Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(rentCostDecrement: 40);
}