using System;

public enum DecisionType
{
    None,
    Player,
    Opponent,
    Both
}

public static class Simulation
{
    private const float SimulationInterval = 0.5f;
    private const int SimulationIntervalMaxCount = 2;
    
    private static float GetFinalPrice(Tile tile, Store store)
    {
        // Price for one   
        return store.Price + store.DeliveryFee * Tile.GetDistance(tile, store.Position) * Event.DeliveryFeeFactor;
    }
    
    private static DecisionType GetDecision(Tile tile, Store player, Store opponent)
    {
        // doesn't consider stock
        
        var playerFinalPrice = GetFinalPrice(tile, player);
        var opponentFinalPrice = GetFinalPrice(tile, opponent);
        
        // TODO: Add calculations for events
        
        if (playerFinalPrice < opponentFinalPrice)
        {
            return DecisionType.Player;
        }
        else if (playerFinalPrice > opponentFinalPrice)
        {
            return DecisionType.Opponent;
        }
        else
        {
            return DecisionType.Both;
        }
    }

    private static DecisionType CheckForStock(int purchaseCount, int playerStock, int opponentStock,
        DecisionType priorDecision)
    {
        switch (priorDecision)
        {
            case DecisionType.Player:
                if (playerStock >= purchaseCount)
                {
                    return DecisionType.Player;
                }
                else if (opponentStock >= purchaseCount)
                {
                    return DecisionType.Opponent;
                }
                else
                {
                    return DecisionType.None;
                }
            case DecisionType.Opponent:
                if (opponentStock >= purchaseCount)
                {
                    return DecisionType.Opponent;
                }
                else if (playerStock >= purchaseCount)
                {
                    return DecisionType.Player;
                }
                else
                {
                    return DecisionType.None;
                }
            case DecisionType.Both:
                if (playerStock >= purchaseCount / 2 && opponentStock >= purchaseCount / 2)
                {
                    return DecisionType.Both;
                }
                else if (playerStock >= purchaseCount / 2)
                {
                    return CheckForStock(purchaseCount, playerStock, opponentStock, DecisionType.Player);
                }
                else if (opponentStock >= purchaseCount / 2)
                {
                    return CheckForStock(purchaseCount, playerStock, opponentStock, DecisionType.Opponent);
                }
                else
                {
                    return DecisionType.None;
                }
            case DecisionType.None:
                return DecisionType.None;
            default:
                throw new ArgumentException();
        }
    }

    private static float DecideOpponentPrice(Store player, Store opponent)
    {
        var bestMargin = float.MinValue;
        var bestPrice = player.Price;

        for (var price = player.Price - SimulationIntervalMaxCount * SimulationInterval;
            price <= player.Price + SimulationIntervalMaxCount * SimulationInterval;
            price += SimulationInterval)
        {
            var temp = opponent.Price;
            opponent.Price = price;
            
            var currentMargin = SellChicken(player, opponent).enemyMargin;
            if (currentMargin > bestMargin)
            {
                bestMargin = currentMargin;
                bestPrice = price;
            }

            opponent.Price = temp;
        }

        return bestPrice;
    }

    public static void Simulate()
    {
        var player = GameManager.Instance.Player;
        var enemy = GameManager.Instance.Enemy;
        enemy.Price = DecideOpponentPrice(player, enemy);
        var margin = SellChicken(player, enemy);
        player.Money += margin.myMargin;
        enemy.Money += margin.enemyMargin;
    }

    private static (float myMargin, float enemyMargin) SellChicken(Store player, Store opponent)
    {
        var playerStock = player.Stock;
        var opponentStock = opponent.Stock;

        var myMargin = 0f;
        var enemyMargin = 0f;

        Tile.ShuffleTileList();

        foreach (var tile in Tile.AllTiles)
        {
            // Skip this tile if current tile is not customer
            if (tile.Type != TileType.Customer) continue;

            var purchaseCount = tile.PurchaseCount * Event.OrderFactor;

            // Calculates final decision for player and opponent
            switch (CheckForStock(purchaseCount, playerStock, opponentStock, GetDecision(tile, player, opponent)))
            {
                case DecisionType.Player:
                    playerStock -= purchaseCount;
                    myMargin +=
                        (GetFinalPrice(tile, player) - (player.IngredientCost + Event.IngredientCostAdjustValue)) *
                        purchaseCount;
                    break;

                case DecisionType.Opponent:
                    opponentStock -= purchaseCount;
                    enemyMargin +=
                        (GetFinalPrice(tile, opponent) - (opponent.IngredientCost + Event.IngredientCostAdjustValue)) *
                        purchaseCount;
                    break;

                case DecisionType.Both:
                    playerStock -= purchaseCount / 2;
                    opponentStock -= purchaseCount / 2;
                    myMargin +=
                        (GetFinalPrice(tile, player) - (player.IngredientCost + Event.IngredientCostAdjustValue)) *
                        purchaseCount / 2;
                    enemyMargin +=
                        (GetFinalPrice(tile, opponent) - (opponent.IngredientCost + Event.IngredientCostAdjustValue)) *
                        purchaseCount / 2;
                    break;

                case DecisionType.None:
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        return (myMargin, enemyMargin);
    }

}
