using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _price = 20000;
    public float Price
    {
        get
        {
            return _price;
        }
        set
        {
            if (value < 10000 || value > 30000) { return; }

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
