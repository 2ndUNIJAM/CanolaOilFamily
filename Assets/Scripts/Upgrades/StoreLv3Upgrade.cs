
using System;

public class StoreLv3Upgrade: Upgrade
{
    public override string Name => "StoreLv3Upgrade";
    public override string Title => "가게 확장 Lv.3";
    public override string Description => "판매 가능한 치킨 수가 300마리로 늘어난다";
    public override decimal Price => 300m;
    public override Type UpgradeConstraint => typeof(StoreLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override int ToLevel => 3;

    public override string ImagePath => "Sprites/upgrade_store_image";
}
