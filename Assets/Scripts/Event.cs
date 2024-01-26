public enum EventType
{
    None,
    DoubleDeliveryFee,
    DoubleOrder,
    AdjustIngredientCost
}

public static class Event
{
    public static EventType CurrentEventType { get; private set; } = EventType.None;

    public static int OrderFactor { get; private set; } = 1;
    public static float DeliveryFeeFactor { get; private set; } = 1f;
    public static float IngredientCostAdjustValue { get; private set; } = 0f;

    public static void ResetEvent()
    {
        CurrentEventType = EventType.None;
        OrderFactor = 1;
        DeliveryFeeFactor = 1f;
        IngredientCostAdjustValue = 0f;
    }
    
    public static void SetDoubleDeliveryFee()
    {
        CurrentEventType = EventType.DoubleDeliveryFee;
        OrderFactor = 1;
        DeliveryFeeFactor = 2f;
        IngredientCostAdjustValue = 0f;
    }
    
    public static void SetDoubleOrder()
    {
        CurrentEventType = EventType.DoubleOrder;
        OrderFactor = 2;
        DeliveryFeeFactor = 1f;
        IngredientCostAdjustValue = 0f;
    }
    
    public static void SetAdjustIngredientCost(float value)
    {
        CurrentEventType = EventType.AdjustIngredientCost;
        OrderFactor = 1;
        DeliveryFeeFactor = 1f;
        IngredientCostAdjustValue = value;
    }
}
