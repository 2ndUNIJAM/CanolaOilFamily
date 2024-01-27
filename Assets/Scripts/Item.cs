using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private Store _store;
    public bool usingShield = false;
    public bool isIngredientCostSabotaged = false;
    public const decimal INGREDIENT_COST_SABOTAGE_FACTOR = 2;

    public ItemManager(Store store)
    {
        _store = store;
    }

    private List<IItem> _toBeApplied = new();

    public static void EnemyBuyItem(ItemManager enemyItemManager) // called at start of each control turn
    {
        if (true) // some cond to buy shield
        {
            enemyItemManager.BuyItem(new Shield());
        }

        if (true)
        {
            enemyItemManager.BuyItem(new EnemyIngredientCostIncrease());
        }
        //...

    }

    public void Init()
    {
        usingShield = false;
        isIngredientCostSabotaged = false;
    }

    public void BuyItem(IItem item)
        // player: called when they buys it
        // enemy: called by EnemyBuyItem, at start of each control turn
    {
        item.OnBuy(_store);
        _toBeApplied.Add(item);
    }

    public void ApplyItem() // called at end of control turn
    {
        if (_store.GetEnemy().ItemManager.usingShield)
        {
            Debug.Log("Apply shield of " + _store.ToString());
            return;
        }

        _toBeApplied.ForEach(x => x.OnApply(_store));
    }
}

public interface IItem
{
    public void OnBuy(Store user);
    public void OnApply(Store user);
}

public class EnemyIngredientCostIncrease : IItem
{
    public void OnBuy(Store user)
    {
    }

    public void OnApply(Store user)
    {
        user.GetEnemy().ItemManager.isIngredientCostSabotaged = true;
    }
}

public class ThiefItem : IItem
{
    private const decimal AMOUNT = 100;
    public void OnBuy(Store user)
    {
    }

    public void OnApply(Store user)
    {
        user.GetEnemy().Money -= AMOUNT;
        user.Money += AMOUNT;
    }
}

public class Shield : IItem
{
    public void OnBuy(Store user)
    {
        user.GetEnemy().ItemManager.usingShield = true;
    }

    public void OnApply(Store user)
    {
    }
}
