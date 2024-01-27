
using System;

public class StoreLv2Upgrade: Upgrade
{
    public override string Name => "StoreLv2Upgrade";
    public override Type UpgradeConstraint => typeof(StoreLv1Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 2;

    public override string ImagePath => "Sprites/upgrade_store_image";
    public override string IconPath => "Sprites/upgrade_store_image";
}
