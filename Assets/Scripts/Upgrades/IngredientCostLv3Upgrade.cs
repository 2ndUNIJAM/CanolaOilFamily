using System;

public class IngredientCostLv3Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv3Upgrade";
    public override string Title => "원가 절감 Lv.3";
    public override string Description => "1.5$ 감소";
    public override decimal Price => 1000m;
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(IngredientCostLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 1.5m);
    public override string ImagePath => "Sprites/upgrade_ingredient_image";
}
