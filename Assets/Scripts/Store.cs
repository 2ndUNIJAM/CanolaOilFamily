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

    public float Money;

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
