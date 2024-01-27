using System;

public class IngredientCostLv3Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv3Upgrade";
    public override int LvConstraint => 3;
    public override Type UpgradeConstraint => typeof(IngredientCostLv2Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 1.5m);
    public override string ImagePath => "Sprites/upgrade_ingredient_image";
    public override string IconPath => "Sprites/upgrade_ingredient_image";
}
