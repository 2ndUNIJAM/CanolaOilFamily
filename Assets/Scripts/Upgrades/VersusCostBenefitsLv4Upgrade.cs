using System;

public class VersusCostBenefitsLv4Upgrade : Upgrade
{
    public override string Name => "VersusCostBenefitsLv4Upgrade";
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(VersusCostBenefitsLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(versusCostBias: 1);
    public override string ImagePath => "Sprites/upgrade_versus_cost_lv4_image";
    public override string IconPath => "Sprites/upgrade_versus_icon";
}