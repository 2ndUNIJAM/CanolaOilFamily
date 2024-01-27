using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Store
{
    private float _price = 20f;

    public float Price
    {
        get => _price;
        set
        {
            if (value is < 10f or > 30f)
            {
                return;
            }

            _price = value;

            if (this == GameManager.Instance.Player)
                GameManager.Instance.UpdatePriceUI();
        }
    }

    public Tile Position;
    public float DeliveryFee = 1f;
    public float IngredientCost = 1f;
    public float Rent = 150f;
    public int Stock = 100;
    public int Level = 1;
    private List<Upgrade> _upgrades;
    public UpgradeStat Upgrade = new();
    public ItemManager ItemManager;

    public bool IsUpgradeAvailable(Upgrade upgrade) =>
        upgrade.LvConstraint <= Level && (upgrade.UpgradeConstraint == null ||
                                          _upgrades.Any(upg => upg.GetType() == upgrade.UpgradeConstraint));

    public void BuyUpgrade(Upgrade upgrade)
    {
        if (!IsUpgradeAvailable(upgrade)) return;
        if (upgrade.UpgradeConstraint != null)
        {
            var index = _upgrades.FindIndex(upg => upg.GetType() == upgrade.UpgradeConstraint);
            if (index >= 0) 
                _upgrades.RemoveAt(index);
        }
        _upgrades.Add(upgrade);
        Upgrade = _upgrades.Aggregate(new UpgradeStat(), (stat, u) => stat + u.Stat);
    }

    public float Money;

    public override string ToString()
    {
        return this == GameManager.Instance.Player ? "Player" : "Enemy";
    }

    public Store GetEnemy() => GameManager.Instance.FindMyEnemy(this);
}