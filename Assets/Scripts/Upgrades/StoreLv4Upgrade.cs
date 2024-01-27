
using System;

public class StoreLv4Upgrade: Upgrade
{
    public override string Name => "StoreLv4Upgrade";
    public override string Title => "가게 확장 Lv.4";
    public override string Description => "판매 가능한 치킨 수가 500마리로 늘어난다";
    public override decimal Price => 400m;
    public override Type UpgradeConstraint => typeof(StoreLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 4;

    public override string ImagePath => "Sprites/upgrade_store_image";
    public override string IconPath => "Sprites/upgrade_store_image";
}
