using System;
using System.Collections;
using System.Collections.Generic;
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
    public static float GetFinalPrice(Tile tile, Store store)
    {
        // Price for one
        
        return store.Price + store.DeliveryFee * Tile.GetDistance(tile, Tile.FindTile(0, 0));
    }

    
    public static DecisionType GetDecision(Tile tile, Store player, Store opponent)
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

    public static DecisionType CheckForStock(int purchaseCount, int playerStock, int opponentStock,
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
                    return DecisionType.Player;
                }
                else if (opponentStock >= purchaseCount / 2)
                {
                    return DecisionType.Opponent;
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
    
    public static float GetOpponentMargin(Store player, Store opponent)
    {
        // Note that player is player's price and delivery fee, not opponent's.

        var totalMargin = 0f;
        
        var playerStock = player.Stock;
        var opponentStock = opponent.Stock;

        Tile.ShuffleTileList();

        foreach (var tile in Tile.AllTiles)
        {
            // Breaks if opponent's stock is 0
            if (opponentStock == 0) break;
            
            // Skip this tile if current tile is not customer
            if (tile.Type != TileType.Customer) continue;

            // Calculates final decision for player and opponent
            switch (CheckForStock(tile.PurchaseCount, playerStock, opponentStock, GetDecision(tile, player, opponent)))
            {
                case DecisionType.Player:
                    playerStock -= tile.PurchaseCount;
                    break;
                case DecisionType.Opponent:
                    opponentStock -= tile.PurchaseCount;
                    totalMargin += GetFinalPrice(tile, opponent) * tile.PurchaseCount;
                    break;
                case DecisionType.Both:
                    playerStock -= tile.PurchaseCount / 2;
                    opponentStock -= tile.PurchaseCount / 2;
                    totalMargin += GetFinalPrice(tile, opponent) * tile.PurchaseCount / 2;
                    break;
                case DecisionType.None:
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        return totalMargin;
    }
}