public class IngredientCostLv1Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv1Upgrade";
    public override string Title => "원가 절감 Lv.1";
    public override string Description => "0.5$ 감소";
    public override decimal Price => 100m;
    public override int LvConstraint => 1;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 0.5m);
    public override string ImagePath => "Sprites/upgrade_ingredient_image";
}
