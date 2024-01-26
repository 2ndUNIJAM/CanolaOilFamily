using System.Collections;
using System.Collections.Generic;
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

    public Tile FindTile(int q, int r)
    {
        return AllTiles.Find(x => (x.Q == q && x.R == r));
    }

    public static int GetDistance(Tile a, Tile b)
    {
        return Mathf.Max(Mathf.Abs(a.Q - b.Q), Mathf.Abs(a.R - b.R), Mathf.Abs(a.Q + a.R - b.Q - b.R));
    }

    public static int GetPrice(Tile store, Tile customer, int deliveryFee, int price)
    {
        var distance = GetDistance(store, customer);

        return price + distance * deliveryFee;
    }
}
