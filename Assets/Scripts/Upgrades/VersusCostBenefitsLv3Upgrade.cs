using System;

public class VersusCostBenefitsLv3Upgrade : Upgrade
{
    public override string Name => "VersusCostBenefitsLv3Upgrade";
    public override string Title => "양념 소스 개발";
    public override string Description => "0.5$ 더 비싸도 우리 치킨집에서 구매한다";
    public override decimal Price => 200m;
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(VersusPriorityUpgrade);
    public override UpgradeStat Stat => new(versusCostBias: 0.5m);
    public override string ImagePath => "Sprites/upgrade_versus_image";
    public override string IconPath => "Sprites/upgrade_versus_image";
}
