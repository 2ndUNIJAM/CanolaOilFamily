
public class RentCostLv1Upgrade : Upgrade
{
    public override string Name => "RentCostLv1Upgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(rentCostDecrement: 20);
}
