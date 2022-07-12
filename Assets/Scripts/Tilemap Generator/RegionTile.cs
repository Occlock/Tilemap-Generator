using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionTile
{
    private int _regions;
    public int regions { get => _regions; set => _regions = value; }

    private int _xSizeRegion;
    public int xSizeRegion { get => _xSizeRegion; set => _xSizeRegion = value; }

    private int _ySizeRegion;
    public int ySizeRegion { get => _ySizeRegion; set => _ySizeRegion = value; }

    private bool[] _linker;
    public bool[] linker { get => _linker; set => _linker = value; }

    private Dictionary<int, Dictionary<Vector3Int, BoundsInt>> _regionZone;
    public Dictionary<int, Dictionary<Vector3Int, BoundsInt>> RegionZone { get => _regionZone; set => _regionZone = value; }

    public RegionTile(int xSize, int ySize, TileType tileType)
    {
        xSizeRegion = xSize;
        ySizeRegion = ySize;

        RegionsPerTerrain(tileType);

        linker = new bool[regions];

        Randomizelinker();

        RegionZone = new Dictionary<int, Dictionary<Vector3Int, BoundsInt>>();
    }

    public RegionTile() 
    {
        
    }

    private void Randomizelinker()
    {
        int chance;

        if (linker.Length > 0)
        {
            for (int i = 0; i < linker.Length; i++)
            {
                chance = Random.Range(0, 2);

                if (chance == 0)
                    linker[i] = false;
                else
                    linker[i] = true;
            }
        }
    }

    private void RegionsPerTerrain(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.waterTile:
                {
                    regions = Random.Range(1, 4);
                }
                break;

            case TileType.grassTile:
                {
                    regions = Random.Range(1, 10);
                }
                break;

            case TileType.earthTile:
                {
                    regions = Random.Range(1, 10);
                }
                break;

            default:
                { }
                break;
        }
    }

    public void SetRegionZone(Vector3Int pos, BoundsInt bounds)
    {
        Dictionary<Vector3Int, BoundsInt> areaOfRegion = new Dictionary<Vector3Int, BoundsInt>();
        areaOfRegion.Add(pos, bounds);

        RegionZone.Add(regions, areaOfRegion);
    }

}
