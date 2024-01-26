using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    static public GameManager Instance;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Player = new Store();
        Enemy = new Store();

        _tilePrefab = Resources.Load<GameObject>("TileObject");

        _increasePrice.onClick.AddListener(() => Player.Price += 100);
        _decreasePrice.onClick.AddListener(() => Player.Price -= 100);
        _simulateButton.onClick.AddListener(DoSimulation);

        for (int r = -3; r <= 3; r++)
        {
            if (r < 0)
            {
                for (int q = -3 - r; q <= 3; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab);
                    x.GetComponent<Tile>().Init(q, r, TileType.Uninitialized);
                }
            }
            else
            {
                for (int q = -3; q <= 3 - r; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab);
                    x.GetComponent<Tile>().Init(q, r, TileType.Uninitialized);
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

        var r = new System.Random();
        while (true)
        {
            var rr = r.Next();
            if (tiles[rr].Type != TileType.MyStore)
            {
                tiles[rr].Type = TileType.OpponentStore;
                break;
            }
        }

        isStorePositioned = true;
    }

    private void DoSimulation()
    {
        Simulation.Simulate();
        AfterSimulation();
    }

    private void AfterSimulation()
    {
        
    }

}
