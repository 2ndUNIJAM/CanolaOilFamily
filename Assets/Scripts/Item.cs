using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    private Store _store;
    
    public const decimal INGREDIENT_COST_SABOTAGE_FACTOR = 2;

    // notify flag
    public bool DoBlocked = false;
    public bool IsThief = false;
    public bool IsFlamer = false;
    
    // item usage flag
    public bool usingShield = false;
    public bool isIngredientCostSabotaged = false;
    private List<IItem> _toBeApplied = new();
    public decimal FixedPrice = -1;

    public ItemManager(Store store)
    {
        _store = store;
    }


    public static void EnemyTryBuyThief(ItemManager enemyItemManager) // called at start of each control turn
    {
        if (GameManager.Instance.Weeks >= 15 
            && new System.Random().Next(5) == 0 
            && enemyItemManager._store.Money > new ThiefItem().Price)
        {
            enemyItemManager.BuyItem(new ThiefItem());
        }
    }

    public static void EnemyTryBuyShield(ItemManager enemyItemManager)
    {
        if (new System.Random().Next(2) == 0 && enemyItemManager._store.Money > new Shield().Price)
        {
            enemyItemManager.BuyItem(new Shield());
        }

    }

    public void Init()
    {
        usingShield = false;
        isIngredientCostSabotaged = false;
        DoBlocked = false;
        IsThief = false;
        IsFlamer = false;
        _toBeApplied.Clear();
        FixedPrice = -1;
    }

    public void BuyItem(IItem item)
        // player: called when they buys it
        // enemy: called by EnemyBuyItem, at start of each control turn
    {
        _store.Money -= item.Price;
        item.OnBuy(_store);
        _toBeApplied.Add(item);
    }

    public void ApplyItem() // called at the end of control turn
    {
        if (_store.GetEnemy().ItemManager.usingShield)
        {
            DoBlocked = true;
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
    public int Price => 150;

    public void OnBuy(Store user)
    {
        ItemManager.EnemyTryBuyShield(user.GetEnemy().ItemManager);
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
    public int Price => 300;

    public void OnBuy(Store user)
    {
        user.ItemManager.IsThief = true;
        ItemManager.EnemyTryBuyShield(user.GetEnemy().ItemManager);

    }

    public void OnApply(Store user)
    {
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

public class SpyItem : IItem
{
    public string Name => "도청";

    public string Description => "이번 주 상대의 판매 가격을 미리 확인합니다.";

    public int Price => 200;

    public void OnApply(Store user)
    {
    }

    public void OnBuy(Store user)
    {
        var r = new System.Random();
        user.GetEnemy().ItemManager.FixedPrice = user.GetEnemy().Price + 0.5m * (r.Next(5) - 2);
        ItemPanel.Instance.Notify("상대는 이번 주 동안 $" + user.GetEnemy().ItemManager.FixedPrice + "에 치킨을 판매합니다.");
    }
}
