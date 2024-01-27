using System;

public class IngredientCostLv2Upgrade : Upgrade
{
    public override string Name => "IngredientConstLv2Upgrade";
    public override int LvConstraint => 2;
    public override Type UpgradeConstraint => typeof(IngredientCostLv1Upgrade);
    public override bool IsReplaceConstraint => true;

    public override UpgradeStat Stat => new(ingredientCostDecrement: 1);
}
