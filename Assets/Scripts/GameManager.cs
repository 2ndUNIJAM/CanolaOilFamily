using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Button _simulateButton;
    [SerializeField]
    private TMPro.TMP_Text _priceText;
    [SerializeField]
    private Button _increasePrice;
    [SerializeField]
    private Button _decreasePrice;

    [SerializeField]
    private GameObject _tilePrefab;

    public Store Player;
    public Store Enemy;

    public int Weeks; // current weeks
    public bool isStorePositioned = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePriceUI()
    {
        _priceText.text = Player.Price.ToString();
    }

    public void MakeStore(Tile at)
    {
        var tiles = Tile.AllTiles;
        tiles.ForEach(x => x.Type = TileType.Customer);
        at.Type = TileType.MyStore;
        Player.Position = at;

        var r = new System.Random();
        while (true)
        {
            var rr = r.Next(tiles.Count);
            if (tiles[rr].Type != TileType.MyStore)
            {
                tiles[rr].Type = TileType.OpponentStore;
                Enemy.Position = tiles[rr];
                break;
            }
        }

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
        
    }

    public Store FindMyEnemy(Store you) => you == Player ? Enemy : Player;

}
