using System;

public class VersusCostBenefitsLv4Upgrade : Upgrade
{
    public override string Name => "VersusCostBenefitsLv4Upgrade";
    public override string Title => "수상한 하얀 가루";
    public override string Description => "1$ 더 비싸도 우리 치킨집에서 구매한다";
    public override decimal Price => 500m;
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(VersusCostBenefitsLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(versusCostBias: 1.1m);
    public override string ImagePath => "Sprites/upgrade_versus_image";
}