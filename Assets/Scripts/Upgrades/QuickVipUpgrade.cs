using System;

public class QuickVipUpgrade : Upgrade
{
    public override string Name => "QuickVipUpgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(vipTurnDecrement: 5);
}