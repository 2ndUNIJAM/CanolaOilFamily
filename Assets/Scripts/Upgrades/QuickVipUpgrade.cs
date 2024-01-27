using System;

public class QuickVipUpgrade : Upgrade
{
    public override string Name => "QuickVipUpgrade";
    public override string Title => "리뷰 이벤트";
    public override string Description => "단골이 되는 속도가 빨라진다";
    public override decimal Price => 200m;
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(vipTurnDecrement: 5);
    public override string ImagePath => "Sprites/upgrade_vip_image";
    public override string IconPath => "Sprites/upgrade_vip_image";
}