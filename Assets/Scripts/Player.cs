using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Upgrade
{
    public string Name { get; protected set; }

    public int LvConstraint = 0;
    public bool isReplaceConstraint = false;
    public int FreeDeliveryDistance = 0;
    public float DeliveryCostFactor = 1;
    public float IngredientCostDecrement = 0;
    public bool IsPriorInVersus = false;
    public float VersusCostBias = 0;
    public int VipTurnDecrement = 0;
    public float VipVersusCostBias = 0;
}

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
