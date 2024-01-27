public class IngredientCostLv1Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv1Upgrade";
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 0.5m);
    public override string ImagePath => "Sprites/upgrade_ingredient_cost_lv1_image";
    public override string IconPath => "Sprites/upgrade_ingredient_icon";
}
