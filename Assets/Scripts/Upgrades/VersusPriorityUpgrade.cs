using System;

public class VersusPriorityUpgrade : Upgrade
{
    public override string Name => "VersusPriorityUpgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(isPriorInVersus: true);
}
