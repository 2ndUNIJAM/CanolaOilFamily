public class UpgradeStat
{
    public int FreeDeliveryDistance;
    public decimal DeliveryCostFactor;
    public decimal IngredientCostDecrement;
    public bool IsPriorInVersus;
    public decimal VersusCostBias;
    public int VipTurnDecrement;
    public decimal VipVersusCostBias;
    public decimal RentCostDecrement;

    public UpgradeStat(int freeDeliveryDistance = 0, decimal deliveryCostFactor = 1, decimal ingredientCostDecrement = 0,
        bool isPriorInVersus = false, decimal versusCostBias = 0, int vipTurnDecrement = 0, decimal vipVersusCostBias = 0, decimal rentCostDecrement = 0)
    {
        FreeDeliveryDistance = freeDeliveryDistance;
        DeliveryCostFactor = deliveryCostFactor;
        IngredientCostDecrement = ingredientCostDecrement;
        IsPriorInVersus = isPriorInVersus;
        VersusCostBias = versusCostBias;
        VipTurnDecrement = vipTurnDecrement;
        VipVersusCostBias = vipVersusCostBias;
        RentCostDecrement = rentCostDecrement;
    }

    public static UpgradeStat operator +(UpgradeStat a, UpgradeStat b) => new(
        a.FreeDeliveryDistance + b.FreeDeliveryDistance,
        a.DeliveryCostFactor + b.DeliveryCostFactor,
        a.IngredientCostDecrement + b.IngredientCostDecrement,
        a.IsPriorInVersus || b.IsPriorInVersus,
        a.VersusCostBias + b.VersusCostBias,
        a.VipTurnDecrement + b.VipTurnDecrement,
        a.VipVersusCostBias + b.VipVersusCostBias,
        a.RentCostDecrement + b.RentCostDecrement
    );
}