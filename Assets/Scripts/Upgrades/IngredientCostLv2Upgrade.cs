using System;

public class IngredientCostLv2Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv2Upgrade";
    public override string Title => "원가 절감 Lv.2";
    public override string Description => "1$ 감소";
    public override decimal Price => 500m;
    public override int LvConstraint => 2;
    public override Type UpgradeConstraint => typeof(IngredientCostLv1Upgrade);
    public override bool IsReplaceConstraint => true;

    public override UpgradeStat Stat => new(ingredientCostDecrement: 1);
    public override string ImagePath => "Sprites/upgrade_ingredient_image";
}
