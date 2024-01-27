using System;
using UnityEngine;

public enum DecisionType
{
    None,
    Player,
    Opponent,
    Both
}

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

        return store.Price + store.DeliveryFee * d * stats.DeliveryCostFactor * Event.DeliveryFeeFactor;
    }

    // doesn't consider stock
    private static DecisionType GetDecision(Tile tile, Store player, Store opponent)
    {
        var playerStat = player.Upgrade;
        var opponentStat = opponent.Upgrade;

        var playerFinalPrice = GetFinalPrice(tile, player) - playerStat.VersusCostBias -
                               ((tile.Vip == VipType.MyStore) ? playerStat.VipVersusCostBias : 0m);
        var opponentFinalPrice = GetFinalPrice(tile, opponent) - opponentStat.VersusCostBias -
                                 ((tile.Vip == VipType.OpponentStore) ? opponentStat.VipVersusCostBias : 0m);

        if (playerFinalPrice >= tile.MaximumPrice && opponentFinalPrice >= tile.MaximumPrice)
        {
            return DecisionType.None;
        }

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

            var currentMargin = SellChicken(player, opponent).enemyMargin;
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
        enemy.Price = DecideOpponentPrice(player, enemy);
        Debug.Log(enemy.Price);
        var margin = SellChicken(player, enemy);
        Debug.Log(margin);
        player.Money += margin.myMargin;
        enemy.Money += margin.enemyMargin;
    }

    private static (decimal myMargin, decimal enemyMargin) SellChicken(Store player, Store opponent)
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

            var purchaseCount = tile.PurchaseCount * Event.OrderFactor;
            var playerStat = player.Upgrade;
            var opponentStat = opponent.Upgrade;
            var decision = CheckForStock(purchaseCount, playerStock, opponentStock,
                GetDecision(tile, player, opponent));

            // Calculates final decision for player and opponent
            switch (decision)
            {
                case DecisionType.Player:
                    playerStock -= purchaseCount;
                    myMargin +=
                        (player.Price - (player.IngredientCost + Event.IngredientCostAdjustValue - playerStat.IngredientCostDecrement)) *
                        purchaseCount;
                    break;

                case DecisionType.Opponent:
                    opponentStock -= purchaseCount;
                    enemyMargin +=
                        (opponent.Price - (opponent.IngredientCost + Event.IngredientCostAdjustValue -
                                                          opponentStat.IngredientCostDecrement)) *
                        purchaseCount;
                    break;

                case DecisionType.Both:
                    playerStock -= purchaseCount / 2;
                    opponentStock -= purchaseCount / 2;
                    myMargin +=
                        (player.Price - (player.IngredientCost + Event.IngredientCostAdjustValue -
                                                        playerStat.IngredientCostDecrement)) *
                        purchaseCount / 2;
                    enemyMargin +=
                        (opponent.Price - (opponent.IngredientCost + Event.IngredientCostAdjustValue -
                                                          opponentStat.IngredientCostDecrement)) *
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