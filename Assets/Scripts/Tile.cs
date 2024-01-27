using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Uninitialized,
    Customer,
    MyStore,
    OpponentStore
}

public enum TileState
{
    None,       // Store tiles must have this state
    Player,
    Opponent,
    Both
}

public enum VipType
{
    None,
    MyStore,
    OpponentStore
}

public class Tile : MonoBehaviour
{
    private TileType _type;
    private TileState _state = TileState.None;
    
    private SpriteRenderer _spriteRenderer;
    
    public static List<Tile> AllTiles = new();
    
    public int Q, R;

    [HideInInspector]
    public VipType Vip = VipType.None;
    [HideInInspector]
    public bool IsPreferPermanent = false;
    
    public TileType Type
    {
        get => _type;
        set
        {
            _type = value;
            UpdateSprite();
        }
    }

    public TileState State
    {
        get => _state;
        set
        {
            _state = value;
            UpdateSprite();
        }
    }

    public int PurchaseCount { get; set; } = 10;
    public decimal MaximumPrice { get; set; } = 20m;

    private static Vector2 C_VECTOR = new(0.86602540378f, 0);
    private static Vector2 R_VECTOR = new(-0.43301270189f, -0.75f);

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
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

    private void OnMouseUpAsButton()
    {
        if (!GameManager.Instance.isStorePositioned)
        {
            GameManager.Instance.MakeStore(this);
        }
    }

    private void UpdateSprite()
    {
        switch (_type)
        {
            case TileType.Uninitialized:
                break;
            
            case TileType.Customer:
                int spriteIndex;
                
                // TODO: Change spriteIndex based on special tile type
                spriteIndex = 1;
                
                switch (_state)
                {
                    case TileState.None:
                        break;
                    
                    case TileState.Player:
                        _spriteRenderer.sprite = GameManager.Instance.playerTileSprites[spriteIndex];
                        break;
                    
                    case TileState.Opponent:
                        _spriteRenderer.sprite = GameManager.Instance.enemyTileSprites[spriteIndex];
                        break;
                    
                    case TileState.Both:
                        _spriteRenderer.sprite = GameManager.Instance.bothTileSprites[spriteIndex];
                        break;
                    
                    default:
                        throw new ArgumentException();
                }
                break;
            
            case TileType.MyStore:
                _spriteRenderer.sprite = GameManager.Instance.playerStoreTileSprite;
                break;
            
            case TileType.OpponentStore:
                _spriteRenderer.sprite = GameManager.Instance.enemyStoreTileSprite;
                break;
            
            default:
                throw new ArgumentException();
        }
    }
}
