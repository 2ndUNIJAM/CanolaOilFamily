using System;

public class VersusPriorityUpgrade : Upgrade
{
    public override string Name => "VersusPriorityUpgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(isPriorInVersus: true);
    public override string ImagePath => "Sprites/upgrade_versus_priority_image";
    public override string IconPath => "Sprites/upgrade_versus_icon";
}
