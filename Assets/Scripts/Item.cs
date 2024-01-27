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
    public string Name { get; }
    public string Description { get; }
    public int Price { get; }

    public void OnBuy(Store user);
    public void OnApply(Store user);
}

public class EnemyIngredientCostIncrease : IItem
{
    public string Name => "방화";
    public string Description => "상대 가게에 불을 질러 재료비를 증가시킨다.";
    public int Price => 100;

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
    public string Name => "도둑질";
    public string Description => "상대 가게에서 $100을 훔쳐 온다.";
    public int Price => 100;

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
    public string Name => "야간 경비";
    public string Description => "이번 주 동안 오는 모든 방해 공작을 무효화한다.";
    public int Price => 100;
    public void OnBuy(Store user)
    {
        user.GetEnemy().ItemManager.usingShield = true;
    }

    public void OnApply(Store user)
    {
    }
}
