
using System;

public class StoreLv3Upgrade: Upgrade
{
    public override string Name => "StoreLv3Upgrade";
    public override Type UpgradeConstraint => typeof(StoreLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 3;

    public override string ImagePath => "Sprites/upgrade_store_image";
    public override string IconPath => "Sprites/upgrade_store_image";
}
