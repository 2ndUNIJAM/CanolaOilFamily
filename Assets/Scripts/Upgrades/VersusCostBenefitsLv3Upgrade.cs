using System;

public class VersusCostBenefitsLv3Upgrade : Upgrade
{
    public override string Name => "VersusCostBenefitsLv3Upgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(VersusPriorityUpgrade);
    public override UpgradeStat Stat => new(versusCostBias: 0.5m);
    public override string ImagePath => "Sprites/upgrade_versus_cost_lv3_image";
    public override string IconPath => "Sprites/upgrade_versus_icon";
}
