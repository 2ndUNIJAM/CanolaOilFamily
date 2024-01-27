using System;

public class VipCostBenefitsUpgrade : Upgrade
{
    public override string Name => "VipCostBenefitsUpgrade";
    public override string Title => "흑우 발견";
    public override string Description => "단골이 되면 2$ 더 비싸도 우리 치킨집에서 구매한다";
    public override decimal Price => 500m;
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(QuickVipUpgrade);
    public override UpgradeStat Stat => new(vipVersusCostBias: 2);
    public override string ImagePath => "Sprites/upgrade_vip_image";
    public override string IconPath => "Sprites/upgrade_vip_image";
}
