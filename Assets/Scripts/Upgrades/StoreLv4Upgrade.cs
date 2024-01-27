
using System;

public class StoreLv4Upgrade: Upgrade
{
    public override string Name => "StoreLv4Upgrade";
    public override Type UpgradeConstraint => typeof(StoreLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 4;

    public override string ImagePath => "Sprites/upgrade_store_image";
    public override string IconPath => "Sprites/upgrade_store_image";
}
