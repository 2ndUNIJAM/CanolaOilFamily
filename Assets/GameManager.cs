using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // I AM SINGLETON!
{
    static public GameManager Instance;

    public Tile[][] grid;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
