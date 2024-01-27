using System;

public class IngredientCostLv4Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv4Upgrade";
    public override string Title => "원가 절감 Lv.4";
    public override string Description => "2$ 감소";
    public override decimal Price => 400m;
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(IngredientCostLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 2);
    public override string ImagePath => "Sprites/upgrade_ingredient_image";
    public override string IconPath => "Sprites/upgrade_ingredient_image";
}
