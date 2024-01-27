using System.Collections.Generic;
using System.Linq;

public class Store
{
    private decimal _price = 20;

    public decimal Price
    {
        get => _price;
        set
        {
            if (value is < 10 or > 30)
            {
                return;
            }

            _price = value;

            if (this == GameManager.Instance.Player)
                GameManager.Instance.UpdatePriceUI();
        }
    }

    public Tile Position;
    public decimal DeliveryFee = 1;
    public decimal IngredientCost = 1;
    public decimal Rent = 150;
    public int Stock = 100;
    public int Level = 1;
    private List<Upgrade> _upgrades;
    public UpgradeStat Upgrade = new();
    public ItemManager ItemManager;

    public Store()
    {
        ItemManager = new(this);
    }

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

    public decimal Money;

    public override string ToString()
    {
        return this == GameManager.Instance.Player ? "Player" : "Enemy";
    }

    public Store GetEnemy() => GameManager.Instance.FindMyEnemy(this);
}