
public class StoreLv1Upgrade: Upgrade
{
    public override string Name => "StoreLv1Upgrade";
    public override string Title => "가게 확장 Lv.1";
    public override string Description => "판매 가능한 치킨 수가 100마리로 늘어난다";
    public override decimal Price => 100m;
    public override int ToLevel => 1;

    public override string ImagePath => "Sprites/upgrade_store_image";
}
