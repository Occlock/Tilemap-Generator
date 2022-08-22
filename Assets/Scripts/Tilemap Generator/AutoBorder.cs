using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoBorder : Tile
{
    private SO_Tiles tilesSO;
    private Tilemap tilemap;

    //private string[,] regionInfo;

    public Dictionary<Vector2IntWithLayer, Tile> GetAutoBorderInfoForSetTile(Vector3Int posXYZ, string tileNameWithBorder, string[,] regionInfo)
    {
        Init(TileNameWithoutBorder(tileNameWithBorder));

        Dictionary<Vector2IntWithLayer, Tile> bordersInPos = new Dictionary<Vector2IntWithLayer, Tile>();
        Vector2IntWithLayer posWithLayer = new Vector2IntWithLayer(posXYZ.x, posXYZ.y, GetBaseLayerByRegionName(tileNameWithBorder));

        bool[] neighbour = CheckTileRegionInNeighbour(posXYZ, tileNameWithBorder, regionInfo);

        if (RegionTypeForBorder(tileNameWithBorder) == Names.overLap)
        {
            #region Conditions for check if need border
            if (regionInfo[posWithLayer.x, posWithLayer.y] == tileNameWithBorder)
            {
                if ((neighbour[(int)FullDirections.Right]) && (neighbour[(int)FullDirections.Bottom]))
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.RightAndBottom]);
                    posWithLayer.layer++;

                }
                if ((neighbour[(int)FullDirections.Right]) && (neighbour[(int)FullDirections.Top]))
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.RightAndTop]);
                    posWithLayer.layer++;

                }
                if ((neighbour[(int)FullDirections.Left]) && (neighbour[(int)FullDirections.Bottom]))
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.LeftAndBottom]);
                    posWithLayer.layer++;

                }
                if ((neighbour[(int)FullDirections.Left]) && (neighbour[(int)FullDirections.Top]))
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.LeftAndTop]);
                    posWithLayer.layer++;

                }


                if (neighbour[(int)FullDirections.Right])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.Right]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.Left])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.Left]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.Bottom])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.Bottom]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.Top])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.Top]);
                    posWithLayer.layer++;

                }


                if (neighbour[(int)FullDirections.RightBottom])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.DiagonalRightBottom]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.LeftBottom])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.DiagonalLeftBottom]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.LeftTop])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.DiagonalLeftTop]);
                    posWithLayer.layer++;

                }
                if (neighbour[(int)FullDirections.RightTop])
                {
                    bordersInPos.Add(posWithLayer, tilesSO.tiles[(int)EnumTiles.DiagonalRightTop]);

                }

            }
            #endregion Conditions for check if need border
        }

        return bordersInPos;
    }

    private void Init(string tileName)
    {
        tilesSO = ChooseTiles(tileName);
    }


    public bool[] CheckTileRegionInNeighbour(Vector3Int posXYZ, string tileName, string[,] regionInfo)
    {
        tileName = TileNameWithoutBorder(tileName);

        Vector2Int pos = new Vector2Int(posXYZ.x, posXYZ.y);
        Vector2Int[] neighbour = Helper.CheckFullNeighbourTile(pos);

        bool[] hasTileRegionInNeighbour = new bool[neighbour.Length];

        for (int i = 0; i < neighbour.Length; i++)
        {
            Vector3Int neighbourPosXYZ = new Vector3Int(neighbour[i].x, neighbour[i].y, 0);

            if (!tilemap.HasTile(neighbourPosXYZ))
                continue;

            string nameRegionInTilePos = regionInfo[neighbour[i].x, neighbour[i].y];

            if (nameRegionInTilePos == tileName)
                hasTileRegionInNeighbour[i] = true;

        }

        return hasTileRegionInNeighbour;
    }

    private string TileNameWithoutBorder(string tileNameWithBorder)
    {
        int indexForRemoveBorder = tileNameWithBorder.IndexOf(" ");
        int charactersForRemove = tileNameWithBorder.Length - indexForRemoveBorder;
        string tileNameWithoutBorder = tileNameWithBorder.Remove(indexForRemoveBorder, charactersForRemove);

        return tileNameWithoutBorder;
    }

    private string RegionTypeForBorder(string regionNameWithBorder)
    {
        string overLap = Names.overLap;
        string nonOverLap = Names.nonOverLap;

        foreach (string region in OverLapRegions())
        {
            if (TileNameWithoutBorder(regionNameWithBorder) == region)
                return overLap;
        }

        return nonOverLap;
    }

    private List<string> OverLapRegions()
    {
        List<string> overLapRegions = new List<string>();

        overLapRegions.Add(Names.baseGrassName);
        overLapRegions.Add(Names.mediumGrassName);
        overLapRegions.Add(Names.highGrassName);

        return overLapRegions;
    }

    private int GetBaseLayerByRegionName(string regionNameWithBorder)
    {
        return regionNameWithBorder switch
        {
            Names.baseGrassBorderName => ConstValues.grassBaseLayer,
            Names.mediumGrassBorderName => ConstValues.mediumGrassLayer,
            Names.highGrassBorderName => ConstValues.highGrassLayer,
            _ => (int)EnumTilemaps.Default,
        };
    }

    //public void GetGenerateMapRegionsName(string[,] regionsName)
    //{
    //    this.regionInfo = regionsName;
    //}

    public void GetGenerateMapTilemap(Tilemap tilemap)
    {
        this.tilemap = tilemap;
    }

    private SO_Tiles ChooseTiles(string tileSO)
    {
        return tileSO switch
        {
            Names.baseGrassName => Resources.Load<SO_Tiles>("ScriptableObjects/GrassTiles"),
            Names.mediumGrassName => Resources.Load<SO_Tiles>("ScriptableObjects/MediumGrassTiles"),
            Names.highGrassName => Resources.Load<SO_Tiles>("ScriptableObjects/HighGrassTiles"),
            _ => Resources.Load<SO_Tiles>(""),
        };
    }
}
