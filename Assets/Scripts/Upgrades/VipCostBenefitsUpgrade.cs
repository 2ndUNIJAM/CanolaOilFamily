using System;

public class VipCostBenefitsUpgrade : Upgrade
{
    public override string Name => "VipCostBenefitsUpgrade";
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(QuickVipUpgrade);
    public override float VipVersusCostBias => 2f;
}
