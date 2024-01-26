public abstract class Upgrade
{
    public virtual string Name => "";

    public virtual int LvConstraint => 0;
    public virtual bool IsReplaceConstraint => false;
    public virtual int FreeDeliveryDistance => 0;
    public virtual float DeliveryCostFactor => 1;
    public virtual float IngredientCostDecrement => 0;
    public virtual bool IsPriorInVersus => false;
    public virtual float VersusCostBias => 0;
    public virtual int VipTurnDecrement => 0;
    public virtual float VipVersusCostBias => 0;
}