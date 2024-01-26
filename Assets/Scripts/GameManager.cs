using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    static public GameManager Instance;

    public Tile[][] grid;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Player = new Store();
        Enemy = new Store();

        _tilePrefab = Resources.Load<GameObject>("TileObject");

        _increasePrice.onClick.AddListener(() => Player.Price += 100);
        _decreasePrice.onClick.AddListener(() => Player.Price -= 100);

        for (int r = -3; r <= 3; r++)
        {
            if (r < 0)
            {
                for (int q = -3 - r; q <= 3; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab);
                    x.GetComponent<Tile>().Init(q, r, TileType.Customer);
                }
            }
            else
            {
                for (int q = -3; q <= 3 - r; q++)
                {
                    var x = Instantiate<GameObject>(_tilePrefab);
                    x.GetComponent<Tile>().Init(q, r, TileType.Customer);
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

    private void AfterSimulation()
    {
        //CurrentEvent = Event.MakeRandomEvent();
    }

}
