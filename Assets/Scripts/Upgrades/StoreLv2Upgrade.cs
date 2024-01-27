
using System;

public class StoreLv2Upgrade: Upgrade
{
    public override string Name => "StoreLv2Upgrade";
    public override string Title => "가게 확장 Lv.2";
    public override string Description => "판매 가능한 치킨 수가 200마리로 늘어난다";
    public override decimal Price => 1000m;
    public override Type UpgradeConstraint => typeof(StoreLv1Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 2;

    public override string ImagePath => "Sprites/upgrade_store_image";
}
