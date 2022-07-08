using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [Range(1, 100)]
    private int chance;

    private int waterMin;
    private int waterMax;
    private int waterTileCount;

    private int grassMin;
    private int grassMax;
    private int grassTileCount;

    private int earthMin;
    private int earthMax;
    private int earthTileCount;


    private int x, y;
    private int tilemapMaxTile;

    private Grid grid;
    private Tilemap[] tilemaps;


    [SerializeField]
    private Tile[] tileGround;



    private void Awake()
    {
        Initialize();
        GenerateCountTile();

        grid = GetComponent<Grid>();
        tilemaps = GetComponentsInChildren<Tilemap>();

        tilemaps[0].size = new Vector3Int(x, y, 1);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void Initialize()
    {
        x = 64;
        y = 64;
        tilemapMaxTile = x * y;

        waterMin = 820;
        waterMax = 1640;
        grassMin = 820;
        grassMax = 1640;
        earthMin = 820;
        earthMax = 1640;
    }

    private void GenerateCountTile()
    {
        waterTileCount = Random.Range(waterMin, waterMax);
        grassTileCount = Random.Range(grassMin, grassMax);
        earthTileCount = Random.Range(earthMin, earthMax);
    }
    
    private void adjustCountTilesForMaxCapacity()
    {
        if ((waterTileCount + grassTileCount + earthTileCount) > tilemapMaxTile)
        {
            int getMaxTerrainTile = Mathf.Max(waterTileCount, grassTileCount, earthTileCount);
        }
    }
}
