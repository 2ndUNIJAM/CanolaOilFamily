using System;

public class VersusCostBiasLv3 : Upgrade
{
    public override string Name => "VersusCostBiasLv3";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(VersusPriorityUpgrade);
    public override float VersusCostBias => 0.5f;
}
