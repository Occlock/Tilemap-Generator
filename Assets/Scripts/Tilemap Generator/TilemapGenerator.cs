using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    waterTile,
    grassTile,
    earthTile
}

public class TilemapGenerator : MonoBehaviour
{
    [Range(1, 100)]
    private int chance;

    private int waterMin;
    private int waterMax;
    //private int waterTileCount;

    private int grassMin;
    private int grassMax;
    //private int grassTileCount;

    private int earthMin;
    private int earthMax;
    //private int earthTileCount;

    private const int maxQuantityTerrain = 3;

    private int tilemapSizeX, tilemapSizeY;
    private int tilemapMaxTile;
    private int[] tileCounts;


    private Grid grid;
    private Tilemap[] tilemaps;


    [SerializeField]
    private Tile[] tileGround;

    //private BoundsInt boundsTest;

    private void Awake()
    {
        Initialize();

        GenerateCountTile();
        adjustCountTilesForMaxCapacity();

        GenerateTile();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void Initialize()
    {
        grid = GetComponent<Grid>();
        tilemaps = GetComponentsInChildren<Tilemap>();

        tilemapSizeX = 64;
        tilemapSizeY = 64;
        tilemapMaxTile = tilemapSizeX * tilemapSizeY;

        tileCounts = new int[maxQuantityTerrain];

        waterMin = 820;
        waterMax = 1640;
        grassMin = 820;
        grassMax = 1640;
        earthMin = 820;
        earthMax = 1640;

        SetTilemapSize();
        CentralizeXYTilemap();
    }

    private void SetTilemapSize()
    {
        tilemaps[0].size = new Vector3Int(tilemapSizeX, tilemapSizeY, 1);
    }
    private void CentralizeXYTilemap()
    {
        tilemaps[0].origin = new Vector3Int(tilemaps[0].size.x / -2, tilemaps[0].size.y / -2, 0);
    }


    private void GenerateCountTile()
    {
        tileCounts[(int)TileType.waterTile] = Random.Range(waterMin, waterMax);
        tileCounts[(int)TileType.grassTile] = Random.Range(grassMin, grassMax);
        tileCounts[(int)TileType.earthTile] = Random.Range(earthMin, earthMax);
    }

    private void adjustCountTilesForMaxCapacity()
    {
        int count = 0;

        while (GetMaxAllTileCount() > tilemapMaxTile)
        {
            DecreaseMaxTileCount();
        }

        while (GetMaxAllTileCount() < tilemapMaxTile)
        {
            tileCounts[count]++;

            count++;

            if (count >= tileCounts.Length)
                count = 0;
        }

    }

    private int GetMaxAllTileCount()
    {
        int maxTileCount = 0;

        for (int i = 0; i < tileCounts.Length; i++)
        {
            maxTileCount += tileCounts[i];
        }

        return maxTileCount;
    }

    private void DecreaseMaxTileCount()
    {
        int getCountMaxTile = Mathf.Max(tileCounts);

        for (int i = 0; i < tileCounts.Length; i++)
        {
            if (getCountMaxTile == tileCounts[i])
            {
                tileCounts[i] -= 100;
            }

        }
    }

    private void GenerateTile()
    {
        int cont = 0;
        int chance = 0;

        BoundsInt boundsTilemap = new BoundsInt(tilemaps[0].cellBounds.xMin, tilemaps[0].cellBounds.yMin, tilemaps[0].cellBounds.zMin, tilemaps[0].size.x, tilemaps[0].size.y, tilemaps[0].size.z);


        while ((tileCounts[(int)TileType.waterTile] > 0) || (tileCounts[(int)TileType.grassTile] > 0) || (tileCounts[(int)TileType.earthTile] > 0))
        {
            chance = Random.Range(0, 3);

            switch ((TileType)chance)
            {
                case TileType.waterTile:
                    {
                        foreach (Vector3Int point in boundsTilemap.allPositionsWithin)
                        {
                            if (tileCounts[(int)TileType.waterTile] == 0) break;

                            if (!tilemaps[0].HasTile(point))
                            {
                                tilemaps[0].SetTile(point, tileGround[(int)TileType.waterTile]);
                                tileCounts[(int)TileType.waterTile]--;

                                if (tileCounts[(int)TileType.waterTile] == Mathf.Max(tileCounts) && cont < 2)
                                {
                                    cont++;
                                    continue;
                                }
                                else
                                {
                                    cont = 0;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case TileType.grassTile:
                    {
                        foreach (Vector3Int point in boundsTilemap.allPositionsWithin)
                        {
                            if (tileCounts[(int)TileType.grassTile] == 0) break;

                            if (!tilemaps[0].HasTile(point))
                            {
                                tilemaps[0].SetTile(point, tileGround[(int)TileType.grassTile]);
                                tileCounts[(int)TileType.grassTile]--;

                                if (tileCounts[(int)TileType.grassTile] == Mathf.Max(tileCounts) && cont < 2)
                                {
                                    cont++;
                                    continue;
                                }
                                else
                                {
                                    cont = 0;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                case TileType.earthTile:
                    {
                        foreach (Vector3Int point in boundsTilemap.allPositionsWithin)
                        {
                            if (tileCounts[(int)TileType.earthTile] == 0) break;

                            if (!tilemaps[0].HasTile(point))
                            {
                                tilemaps[0].SetTile(point, tileGround[(int)TileType.earthTile]);
                                tileCounts[(int)TileType.earthTile]--;

                                if (tileCounts[(int)TileType.earthTile] == Mathf.Max(tileCounts) && cont < 2)
                                {
                                    cont++;
                                    continue;
                                }
                                else
                                {
                                    cont = 0;
                                    break;
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
