using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float DeliveryFee = 1000f;
    public int Stock = 100;

    public float Money;
}
