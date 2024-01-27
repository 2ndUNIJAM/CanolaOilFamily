public class UpgradeStat
{
    public readonly int FreeDeliveryDistance;
    public readonly decimal DeliveryCostDecrement;
    public readonly decimal IngredientCostDecrement;
    public readonly bool IsPriorInVersus;
    public readonly decimal VersusCostBias;
    public readonly int VipTurnDecrement;
    public readonly decimal VipVersusCostBias;
    public readonly decimal RentCostDecrement;

    public UpgradeStat(int freeDeliveryDistance = 0, decimal deliveryCostDecrement = 0, decimal ingredientCostDecrement = 0,
        bool isPriorInVersus = false, decimal versusCostBias = 0, int vipTurnDecrement = 0, decimal vipVersusCostBias = 0, decimal rentCostDecrement = 0)
    {
        FreeDeliveryDistance = freeDeliveryDistance;
        DeliveryCostDecrement = deliveryCostDecrement;
        IngredientCostDecrement = ingredientCostDecrement;
        IsPriorInVersus = isPriorInVersus;
        VersusCostBias = versusCostBias;
        VipTurnDecrement = vipTurnDecrement;
        VipVersusCostBias = vipVersusCostBias;
        RentCostDecrement = rentCostDecrement;
    }

    public static UpgradeStat operator +(UpgradeStat a, UpgradeStat b) => new(
        a.FreeDeliveryDistance + b.FreeDeliveryDistance,
        a.DeliveryCostDecrement + b.DeliveryCostDecrement,
        a.IngredientCostDecrement + b.IngredientCostDecrement,
        a.IsPriorInVersus || b.IsPriorInVersus,
        a.VersusCostBias + b.VersusCostBias,
        a.VipTurnDecrement + b.VipTurnDecrement,
        a.VipVersusCostBias + b.VipVersusCostBias,
        a.RentCostDecrement + b.RentCostDecrement
    );
}