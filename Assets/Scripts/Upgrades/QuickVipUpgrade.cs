using System;

public class QuickVipUpgrade : Upgrade
{
    public override string Name => "QuickVipUpgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(vipTurnDecrement: 5);
    public override string ImagePath => "Sprites/upgrade_quick_vip_image";
    public override string IconPath => "Sprites/upgrade_vip_icon";
}