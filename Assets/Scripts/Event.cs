using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    None,
    DoubleDeliveryFee,
    DoubleOrder,
    AdjustIngredientCost,
    AdjustRent
}

public static class Event
{
    public static EventType CurrentEventType { get; private set; } = EventType.None;

    public static int OrderFactor { get; private set; } = 1;
    public static decimal DeliveryFeeFactor { get; private set; } = 1;
    public static decimal IngredientCostAdjustValue { get; private set; } = 0;
    public static decimal RentAdjustValue { get; private set; } = 0;

    public static List<(int, int, Action, (string, string, Sprite))> _futureEvents;

    public static void Init()
    {
        List<Sprite> _sprites = new()
        {
            Resources.Load<Sprite>("Evnet/Delievery"),
            null,//Resources.Load<Sprite>("Evnet/"),
            Resources.Load<Sprite>("Evnet/Soccer"),
            Resources.Load<Sprite>("Evnet/Storm"),
            Resources.Load<Sprite>("Evnet/Spanner"),
            Resources.Load<Sprite>("Evnet/Virus"),
            Resources.Load<Sprite>("Evnet/Rice"),
            Resources.Load<Sprite>("Evnet/Snow"),
        };

        _futureEvents = new()
        {
            (6, 10, () => SetDoubleDeliveryFee(0.66666666666m), ("배달 이벤트", "배달비 1/3 감소", _sprites[0])),
            // dangol
            (16, 20, () => SetDoubleOrder(), ("월드컵", "구매 치킨 수 2배 증가", _sprites[2])),
            (24, 28, () => SetDoubleDeliveryFee(1.33333333333m), ("폭우", "배달비 1/3 증가", _sprites[3])),
            (30, 34, () => SetAdjustRent(50), ("가게 보수", "임대료 $50 증가", _sprites[4])),
            (36, 40, () => SetAdjustIngredientCost(2), ("조류 독감", "재료비 $2 증가", _sprites[5])),
            (42, 44, () => SetAdjustIngredientCost(-1), ("풍년", "재료비 $1 감소", _sprites[6])),
            (50, 52, () => SetDoubleDeliveryFee(1.66666666666m), ("폭설", "배달비 2/3 증가", _sprites[7])),
        };
    }

    public static (string name, string desc, Sprite spr)? FireEvent(int weeks)
    {
        var e = _futureEvents[0];
        var r = new System.Random();

        if (weeks < e.Item1) // not yet to occur event
        {
            return null;
        }

        // event should happen as it did not happen until now (week Item2)
        //  || event happens as it made a dice roll
        else if (weeks == e.Item2 || r.Next(e.Item2 - e.Item1) == 0)
        {
            // start event
            e.Item3();
            _futureEvents.RemoveAt(0);
            return e.Item4;
        }

        return null;
    }

    public static void ResetEvent()
    {
        CurrentEventType = EventType.None;
        OrderFactor = 1;
        DeliveryFeeFactor = 1;
        IngredientCostAdjustValue = 0;
        RentAdjustValue = 0;
    }
    
    public static void SetDoubleDeliveryFee(decimal factor)
    {
        CurrentEventType = EventType.DoubleDeliveryFee;
        DeliveryFeeFactor = factor;
    }
    
    public static void SetDoubleOrder()
    {
        CurrentEventType = EventType.DoubleOrder;
        OrderFactor = 2;
    }
    
    public static void SetAdjustIngredientCost(decimal value)
    {
        CurrentEventType = EventType.AdjustIngredientCost;
        IngredientCostAdjustValue = value;
    }
    
    public static void SetAdjustRent(decimal value)
    {
        CurrentEventType = EventType.AdjustRent;
        RentAdjustValue = value;
    }
}
