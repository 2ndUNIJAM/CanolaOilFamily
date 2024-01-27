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

    public static void ResetEvent()
    {
        CurrentEventType = EventType.None;
        OrderFactor = 1;
        DeliveryFeeFactor = 1;
        IngredientCostAdjustValue = 0;
        RentAdjustValue = 0;
    }
    
    public static void SetDoubleDeliveryFee()
    {
        CurrentEventType = EventType.DoubleDeliveryFee;
        DeliveryFeeFactor = 2;
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
