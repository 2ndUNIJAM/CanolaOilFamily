
public class RentCostLv1Upgrade : Upgrade
{
    public override string Name => "RentCostLv1Upgrade";
    public override string Title => "임대료 절감 Lv.1";
    public override string Description => "임대료가 30$ 감소한다";
    public override decimal Price => 100m;
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(rentCostDecrement: 20);
    public override string ImagePath => "Sprites/upgrade_rent_image";
    public override string IconPath => "Sprites/upgrade_rent_image";
}
