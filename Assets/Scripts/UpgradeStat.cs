public class UpgradeStat
{
    public int FreeDeliveryDistance;
    public float DeliveryCostFactor;
    public float IngredientCostDecrement;
    public bool IsPriorInVersus;
    public float VersusCostBias;
    public int VipTurnDecrement;
    public float VipVersusCostBias;

    public UpgradeStat(int freeDeliveryDistance = 0, float deliveryCostFactor = 1f, float ingredientCostDecrement = 0f,
        bool isPriorInVersus = false, float versusCostBias = 0f, int vipTurnDecrement = 0, float vipVersusCostBias = 0f)
    {
        FreeDeliveryDistance = freeDeliveryDistance;
        DeliveryCostFactor = deliveryCostFactor;
        IngredientCostDecrement = ingredientCostDecrement;
        IsPriorInVersus = isPriorInVersus;
        VersusCostBias = versusCostBias;
        VipTurnDecrement = vipTurnDecrement;
        VipVersusCostBias = vipVersusCostBias;
    }

    public static UpgradeStat operator +(UpgradeStat a, UpgradeStat b) => new(
        a.FreeDeliveryDistance + b.FreeDeliveryDistance,
        a.DeliveryCostFactor + b.DeliveryCostFactor,
        a.IngredientCostDecrement + b.IngredientCostDecrement,
        a.IsPriorInVersus || b.IsPriorInVersus,
        a.VersusCostBias + b.VersusCostBias,
        a.VipTurnDecrement + b.VipTurnDecrement,
        a.VipVersusCostBias + b.VipVersusCostBias
    );
}