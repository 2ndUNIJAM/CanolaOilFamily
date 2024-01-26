using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum TileType
{
    Customer,
    MyStore,
    OpponentStore
}

public enum PreferType
{
    None,
    MyStore,
    OpponentStore
}

public class Tile
{
    public static List<Tile> AllTiles = new();

    public int Q, R;
    
    public TileType Type;
    
    public PreferType Prefer = PreferType.None;
    public bool IsPreferPermanent = false;
    
    public Tile(int q, int r, TileType type)
    {
        Q = q;
        R = r;
        Type = type;
    }


    /* Helpers */
    public static Tile FindTile(int q, int r)
    {
        return AllTiles.Find(x => (x.Q == q && x.R == r));
    }

    public static void ShuffleTileList()
    {
        System.Random rng = new();
        int n = AllTiles.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (AllTiles[n], AllTiles[k]) = (AllTiles[k], AllTiles[n]);
        }
    }

    public static int GetDistance(Tile a, Tile b)
    {
        return Mathf.Max(Mathf.Abs(a.Q - b.Q), Mathf.Abs(a.R - b.R), Mathf.Abs(a.Q + a.R - b.Q - b.R));
    }

    public static float GetPrice(Tile store, Tile customer, float deliveryFee, float price)
    {
        var distance = GetDistance(store, customer);

        return price + distance * deliveryFee;
    }
}
