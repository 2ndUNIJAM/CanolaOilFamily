using System.Collections.Generic;

public class Store
{
    private float _price = 20f;
    public float Price
    {
        get => _price;
        set
        {
            if (value is < 10f or > 30f) { return; }

            _price = value;
            GameManager.Instance.UpdatePriceUI();
        }
    }

    public float DeliveryFee = 1f;
    public int Stock = 100;
    private List<Upgrade> _upgrades;
    public IReadOnlyList<Upgrade> Upgrades => _upgrades;

    public float Money;
}
