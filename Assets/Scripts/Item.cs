using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ItemManager
{
    public bool EnemyUsingShield = false;

    delegate void ItemUse();
    List<ItemUse> toBeApplied = new();

    public void EnemyBuyItem() // called at start of each control turn
    {
        if (true) // buy shield
        {
            EnemyUsingShield = true;
        }


    }

    public void ApplyEnemyItem()
    {

    }
}

public interface IItem
{
    public void Use(Store user);
}

public class EnemyIngredientCostIncrease : IItem
{
    public void Use(Store user)
    {
        throw new NotImplementedException();
    }
}

public class ThiefItem : IItem
{
    private const float AMOUNT = 100f;

    public void Use(Store user)
    {
        GameManager.Instance.FindMyEnemy(user).Money -= AMOUNT;
        user.Money += AMOUNT;
    }
}

public class Shield : IItem
{
    public void Use(Store user)
    {
        throw new NotImplementedException();
    }
}
