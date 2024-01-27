using System;

public class IngredientCostLv4Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv4Upgrade";
    public override int LvConstraint => 4;
    public override Type UpgradeConstraint => typeof(IngredientCostLv3Upgrade);
    public override bool IsReplaceConstraint => true;
    public override UpgradeStat Stat => new(ingredientCostDecrement: 2);
    public override string ImagePath => "Sprites/upgrade_ingredient_cost_lv4_image";
    public override string IconPath => "Sprites/upgrade_ingredient_icon";
}
