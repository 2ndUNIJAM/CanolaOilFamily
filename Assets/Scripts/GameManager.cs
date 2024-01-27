using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    public const int MaxWeeks = 52;
    
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private Button _simulateButton;
    [SerializeField]
    private TMPro.TMP_Text _priceText;
    [SerializeField]
    private Button _increasePrice;
    [SerializeField]
    private Button _decreasePrice;
    [SerializeField]
    private TMPro.TMP_Text _myMoney;
    [SerializeField]
    private TMPro.TMP_Text _enemyMoney;
    [SerializeField]
    private TMPro.TMP_Text _myDeliveryFee;
    [SerializeField]
    private TMPro.TMP_Text _enemyDeliveryFee;
    [SerializeField]
    private TMPro.TMP_Text _myIngreCost;
    [SerializeField]
    private TMPro.TMP_Text _enemyIngreCost;
    [SerializeField]
    private TMPro.TMP_Text _weekText;

    private GameObject _tilePrefab;

    [Header("Sprites")]
    public Sprite playerStoreTileSprite;
    public Sprite enemyStoreTileSprite;
    
    [Space(10)]
    public Sprite[] normalTileSprites;
    public Sprite[] playerTileSprites;
    public Sprite[] enemyTileSprites;
    public Sprite[] bothTileSprites;
    
    [Header("Values")]
    private int _weeks; // current weeks
    public int Weeks
    {
        get { return _weeks; }
        set
        {
            _weeks = value;
            _weekText.text = _weeks.ToString();
        }
    }

    public bool isStorePositioned = false;
    
    public Store Player;
    public Store Enemy;
    
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Player = new Store();
        Enemy = new Store();

        _weeks = 0;

        _tilePrefab = Resources.Load<GameObject>("TileObject");
        _increasePrice.onClick.AddListener(() => Player.Price += 0.5m);
        _decreasePrice.onClick.AddListener(() => Player.Price -= 0.5m);
        _simulateButton.onClick.AddListener(StartSimulationPhase);

        for (int r = -3; r <= 3; r++)
        {
            if (r < 0)
            {
                for (int q = -3 - r; q <= 3; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab).GetComponent<Tile>();
                    x.Init(q, r, TileType.Uninitialized);
                    Tile.AllTiles.Add(x);
                }
            }
            else
            {
                for (int q = -3; q <= 3 - r; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab).GetComponent<Tile>();
                    x.Init(q, r, TileType.Uninitialized);
                    Tile.AllTiles.Add(x);
                }
            }
        }
        
        // Set tiles initial validity
        // Decision of uninitialized tiles shows validity
        var center = Tile.FindTile(0, 0);
        
        foreach (var tile in Tile.AllTiles)
        {
            if (tile == center || Tile.GetDistance(tile, center) == 1 || (tile.Q >= -1 && tile.S <= +1))
            {
                tile.Decision = DecisionType.Opponent;
            }
            else
            {
                tile.Decision = DecisionType.Player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePriceUI()
    {
        _priceText.text = Player.Price.ToString();
    }

    public void UpdateMoneyUI(Store store)
    {
        if (store == Player)
        {
            _myMoney.text = "Money: " + Player.Money.ToString();
        }
        else
        {
            _enemyMoney.text = "Money: " + Enemy.Money.ToString();
        }
    }

    public void UpdateDeliveryFeeUI(Store store)
    {
        if(store == Player)
        {
            _myDeliveryFee.text = "Deliv. fee: " + Player.DeliveryFee.ToString();
        }
        else
        {
            _myDeliveryFee.text = "Deliv. fee: " + Enemy.DeliveryFee.ToString();
        }
    }

    public void UpdateIngreCostUI(Store store)
    {
        if(store == Player)
        {
            _myIngreCost.text = "Ingre. cost: " + Player.IngredientCost.ToString();
        }
        else
        {
            _enemyIngreCost.text = "Ingre. cost: " + Enemy.IngredientCost.ToString();
        }
    }

    public void MakeStore(Tile at)
    {
        if (at.Decision != DecisionType.Player)
        {
            Debug.Log("Invalid position");
            return;
        }
        
        foreach (var tile in Tile.AllTiles)
        {
            tile.Type = TileType.Customer;
            tile.Decision = DecisionType.None;
        }
        at.Type = TileType.MyStore;
        Player.Position = at;

        var enemyTile = Tile.AllTiles.Find(t => t.Q == -at.Q && t.R == -at.R);
        enemyTile.Type = TileType.OpponentStore;
        Enemy.Position = enemyTile;
        
        // var r = new System.Random();
        // while (true)
        // {
        //     var rr = r.Next(tiles.Count);
        //     if (tiles[rr].Type != TileType.MyStore)
        //     {
        //         tiles[rr].Type = TileType.OpponentStore;
        //         Enemy.Position = tiles[rr];
        //         break;
        //     }
        // }

        isStorePositioned = true;

        // start first turn
        StartControlPhase();
    }

    private void StartControlPhase()
    {
        Player.ItemManager.Init();
        Enemy.ItemManager.Init();

        ItemManager.EnemyBuyItem(Enemy.ItemManager);
    }

    private void StartSimulationPhase()
    {
        Player.ItemManager.ApplyItem();
        Enemy.ItemManager.ApplyItem();

        Simulation.Simulate();
        AfterSimulation();
    }

    private void AfterSimulation()
    {
        if (Player.Money <= 0)
        {
            FinishGame(false);
            return;
        }

        if (Enemy.Money <= 0)
        {
            FinishGame(true);
            return;
        }

        EndWeek();
    }

    private void StartWeek()
    {
        Weeks++;
        
        /*
         * TODO:
         * Show enemy upgrade
         * Show week event (if exists)
         * Upgrade, item, pricing
         *
         * finally: simulation
         */
    }

    private void EndWeek()
    {
        // TODO: Show this week result panel

        if (Weeks == MaxWeeks)
        {
            FinishGame(Player.Money > Enemy.Money);
        }
    }

    private void FinishGame(bool isPlayerWin)
    {
        // TODO: Show game result panel
    }

    public Store FindMyEnemy(Store you) => you == Player ? Enemy : Player;

}
