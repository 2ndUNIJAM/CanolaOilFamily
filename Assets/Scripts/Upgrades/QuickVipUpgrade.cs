using System;

public class QuickVipUpgrade : Upgrade
{
    public override string Name => "QuickVipUpgrade";
    public override int LvConstraint => 1;
    public override int VipTurnDecrement => 5;
}