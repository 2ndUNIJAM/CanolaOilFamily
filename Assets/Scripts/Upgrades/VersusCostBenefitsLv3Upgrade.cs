using System;

public class VersusCostBenefitsLv3Upgrade : Upgrade
{
    public override string Name => "VersusCostBenefitsLv3Upgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(VersusPriorityUpgrade);
    public override float VersusCostBias => 0.5f;
}
