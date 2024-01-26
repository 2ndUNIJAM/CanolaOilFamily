public class IngredientCostLv1Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv1Upgrade";
    public override int LvConstraint => 1;
    public override float IngredientCostDecrement => 0.5f;
}
