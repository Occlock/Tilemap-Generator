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
    private int waterRegionXSize;
    private int waterRegionYSize;
    //private int waterTileCount;

    private int grassMin;
    private int grassMax;
    private int grassRegionXSize;
    private int grassRegionYSize;
    //private int grassTileCount;

    private int earthMin;
    private int earthMax;
    private int earthRegionXSize;
    private int earthRegionYSize;
    //private int earthTileCount;

    private const int maxQuantityTerrain = 3;

    private int tilemapSizeX, tilemapSizeY;
    private int tilemapMaxTile;
    private int[] tileCounts;


    private Grid grid;
    private Tilemap[] tilemaps;


    [SerializeField]
    private Tile[] tileGround;

    private RegionTile waterRegion;
    private RegionTile grassRegion;
    private RegionTile earthRegion;

    private Dictionary<Vector3Int, BoundsInt> areaOfRegion;
    private List<Vector3Int> saveTiles;

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
        GenerateWaterCorrectly();
        //foreach (var t in saveTiles)
        //    Debug.Log("Pos: " + t);
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
        waterRegionXSize = 30;
        waterRegionYSize = 30;

        grassMin = 820;
        grassMax = 1640;
        grassRegionXSize = 48;
        grassRegionYSize = 48;

        earthMin = 820;
        earthMax = 1640;
        earthRegionXSize = 30;
        earthRegionYSize = 30;

        SetTilemapSize();
        CentralizeXYTilemap();

        waterRegion = new RegionTile(waterRegionXSize, waterRegionYSize, TileType.waterTile);
        grassRegion = new RegionTile(grassRegionXSize, grassRegionYSize, TileType.grassTile);
        earthRegion = new RegionTile(earthRegionXSize, earthRegionYSize, TileType.earthTile);

        areaOfRegion = new Dictionary<Vector3Int, BoundsInt>();
        saveTiles = new List<Vector3Int>();
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


    private void GenerateRegionZone(TileType tileType)
    {
        int xSize = 10;
        int ySize = 10;
        int x = Random.Range(tilemaps[0].cellBounds.xMin, tilemaps[0].cellBounds.xMax - xSize);
        int y = Random.Range(tilemaps[0].cellBounds.yMin, tilemaps[0].cellBounds.yMax - ySize);

        switch (tileType)
        {
            case TileType.waterTile:
                {
                    int waterRegionXFitted = waterRegionXSize;
                    int waterRegionYFitted = waterRegionYSize;

                    if (x + waterRegionXFitted > tilemaps[0].size.x)
                    {
                        int difference = (x + waterRegionXFitted) - tilemaps[0].size.x;
                        waterRegionXFitted -= difference;
                    }
                    if (y + waterRegionYFitted > tilemaps[0].size.y)
                    {
                        int difference = (y + waterRegionYFitted) - tilemaps[0].size.y;
                        waterRegionYFitted -= difference;
                    }

                    xSize = Random.Range(10, waterRegionXFitted);
                    ySize = Random.Range(10, waterRegionYFitted);
                }
                break;

            case TileType.grassTile:
                {
                    int grassRegionXFitted = grassRegionXSize;
                    int grassRegionYFitted = grassRegionYSize;

                    if (x + grassRegionXFitted > tilemaps[0].size.x)
                    {
                        int difference = (x + grassRegionXFitted) - tilemaps[0].size.x;
                        grassRegionXFitted -= difference;
                    }
                    if (y + grassRegionYFitted > tilemaps[0].size.y)
                    {
                        int difference = (y + grassRegionYFitted) - tilemaps[0].size.y;
                        grassRegionYFitted -= difference;
                    }

                    xSize = Random.Range(10, grassRegionXFitted);
                    ySize = Random.Range(10, grassRegionYFitted);
                }
                break;

            case TileType.earthTile:
                {
                    int earthRegionXFitted = earthRegionXSize;
                    int earthRegionYFitted = earthRegionYSize;

                    if (x + earthRegionXFitted > tilemaps[0].size.x)
                    {
                        int difference = (x + earthRegionXFitted) - tilemaps[0].size.x;
                        earthRegionXFitted -= difference;
                    }
                    if (y + earthRegionYFitted > tilemaps[0].size.y)
                    {
                        int difference = (y + earthRegionYFitted) - tilemaps[0].size.y;
                        earthRegionYFitted -= difference;
                    }

                    xSize = Random.Range(10, earthRegionXFitted);
                    ySize = Random.Range(10, earthRegionYFitted);
                }
                break;

            default:
                { }
                break;
        }

        if (areaOfRegion.Count > 0)
        {
            foreach (BoundsInt area in areaOfRegion.Values)
            {
                BoundsInt actualArea = new BoundsInt(x, y, 0, xSize, ySize, 1);

                foreach (Vector3Int point in actualArea.allPositionsWithin)
                {
                    if (area.Contains(point))
                    {
                        GenerateRegionZone(tileType);
                        return;
                    }
                }
            }
        }

        areaOfRegion.Add(new Vector3Int(x, y, 0), new BoundsInt(x, y, 0, xSize, ySize, 1));
    }

    private void GenerateWaterCorrectly()
    {
        while (areaOfRegion.Count < waterRegion.regions)
            GenerateRegionZone(TileType.waterTile);


        foreach (KeyValuePair<Vector3Int, BoundsInt> forGetPoints in areaOfRegion)
        {
            foreach (Vector3Int point in forGetPoints.Value.allPositionsWithin)
                saveTiles.Add(point);
        }


        foreach (KeyValuePair<Vector3Int, BoundsInt> forGetPoints in areaOfRegion)
        {
            int xCenter = (forGetPoints.Value.xMax + forGetPoints.Value.xMin) / 2;
            int yCenter = (forGetPoints.Value.yMax + forGetPoints.Value.yMin) / 2;

            int topDeepable = Random.Range(0, WaterRegionDeepable(forGetPoints.Value.size.x));
            int botDeepable = Random.Range(0, WaterRegionDeepable(forGetPoints.Value.size.x));
            int rightDeepable = Random.Range(0, WaterRegionDeepable(forGetPoints.Value.size.y));
            int leftDeepable = Random.Range(0, WaterRegionDeepable(forGetPoints.Value.size.y));

            for (int i = topDeepable; i > 0; i--)
            {
                if (forGetPoints.Value.yMax + i > tilemaps[0].cellBounds.yMax)
                    continue;


                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMin + i, forGetPoints.Value.yMax + i - 1, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMin + i, forGetPoints.Value.yMax + i - 1, 0));

                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMax - i - 1, forGetPoints.Value.yMax + i - 1, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMax - i - 1, forGetPoints.Value.yMax + i - 1, 0));

                for (int j = forGetPoints.Value.xMax - i - 1; j > forGetPoints.Value.xMin + i; j--)
                {
                    if (!saveTiles.Contains(new Vector3Int(j, forGetPoints.Value.yMax + i - 1, 0)))
                        saveTiles.Add((new Vector3Int(j, forGetPoints.Value.yMax + i - 1, 0)));
                }

            }

            for (int i = botDeepable; i > 0; i--)
            {
                if (forGetPoints.Value.yMin - i < tilemaps[0].cellBounds.yMin)
                    continue;


                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMin + i, forGetPoints.Value.yMin - i, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMin + i, forGetPoints.Value.yMin - i, 0));

                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMax - i - 1, forGetPoints.Value.yMin - i, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMax - i - 1, forGetPoints.Value.yMin - i, 0));

                for (int j = forGetPoints.Value.xMax - i - 1; j > forGetPoints.Value.xMin + i; j--)
                {
                    if (!saveTiles.Contains(new Vector3Int(j, forGetPoints.Value.yMin - i, 0)))
                        saveTiles.Add((new Vector3Int(j, forGetPoints.Value.yMin - i, 0)));
                }

            }

            for (int i = leftDeepable; i > 0; i--)
            {
                if (forGetPoints.Value.xMin - i < tilemaps[0].cellBounds.xMin)
                    continue;


                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMin - i, forGetPoints.Value.yMin + i, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMin - i, forGetPoints.Value.yMin + i, 0));

                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMin - i, forGetPoints.Value.yMax - i - 1, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMin - i, forGetPoints.Value.yMax - i - 1, 0));

                for (int j = forGetPoints.Value.yMax - i - 1; j > forGetPoints.Value.yMin + i; j--)
                {
                    if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMin - i, j, 0)))
                        saveTiles.Add((new Vector3Int(forGetPoints.Value.xMin - i, j, 0)));
                }

            }

            for (int i = rightDeepable; i > 0; i--)
            {
                if (forGetPoints.Value.xMax + i > tilemaps[0].cellBounds.xMax)
                    continue;


                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMax + i - 1, forGetPoints.Value.yMin + i, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMax + i - 1, forGetPoints.Value.yMin + i, 0));

                if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMax + i - 1, forGetPoints.Value.yMax - i - 1, 0)))
                    saveTiles.Add(new Vector3Int(forGetPoints.Value.xMax + i - 1, forGetPoints.Value.yMax - i - 1, 0));

                for (int j = forGetPoints.Value.yMax - i - 1; j > forGetPoints.Value.yMin + i; j--)
                {
                    if (!saveTiles.Contains(new Vector3Int(forGetPoints.Value.xMax + i - 1, j, 0)))
                        saveTiles.Add((new Vector3Int(forGetPoints.Value.xMax + i - 1, j, 0)));
                }

            }
        }



        for (int i = tilemaps[0].cellBounds.xMin; i < tilemaps[0].cellBounds.xMax; i++)
        {
            for (int j = tilemaps[0].cellBounds.yMin; j < tilemaps[0].cellBounds.yMax; j++)
            {
                    tilemaps[0].SetTile(new Vector3Int(i, j, 0), tileGround[(int)TileType.grassTile]);
                
            }
        }

        for (int i = tilemaps[0].cellBounds.xMin; i < tilemaps[0].cellBounds.xMax; i++)
        {
            for (int j = tilemaps[0].cellBounds.yMin; j < tilemaps[0].cellBounds.yMax; j++)
            {
                if (saveTiles.Contains(new Vector3Int(i, j, 0)))
                {
                    tilemaps[0].SetTile(new Vector3Int(i, j, 0), tileGround[(int)TileType.waterTile]);
                }
            }
        }
    }

    private int WaterRegionDeepable(int side)
    {
        int deepable = 0;
        int _side = side;

        while (_side > 1)
        {
            _side -= 2;
            deepable++;
        }

        return deepable;
    }
}
