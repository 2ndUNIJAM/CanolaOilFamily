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

    private float _price = 20000;
    public float Price
    {
        get
        {
            return _price;
        }
        set
        {
            if (value < 10000 || value > 30000) { return; }

            _price = value;
            _priceText.text = value.ToString();
        }
    }

    public float Money;
    public int Weeks; // 현재 주차

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _increasePrice.onClick.AddListener(() => Price += 100);
        _decreasePrice.onClick.AddListener(() => Price -= 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
