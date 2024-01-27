using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Simulation
{
    private const decimal SimulationInterval = 0.5m;
    private const int SimulationIntervalMaxCount = 2;

    // Price for one 
    private static decimal GetFinalPrice(Tile tile, Store store)
    {
        var d = (decimal)Tile.GetDistance(tile, store.Position);
        var stats = store.Upgrade;
        if (stats.FreeDeliveryDistance >= d)
            d = 0;

        var finalPrice = store.Price + (store.DeliveryFee - stats.DeliveryCostDecrement + Event.DeliveryFeeBias) * d;
        // Debug.Log($"Tile: ({tile.Q}, {tile.R}), Store: ({store.Position.Q}, {store.Position.R}), Distance: {d}, Price: {finalPrice}");
        return finalPrice;
    }

    // doesn't consider stock
    private static DecisionType GetDecision(Tile tile, Store player, Store opponent, int playerStock, int opponentStock)
    {
        var playerStat = player.Upgrade;
        var opponentStat = opponent.Upgrade;

        var playerDecisionPrice = GetFinalPrice(tile, player) - playerStat.VersusCostBias;
        var opponentDecisionPrice = GetFinalPrice(tile, opponent) - opponentStat.VersusCostBias;

        // Do not place order when both exceeds MaximumPrice
        if (playerDecisionPrice > tile.MaximumPrice && opponentDecisionPrice > tile.MaximumPrice)
        {
            return DecisionType.None;
        }
        
        // RandomOrder tile actions
        if (tile.SpecialType == SpecialTileType.RandomOrder)
        {
            if (playerDecisionPrice > tile.MaximumPrice)
            {
                if (opponentStock >= tile.PurchaseCount)
                {
                    return DecisionType.Opponent;
                }
                else
                {
                    return DecisionType.None;
                }
            }
            else if (opponentDecisionPrice > tile.MaximumPrice)
            {
                if (playerStock >= tile.PurchaseCount)
                {
                    return DecisionType.Player;
                }
                else
                {
                    return DecisionType.None;
                }
            }
            else
            {
                // Random select player
                if (Random.Range(0, 2) == 0)
                {
                    if (playerStock >= tile.PurchaseCount)
                    {
                        return DecisionType.Player;
                    }
                    else if (opponentStock >= tile.PurchaseCount)
                    {
                        return DecisionType.Opponent;
                    }
                    else
                    {
                        return DecisionType.None;
                    }
                }
                
                // Random select opponent
                else
                {
                    if (opponentStock >= tile.PurchaseCount)
                    {
                        return DecisionType.Opponent;
                    }
                    else if (opponentStock >= tile.PurchaseCount)
                    {
                        return DecisionType.Player;
                    }
                    else
                    {
                        return DecisionType.None;
                    }
                }
            }
        }
        
        // Player is cheaper
        if (playerDecisionPrice < opponentDecisionPrice)
        {
            if (playerStock >= tile.PurchaseCount)
            {
                return DecisionType.Player;
            }
            else if (opponentDecisionPrice <= tile.MaximumPrice && opponentStock >= tile.PurchaseCount)
            {
                return DecisionType.Opponent;
            }
            else
            {
                return DecisionType.None;
            }
        }
        
        // Opponent is cheaper
        if (playerDecisionPrice > opponentDecisionPrice)
        {
            if (opponentStock >= tile.PurchaseCount)
            {
                return DecisionType.Opponent;
            }
            else if (playerDecisionPrice <= tile.MaximumPrice && playerStock >= tile.PurchaseCount)
            {
                return DecisionType.Player;
            }
            else
            {
                return DecisionType.None;
            }
        }
        
        // Both are same
        if (playerStock >= tile.PurchaseCount / 2)
        {
            if (opponentStock >= tile.PurchaseCount / 2)
            {
                return DecisionType.Both;
            }
            else if (playerStock >= tile.PurchaseCount)
            {
                return DecisionType.Player;
            }
            else
            {
                return DecisionType.None;
            }
        }
        else if (opponentStock >= tile.PurchaseCount)
        {
            return DecisionType.Opponent;
        }
        else
        {
            return DecisionType.None;
        }
    }

    private static decimal DecideOpponentPrice(Store player, Store opponent)
    {
        var bestMargin = decimal.MinValue;
        var bestPrice = player.Price;

        for (var price = player.Price - SimulationIntervalMaxCount * SimulationInterval;
             price <= player.Price + SimulationIntervalMaxCount * SimulationInterval;
             price += SimulationInterval)
        {
            var temp = opponent.Price;
            opponent.Price = price;

            var sale = SellChicken(player, opponent, true);
            var currentMargin = sale.enemyMargin;
            
            Debug.Log($"Enemy price: {price}, margin: {sale}");
            
            if (currentMargin > bestMargin)
            {
                bestMargin = currentMargin;
                bestPrice = price;
            }

            opponent.Price = temp;
        }

        Debug.Log($"bestPrice: {bestPrice}");
        return bestPrice;
    }

    public static void Simulate()
    {
        var player = GameManager.Instance.Player;
        var enemy = GameManager.Instance.Enemy;
        player.SaleVolume = 0;
        enemy.SaleVolume = 0;
        player.Profit = 0;
        enemy.Profit = 0;
        enemy.Price = DecideOpponentPrice(player, enemy);
        var margin = SellChicken(player, enemy);
        player.Profit = margin.myMargin - player.Rent;
        enemy.Profit = margin.enemyMargin - enemy.Rent;
        player.Money += player.Profit;
        enemy.Money += enemy.Profit;
    }

    private static (decimal myMargin, decimal enemyMargin) SellChicken(Store player, Store opponent, bool isVirtual = false)
    {
        var playerStock = player.Stock;
        var opponentStock = opponent.Stock;

        var myMargin = 0m;
        var enemyMargin = 0m;

        Tile.ShuffleTileList();

        foreach (var tile in Tile.AllTiles)
        {
            // Skip this tile if current tile is not customer
            if (tile.Type != TileType.Customer) continue;
            
            tile.SetSpacialTileValues();

            var purchaseCount = tile.PurchaseCount * Event.OrderFactor;
            var playerStat = player.Upgrade;
            var opponentStat = opponent.Upgrade;
            var decision = GetDecision(tile, player, opponent, playerStock, opponentStock);

            // Calculates final decision for player and opponent
            switch (decision)
            {
                case DecisionType.Player:
                    playerStock -= purchaseCount;
                    myMargin += (player.Price - (player.IngredientCost + Event.IngredientCostAdjustValue -
                                                 (decimal)playerStat.IngredientCostDecrement)) * purchaseCount;
                    player.SaleVolume += purchaseCount;
                    break;

                case DecisionType.Opponent:
                    opponentStock -= purchaseCount;
                    enemyMargin += (opponent.Price - (opponent.IngredientCost + Event.IngredientCostAdjustValue -
                                                      (decimal)opponentStat.IngredientCostDecrement)) * purchaseCount;
                    opponent.SaleVolume += purchaseCount;
                    break;

                case DecisionType.Both:
                    playerStock -= purchaseCount / 2;
                    opponentStock -= purchaseCount / 2;
                    myMargin += (player.Price - (player.IngredientCost + Event.IngredientCostAdjustValue -
                                                 (decimal)playerStat.IngredientCostDecrement)) * purchaseCount / 2;
                    enemyMargin += (opponent.Price - (opponent.IngredientCost + Event.IngredientCostAdjustValue -
                                                      (decimal)opponentStat.IngredientCostDecrement)) * purchaseCount / 2;
                    player.SaleVolume += purchaseCount / 2;
                    opponent.SaleVolume += purchaseCount / 2;
                    break;

                case DecisionType.None:
                    break;

                default:
                    throw new ArgumentException();
            }

            if (!isVirtual)
            {
                tile.Decision = decision;
            }
        }

        return (myMargin, enemyMargin);
    }
}