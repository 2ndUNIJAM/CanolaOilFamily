#define INVULNERABLE

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    private const string DecimalSpecifier = "#####0.0";
    
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
    [SerializeField]
    private Transform _tileParent;
    [SerializeField]
    private Button _upgradePanelOn;
    [SerializeField]
    private GameObject _upgradePanel;
    [SerializeField]
    private Button _shopPanelOn;
    [SerializeField]
    private ItemPanel _shopPanel;
    [SerializeField]
    private GameObject _eventNoticePanel;
    [SerializeField]
    private Image _eventImage;
    [SerializeField]
    private TMPro.TMP_Text _eventName;
    [SerializeField]
    private TMPro.TMP_Text _eventDescription;
    [SerializeField]
    private Image _topEventIcon;
    
    [Header("Weekly Result Panel")]
    [SerializeField]
    private GameObject weeklyResultPanel;
    [SerializeField]
    private TMPro.TMP_Text titleText;

    [Space(10)]
    [SerializeField]
    private TMPro.TMP_Text myPriceText;
    [SerializeField]
    private TMPro.TMP_Text enemyPriceText;
    [SerializeField]
    private TMPro.TMP_Text myVolumeText;
    [SerializeField]
    private TMPro.TMP_Text enemyVolumeText;
    [SerializeField]
    private TMPro.TMP_Text myRentText;
    [SerializeField]
    private TMPro.TMP_Text enemyRentText;
    [SerializeField]
    private TMPro.TMP_Text myProfitText;
    [SerializeField]
    private TMPro.TMP_Text enemyProfitText;
    [SerializeField]
    private TMPro.TMP_Text myMoneyText;
    [SerializeField]
    private TMPro.TMP_Text enemyMoneyText;

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

    public Store Player = new Store();
    public Store Enemy = new Store();
    
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
        _weeks = 0;

        _tilePrefab = Resources.Load<GameObject>("TileObject");
        _increasePrice.onClick.AddListener(() => Player.Price += 0.5m);
        _decreasePrice.onClick.AddListener(() => Player.Price -= 0.5m);
        _simulateButton.onClick.AddListener(StartSimulationPhase);
        _upgradePanelOn.onClick.AddListener(() => _upgradePanel.SetActive(true));
        _shopPanelOn.onClick.AddListener(() => _shopPanel.gameObject.SetActive(true));

        Event.Init();

        for (int r = -3; r <= 3; r++)
        {
            if (r < 0)
            {
                for (int q = -3 - r; q <= 3; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab, _tileParent).GetComponent<Tile>();
                    x.Init(q, r, TileType.Uninitialized);
                    Tile.AllTiles.Add(x);
                }
            }
            else
            {
                for (int q = -3; q <= 3 - r; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab, _tileParent).GetComponent<Tile>();
                    x.Init(q, r, TileType.Uninitialized);
                    Tile.AllTiles.Add(x);
                }
            }
        }
        
        // Set tiles initial validity
        // Decision of uninitialized tiles shows validity
        foreach (var tile in Tile.AllTiles)
        {
            if (tile.Q >= -1 && tile.S <= +1)
            {
                tile.Decision = DecisionType.Opponent;  // Invalid MyStore position
            }
            else
            {
                tile.Decision = DecisionType.Player;    // Valid MyStore position
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
            _enemyDeliveryFee.text = "Deliv. fee: " + Enemy.DeliveryFee.ToString();
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

        // Determine enemy store position
        Tile enemyTile;
        if (Random.Range(0, 2) == 0)
        {
            // Point symmetry
            enemyTile = Tile.AllTiles.Find(t => t.Q == -at.Q && t.R == -at.R);
            enemyTile.Type = TileType.OpponentStore;
            Enemy.Position = enemyTile;
        }
        else
        {
            // Line symmetry
            enemyTile = Tile.AllTiles.Find(t => t.Q == at.S && t.R == at.R);
            enemyTile.Type = TileType.OpponentStore;
            Enemy.Position = enemyTile;
        }

        isStorePositioned = true;

        // start first turn
        Player.InitValues();
        Enemy.InitValues();
        StartControlPhase();
    }

    public void StartControlPhase()
    {
        Weeks++;

        Event.ResetEvent();
        _topEventIcon.gameObject.SetActive(false);
        var eventInfo = Event.FireEvent(Weeks);

        _shopPanel.Refresh();

        if (eventInfo != null)
        {
            Debug.Log("event occured");
            _eventNoticePanel.SetActive(true);
            _eventImage.sprite = eventInfo.Value.spr;
            _eventName.text = eventInfo.Value.name;
            _eventDescription.text = eventInfo.Value.desc;
            _topEventIcon.gameObject.SetActive(true);
            _topEventIcon.sprite = eventInfo.Value.spr;
        }

        Player.ItemManager.Init();
        Enemy.ItemManager.Init();

        ItemManager.EnemyBuyItem(Enemy.ItemManager);
    }

    private void StartSimulationPhase()
    {
        if (!isStorePositioned)
        { return; }

        Player.ItemManager.ApplyItem();
        Enemy.ItemManager.ApplyItem();

        Simulation.Simulate();
        AfterSimulation();
    }

    private void AfterSimulation()
    {
#if !INVULNERABLE
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
#endif

        EndWeek();
    }
    

    private void EndWeek()
    {
        if (Weeks == MaxWeeks)
        {
            FinishGame(Player.Money > Enemy.Money);
            return;
        }
        
        weeklyResultPanel.SetActive(true);

        titleText.text = $"Week {Weeks} Result";
        
        // Sales
        myPriceText.text = Player.Price.ToString(DecimalSpecifier);
        enemyPriceText.text = Enemy.Price.ToString(DecimalSpecifier);

        // Volume
        myVolumeText.text = Player.SaleVolume.ToString();
        enemyVolumeText.text = Enemy.SaleVolume.ToString();

        // Rent
        myRentText.text = Player.Rent.ToString(DecimalSpecifier);
        enemyRentText.text = Enemy.Rent.ToString(DecimalSpecifier);

        // Profit
        myProfitText.text = Player.Profit.ToString(DecimalSpecifier);
        enemyProfitText.text = Enemy.Profit.ToString(DecimalSpecifier);

        // Money
        myMoneyText.text = Player.Money.ToString(DecimalSpecifier);
        enemyMoneyText.text = Enemy.Money.ToString(DecimalSpecifier);
    }

    private void FinishGame(bool isPlayerWin)
    {
        // TODO: Show game result panel
        // TODO: Back to title scene
    }

    public Store FindMyEnemy(Store you) => you == Player ? Enemy : Player;

}
