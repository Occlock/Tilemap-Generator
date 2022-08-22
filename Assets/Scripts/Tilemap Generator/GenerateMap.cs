using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TerrainType
{
    public string name;
    public Tile tile;
}

public class GenerateMap : Noise
{
    protected Tile oceanWaterTile;
    protected Tile grassTile;
    protected Tile midGrassTile;
    protected Tile highGrassTile;
    protected Tile earthTile;

    public TerrainType[] regions;

    [Header("Map Settings")]
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private int seed;
    [SerializeField] private float scale;
    [SerializeField] private int octaves;
    [SerializeField] private float lacunarity;

    [Header("Marge Settings")]
    [SerializeField] private int margeSeed;
    [SerializeField] private float margeScale;
    [SerializeField] private float margeLacunarity;
    [SerializeField] private int margeMaxRemove;

    protected Tilemap[] tilemaps;
    protected string[,] RegionNameInTilePos { get; set; }
    protected string[,] SubRegionNameInTilePos { get; set; }

    private void Init()
    {
        tilemaps = new Tilemap[GetComponentsInChildren<Tilemap>().Length];
        oceanWaterTile = Resources.Load<Tile>("Tilemap/Tiles/oceanWater");
        earthTile = Resources.Load<Tile>("Tilemap/Tiles/earth");
        grassTile = Resources.Load<Tile>("Tilemap/Tiles/grass/grass_8");
        midGrassTile = Resources.Load<Tile>("Tilemap/Tiles/grass/medium grass/grass_8");
        highGrassTile = Resources.Load<Tile>("Tilemap/Tiles/grass/high grass/grass_8");

        MapWidth = mapWidth;
        MapHeight = mapHeight;
        Seed = seed;
        Scale = scale;
        Octaves = octaves;
        Lacunarity = lacunarity;

        MargeSeed = margeSeed;
        MargeScale = margeScale;
        MargeLacunarity = margeLacunarity;
        MargeMaxRemove = margeMaxRemove;

        RegionNameInTilePos = new string[MapWidth, mapHeight];
        SubRegionNameInTilePos = new string[MapWidth, mapHeight];

        TilemapsConfig();
    }

    public virtual void Generate()
    {
        DevelopmantPhaseTest();

        float[,] noiseMap = GenerateNoiseMap();
        CheckSetTileMargin[,] checkedTile = CheckTile();


        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                float minTile = 0f;
                float maxTile = regions.Length;
                float perlinValue = noiseMap[x, y];

                int tileChoosed = Helper.Map01ToMinBMaxB(perlinValue, minTile, maxTile);

                if (checkedTile[x, y] == CheckSetTileMargin.CantSetTile)
                {
                    tilemaps[(int)EnumTilemaps.Default].SetTile(tilePos, oceanWaterTile);
                    RegionNameInTilePos[x, y] = Names.oceanWaterName;
                }
                else if (checkedTile[x, y] == CheckSetTileMargin.CanSetTile)
                {
                    RegionNameInTilePos[x, y] = regions[tileChoosed].name;
                    tilemaps[(int)EnumTilemaps.Default].SetTile(tilePos, regions[tileChoosed].tile);
                }
            }
        }
    }

    private CheckSetTileMargin[,] CheckTile()
    {
        CheckSetTileMargin[,] checkTile = new CheckSetTileMargin[MapWidth, MapHeight];

        int[] margeRemoveMap = TilesForRemove();

        int sideQuantity = 4;
        int sideSize = margeRemoveMap.Length / sideQuantity;

        for (int side = 0; side < sideQuantity; side++)
        {
            for (int margePos = 0; margePos < margeMaxRemove; margePos++)
            {
                for (int sidePos = 0; sidePos < sideSize; sidePos++)
                {
                    switch ((SideEnum)side)
                    {
                        case SideEnum.Left:
                            {
                                if (margeRemoveMap[sidePos + side * sideSize] > margePos)
                                    checkTile[margePos, sidePos] = CheckSetTileMargin.CantSetTile;
                            }
                            break;
                        case SideEnum.Up:
                            {
                                if (margeRemoveMap[sidePos + side * sideSize] > margePos)
                                    checkTile[sidePos, sideSize - 1 - margePos] = CheckSetTileMargin.CantSetTile;
                            }
                            break;
                        case SideEnum.Right:
                            {

                                if (margeRemoveMap[sidePos + side * sideSize] > margePos)
                                    checkTile[sideSize - 1 - margePos, sideSize - 1 - sidePos] = CheckSetTileMargin.CantSetTile;
                            }
                            break;
                        case SideEnum.Down:
                            {
                                if (margeRemoveMap[sidePos + side * sideSize] > margePos)
                                    checkTile[sideSize - 1 - sidePos, margePos] = CheckSetTileMargin.CantSetTile;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        TilesChecked(ref checkTile);

        return checkTile;
    }

    private int[] TilesForRemove()
    {
        float[] perlinValues = GenerateNoiseMargeMap();
        int[] tilesForRemove = new int[MapWidth + MapWidth + MapHeight + MapHeight];

        for (int i = 0; i < perlinValues.Length; i++)
        {
            tilesForRemove[i] = Helper.Map01ToMinBMaxB(perlinValues[i], 0, margeMaxRemove);
        }

        return tilesForRemove;
    }

    private void TilesChecked(ref CheckSetTileMargin[,] tilesCheck)
    {
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                if (tilesCheck[x, y] != CheckSetTileMargin.CantSetTile)
                    tilesCheck[x, y] = CheckSetTileMargin.CanSetTile;
            }
        }
    }

    //TODO add border in another layer for tiles isolated
    private List<Vector2Int> RemoveIsolatedTile()
    {
        List<Vector2Int> isolatedTilesPos = new List<Vector2Int>();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                Vector2Int[] neighbour = Helper.CheckFullNeighbourTile(pos);

                for (int i = 0; i < neighbour.Length; i++)
                {
                    if (!tilemaps[(int)EnumTilemaps.Default].HasTile(new Vector3Int(neighbour[i].x, neighbour[i].y, 0)))
                        continue;

                    if (RegionNameInTilePos[pos.x, pos.y] == RegionNameInTilePos[neighbour[i].x, neighbour[i].y])
                        break;

                    int lastPosNeighbour = neighbour.Length - 1;
                    if (i == lastPosNeighbour)
                        isolatedTilesPos.Add(pos);
                }
            }
        }

        return isolatedTilesPos;
    }

    private void TilemapsConfig()
    {
        GetTilemapsComponents();
        SetTilemapsSize();
    }
    private void GetTilemapsComponents()
    {
        tilemaps = GetComponentsInChildren<Tilemap>();
    }
    private void SetTilemapsSize()
    {
        foreach(Tilemap tilemap in tilemaps)
        {
            tilemap.size = new Vector3Int(MapWidth, MapHeight, 1);
        }
    }

    public void ClearTiles()
    {
        if (tilemaps == null)
            return;

        foreach(Tilemap tilemap in tilemaps)
        {
            if (tilemap != null)
                tilemap.ClearAllTiles();
        }
    }

    private void DevelopmantPhaseTest()
    {
        Init();
        ClearTiles();
    }
}
