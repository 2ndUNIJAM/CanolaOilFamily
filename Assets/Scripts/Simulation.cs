using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Simulation
{
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

            
        }

        return totalMargin;
    }
}