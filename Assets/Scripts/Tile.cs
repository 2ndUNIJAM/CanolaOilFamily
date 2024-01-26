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

public class Tile : MonoBehaviour
{
    public static List<Tile> AllTiles = new();

    public int Q, R;
    
    public TileType Type;
    
    public PreferType Prefer = PreferType.None;
    public bool IsPreferPermanent = false;
    public int PurchaseCount = 10;

    private static Vector2 C_VECTOR = new(0.86602540378f, 0);
    private static Vector2 R_VECTOR = new(-0.43301270189f, -0.75f);


    public void Init(int q, int r, TileType type)
    {
        Q = q;
        R = r;
        Type = type;

        var col = q + (r - (r & 1) / 2);

        transform.position = col * C_VECTOR + r * R_VECTOR;
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
}
