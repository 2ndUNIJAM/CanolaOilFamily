
public class RentCostLv1Upgrade : Upgrade
{
    public override string Name => "RentCostLv1Upgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(rentCostDecrement: 20);
    public override string ImagePath => "Sprites/upgrade_rent_cost_lv1_image";
    public override string IconPath => "Sprites/upgrade_rent_icon";
}
