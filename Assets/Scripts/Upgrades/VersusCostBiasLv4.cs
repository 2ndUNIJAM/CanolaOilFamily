using System;

public class VersusCostBiasLv4 : Upgrade
{
    public override string Name => "VersusCostBiasLv4";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(VersusCostBiasLv3);
    public override bool IsReplaceConstraint => true;
    public override float VersusCostBias => 1f;
}