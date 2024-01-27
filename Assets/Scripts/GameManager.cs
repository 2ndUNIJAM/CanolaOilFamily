using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    private readonly int AnimShowHash = Animator.StringToHash("Show");
    private readonly int AnimHideHash = Animator.StringToHash("Hide");
    
    private const float SimulationMotionInterval = 0.03f;
    private const string PriceSpecifier = "$######0.0";
    
    private Coroutine _simulationCoroutine;
    
    public const int MaxWeeks = 52;
    
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject makeStoreInfoText;
    [SerializeField]
    private Button _simulateButton;
    [SerializeField]
    private TMP_Text _priceText;
    [SerializeField]
    private Button _increasePrice;
    [SerializeField]
    private Button _decreasePrice;
    
    [Space(10)]
    [SerializeField]
    private TMP_Text _myMoney;
    [SerializeField]
    private TMP_Text _enemyMoney;
    [SerializeField]
    private TMP_Text _myStock;
    [SerializeField]
    private TMP_Text _enemyStock;
    [SerializeField]
    private TMP_Text _myIngreCost;
    [SerializeField]
    private TMP_Text _enemyIngreCost;
    [SerializeField]
    private TMP_Text _myRent;
    [SerializeField]
    private TMP_Text _enemyRent;
    [SerializeField]
    private TMP_Text _myDeliveryFee;
    [SerializeField]
    private TMP_Text _enemyDeliveryFee;
    [SerializeField]
    
    [Space(10)]
    private TMP_Text _weekText;
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
    private TMP_Text _eventName;
    [SerializeField]
    private TMP_Text _eventDescription;
    [SerializeField]
    private Image _topEventIcon;
    [SerializeField]
    private TileTooltip _tileTooltip;
    
    [Header("Weekly Result Panel")]
    [SerializeField]
    private GameObject weeklyResultPanel;
    [SerializeField]
    private Animator weeklyResultPanelAnim;
    [SerializeField]
    private TMP_Text titleText;

    [Space(10)]
    [SerializeField]
    private TMP_Text myPriceText;
    [SerializeField]
    private TMP_Text enemyPriceText;
    [SerializeField]
    private TMP_Text myVolumeText;
    [SerializeField]
    private TMP_Text enemyVolumeText;
    [SerializeField]
    private TMP_Text myRentText;
    [SerializeField]
    private TMP_Text enemyRentText;
    [SerializeField]
    private TMP_Text myProfitText;
    [SerializeField]
    private TMP_Text enemyProfitText;
    [SerializeField]
    private TMP_Text myMoneyText;
    [SerializeField]
    private TMP_Text enemyMoneyText;
    [SerializeField]
    private TMP_Text _enemyActionSummary;

    [Header("Game Result Panel")]
    [SerializeField]
    private GameObject gameResult;
    [SerializeField]
    private Image resultBackgroundImage;
    [SerializeField]
    private TMP_Text resultWeekText;
    [SerializeField]
    private TMP_Text resultMyMoneyText;


    [Header("Sprites")]
    [SerializeField] private Sprite victorySprite;
    [SerializeField] private Sprite defeatSprite;
    
    [Space(10)]
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
        get => _weeks;
        private set
        {
            _weeks = value;
            _weekText.text = _weeks.ToString();
        }
    }

    private GameObject _tilePrefab;

    public bool IsStorePositioned = false;

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
        _upgradePanelOn.onClick.AddListener(() =>
        {
            _upgradePanel.GetComponent<UpgradeManager>().UpdateUi();
            _upgradePanel.SetActive(true);
        });
        _shopPanelOn.onClick.AddListener(() => _shopPanel.gameObject.SetActive(true));

        Tile.Ttt = _tileTooltip;

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
        
        // Random select Special tiles
        Tile.ShuffleTileList();
        
        Tile.AllTiles[0].SpecialType = SpecialTileType.HighOrder;
        Tile.AllTiles[1].SpecialType = SpecialTileType.LowOrder;
        Tile.AllTiles[2].SpecialType = SpecialTileType.OccasionalHighOrder;
        Tile.AllTiles[3].SpecialType = SpecialTileType.RandomOrder;
        
        Debug.Log($"HighOrder: {Tile.AllTiles[0].Q}, {Tile.AllTiles[0].R}");
        Debug.Log($"LowOrder: {Tile.AllTiles[1].Q}, {Tile.AllTiles[1].R}");
        Debug.Log($"OccasionalHighOrder: {Tile.AllTiles[2].Q}, {Tile.AllTiles[2].R}");
        Debug.Log($"RandomOrder: {Tile.AllTiles[3].Q}, {Tile.AllTiles[3].R}");
        
        // Set tiles initial validity
        // Decision of uninitialized tiles shows validity
        foreach (var tile in Tile.AllTiles)
        {
            if ((tile.Q >= -1 && tile.S <= +1) || tile.SpecialType != SpecialTileType.None)
            {
                tile.Decision = DecisionType.Opponent;  // Invalid MyStore position
            }
            else
            {
                tile.Decision = DecisionType.Player;    // Valid MyStore position
            }
            tile.UpdateSprite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePriceUI()
    {
        _priceText.text = Player.Price.ToString("$#0.0");
    }

    public void UpdateMoneyUI(Store store)
    {
        if (store == Player)
        {
            _myMoney.text = "소지금 | " + Player.Money.ToString(PriceSpecifier);
        }
        else
        {
            _enemyMoney.text = "소지금 | " + Enemy.Money.ToString(PriceSpecifier);
        }
    }

    public void UpdateUpgradableStatUI()
    {
        _myStock.text = "재고량 | " + Player.Stock.ToString();
        _enemyStock.text = "재고량 | " + Enemy.Stock.ToString();
        _myIngreCost.text
            = "재료비 | " 
            +   (Player.IngredientCost 
                - Player.Upgrade.IngredientCostDecrement
                + (Player.ItemManager.isIngredientCostSabotaged ? 1 : 0 )).ToString(PriceSpecifier);
        _enemyIngreCost.text
            = "재료비 | "
            +   (Enemy.IngredientCost
                - Enemy.Upgrade.IngredientCostDecrement
                + (Enemy.ItemManager.isIngredientCostSabotaged ? 1 : 0)).ToString(PriceSpecifier);
        _myRent.text
            = "임대료 | " + (Player.Rent - Player.Upgrade.RentCostDecrement).ToString(PriceSpecifier);
        _enemyRent.text
            = "임대료 | " + (Enemy.Rent - Enemy.Upgrade.RentCostDecrement).ToString(PriceSpecifier);
        _myDeliveryFee.text
            = "배달비 | "
            + (Player.DeliveryFee - Player.Upgrade.DeliveryCostDecrement + Event.DeliveryFeeBias).ToString(PriceSpecifier);
        _enemyDeliveryFee.text
            = "배달비 | "
            + (Enemy.DeliveryFee - Enemy.Upgrade.DeliveryCostDecrement + Event.DeliveryFeeBias).ToString(PriceSpecifier);
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
            tile.UpdateSprite();
        }
        at.Type = TileType.MyStore;
        at.UpdateSprite();
        Player.Position = at;
        Tile.ShuffleTileList();

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
        enemyTile.UpdateSprite();
        
        // If enemy store position is special tile, randomly select the special tile
        if (enemyTile.SpecialType != SpecialTileType.None)
        {
            foreach (var tile in Tile.AllTiles)
            {
                if (tile.SpecialType == SpecialTileType.None)
                {
                    tile.SpecialType = enemyTile.SpecialType;
                    enemyTile.SpecialType = SpecialTileType.None;
                    tile.UpdateSprite();
                    enemyTile.UpdateSprite();
                    break;
                }
            }
        }

        IsStorePositioned = true;

        makeStoreInfoText.SetActive(false);
        readyPanel.SetActive(true);
        _shopPanelOn.gameObject.SetActive(true);
        _upgradePanelOn.gameObject.SetActive(true);
        
        // start first turn
        Player.InitValues();
        Enemy.InitValues();
        StartControlPhase();
    }

    public void StartControlPhase()
    {
        Weeks += 1;
        
        weeklyResultPanelAnim.SetTrigger(AnimHideHash);

        _simulateButton.interactable = true;
        
        Event.ResetEvent();
        _topEventIcon.gameObject.SetActive(false);
        var eventInfo = Event.FireEvent(Weeks);
        UpdateUpgradableStatUI();

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
        if (!IsStorePositioned)
        { return; }

        _simulateButton.interactable = false;
        Debug.Log("StartSimulationPhase");

        Player.ItemManager.ApplyItem();
        Enemy.ItemManager.ApplyItem();

        Simulation.Simulate();
        _simulationCoroutine = StartCoroutine(SimulationMotionCoroutine());
    }

    private void AfterSimulation()
    {
        // check if dead
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

        // theif check
        decimal myGinpai = 0, marineGinpai = 0;
        
        if (Player.ItemManager.IsThief && !Player.ItemManager.DoBlocked)
        {
            myGinpai = Enemy.Profit / 2;
        }

        if (Enemy.ItemManager.IsThief && !Enemy.ItemManager.DoBlocked)
        {
            marineGinpai = Player.Profit / 2;
        }

        Player.Profit += (myGinpai - marineGinpai);
        Player.Money += (myGinpai - marineGinpai);
        Enemy.Profit += (marineGinpai - myGinpai);
        Enemy.Money += (marineGinpai - myGinpai);

        // enemy consider upgrade
        System.Random r = new();
        List<List<Upgrade>> upgll = new()
        {
            UpgradeManager._delivery, UpgradeManager._ingredient, UpgradeManager._rent, UpgradeManager._store
        };

        var count = 0;
        Upgrade next;
        while (true)
        {
            next = UpgradeManager.GetCurrentUpgrade(upgll[r.Next(upgll.Count)], Enemy);

            if (count++ > 10)
                break;

            if (Enemy.HasUpgrade(next))
                continue;

            if (next.Price < Enemy.Money)
            {
                if (!Enemy.BuyUpgrade(next))
                    continue;
                else
                {
                    EndWeek(next);
                    return;
                }
            }
        }
        EndWeek(null);
    }
    

    private void EndWeek(Upgrade enemyDidThis)
    {
        if (Weeks == MaxWeeks)
        {
            FinishGame(Player.Money > Enemy.Money);
            return;
        }
        
        titleText.text = Weeks.ToString();
        
        // Sales
        myPriceText.text = Player.Price.ToString(PriceSpecifier);
        enemyPriceText.text = Enemy.Price.ToString(PriceSpecifier);

        // Volume
        myVolumeText.text = Player.SaleVolume.ToString();
        enemyVolumeText.text = Enemy.SaleVolume.ToString();

        // Rent
        myRentText.text = Player.Rent.ToString(PriceSpecifier);
        enemyRentText.text = Enemy.Rent.ToString(PriceSpecifier);

        // Profit
        myProfitText.text = Player.Profit.ToString(PriceSpecifier);
        enemyProfitText.text = Enemy.Profit.ToString(PriceSpecifier);

        // Money
        myMoneyText.text = Player.Money.ToString(PriceSpecifier);
        enemyMoneyText.text = Enemy.Money.ToString(PriceSpecifier);

        // Summary
        _enemyActionSummary.text = string.Empty;

        if (Player.ItemManager.IsThief)
        {
            _enemyActionSummary.text += Player.ItemManager.DoBlocked ?
                "상대가 습격을 방어했습니다." : "상대 가게를 습격했습니다.";
            _enemyActionSummary.text += '\n';
        }
        if (Enemy.ItemManager.IsThief)
        {
            _enemyActionSummary.text += Enemy.ItemManager.DoBlocked ?
                "상대의 습격을 방어했습니다." : "상대가 가게를 습격했습니다.";
            _enemyActionSummary.text += '\n';
        }
        //todo upgrade notify
        
        weeklyResultPanel.SetActive(true);
        weeklyResultPanelAnim.SetTrigger(AnimShowHash);
        
        if (enemyDidThis != null)
        {
            _enemyActionSummary.text += "상대가 가게를 강화했습니다. ";
            _enemyActionSummary.text += enemyDidThis.Title;
        }
    }

    private void FinishGame(bool isPlayerWin)
    {
        resultBackgroundImage.sprite = isPlayerWin ? victorySprite : defeatSprite;
        
        resultWeekText.text = Weeks.ToString();
        resultMyMoneyText.text = Player.Money.ToString(PriceSpecifier);
        
        gameResult.SetActive(true);
    }

    private IEnumerator SimulationMotionCoroutine()
    {
        foreach (var tile in Tile.AllTiles)
        {
            if (tile.Type != TileType.Customer) continue;
            tile.UpdateSprite();
            yield return new WaitForSeconds(SimulationMotionInterval);
        }

        AfterSimulation();
    }

    public Store FindMyEnemy(Store you) => you == Player ? Enemy : Player;

    public void OnClickGoToTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
