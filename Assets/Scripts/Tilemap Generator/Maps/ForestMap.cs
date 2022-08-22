using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForestMap : GenerateMap
{
    AutoBorder autoBorder;

    public override void Generate()
    {
        base.Generate();

        AutoBorderConfig();
        SetTileInUnderGround();
        SetTileGround();

        SetTileUnderGroundDetails();
        SetTileOverGroundDetails();

        tilemaps[(int)EnumTilemaps.Default].ClearAllTiles();
    }

    private void AutoBorderConfig()
    {
        autoBorder = ScriptableObject.CreateInstance<AutoBorder>();
        autoBorder.GetGenerateMapTilemap(tilemaps[(int)EnumTilemaps.Default]);
    }

    private void SetTileInUnderGround()
    {
        MargeEarthToWater();
        SetTileEarthUnderGround();
        SetTileOceanWaterUnderGround();
    }
    private void SetTileGround()
    {
        SetTileGroundBaseGrass();
        //SetTileGroundMidGrass();
        //SetTileGroundHighGrass();
    }

    private void SetTileGroundBaseGrass()
    {
        SetTileGrassGround();
        SetAutoBorder(Names.baseGrassName, RegionNameInTilePos);
    }
    private void SetTileGroundMidGrass()
    {
        #region Drawing midGrass in the map prototype yet
        ////GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone, 8, 8);
        ////FinalFormGroundMidGrass(ref generateSpecificZone);
        ////PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone);
        ////SetAutoBorder(Names.mediumGrassName, SubRegionNameInTilePos);
        //GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone, 30, 30);
        //GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone1, 15, 15);
        //GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone2, 8, 8);
        //GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone3, 4, 4);
        //FinalFormGroundMidGrass(ref generateSpecificZone);
        //FinalFormGroundMidGrass(ref generateSpecificZone1);
        //FinalFormGroundMidGrass(ref generateSpecificZone2);
        //FinalFormGroundMidGrass(ref generateSpecificZone3);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone1);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone2);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone3);
        //SetAutoBorder(Names.mediumGrassName, SubRegionNameInTilePos);
        #endregion Drawing midGrass in the map prototype yet
    }
    private void SetTileGroundHighGrass()
    {
        #region Drawing highGrass in the map prototype yet
        //GenerateGroundHighGrass(out GenerateSpecificZone generateSpecificZone, 15, 15);
        //GenerateGroundHighGrass(out GenerateSpecificZone generateSpecificZone1, 8, 8);
        //GenerateGroundHighGrass(out GenerateSpecificZone generateSpecificZone2, 4, 4);
        //FinalFormGroundHighGrass(ref generateSpecificZone);
        //FinalFormGroundHighGrass(ref generateSpecificZone1);
        //FinalFormGroundHighGrass(ref generateSpecificZone2);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone1);
        //PopulateSubRegionNameInTilePosInfo(ref generateSpecificZone2);
        //SetAutoBorder(Names.highGrassName, SubRegionNameInTilePos);
        ////SetTileGrassGround();
        ////SetAutoBorder(Names.baseGrassName);
        #endregion Drawing highGrass in the map prototype yet
    }

    private void SetTileUnderGroundDetails()
    {
        SetBigVariationsUnderGroundDetails(out List<Vector3Int[]> bigVariations);
        SetSmallVariationsUnderGroundDetailsAroundBigVariations(bigVariations);
        SetSmallVariationsUnderGroundDetails();
    }

    private void GenerateGroundMidGrass(out GenerateSpecificZone generateSpecificZone, int xZoneSize, int yZoneSize)
    {
        generateSpecificZone = new GenerateSpecificZone(tilemaps[(int)EnumTilemaps.GroundMidGrass], xZoneSize, yZoneSize, RegionNameInTilePos,
                                                        Names.baseGrassName, SubRegionNameInTilePos, Names.mediumGrassName);

        int space = ConstValues.spaceSecurityForGrassDepth;
        int mapHeightLimit = MapHeight - (yZoneSize + space);
        int mapWidthLimit = MapWidth - (xZoneSize + space);

        for (int y = space; y < mapHeightLimit; y++)
        {
            for (int x = space; x < mapWidthLimit; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                generateSpecificZone.CreateZoneWithMargeSpace(pos);
            }
        }
    }

    private void FinalFormGroundMidGrass(ref GenerateSpecificZone generateSpecificZone)
    {
        List<List<Vector3Int>> zones = generateSpecificZone.GetZones();
        List<List<Vector3Int>> finalZones = generateSpecificZone.ZoneFinalForm(zones);

        foreach (List<Vector3Int> zone in finalZones)
        {
            foreach (Vector3Int zonePos in zone)
            {
                tilemaps[(int)EnumTilemaps.GroundMidGrass].SetTile(zonePos, midGrassTile);
            }
        }

        string[,] sourceArray = generateSpecificZone.GetSubRegionsInTilePos();
        Helper.CopySignificativeValueBidimensionalArrayToAnother(sourceArray, SubRegionNameInTilePos);

    }

    private void GenerateGroundHighGrass(out GenerateSpecificZone generateSpecificZone, int xZoneSize, int yZoneSize)
    {
        generateSpecificZone = new GenerateSpecificZone(tilemaps[(int)EnumTilemaps.GroundHighGrass], xZoneSize, yZoneSize, SubRegionNameInTilePos,
                                                                Names.mediumGrassName, SubRegionNameInTilePos, Names.highGrassName);

        int space = ConstValues.spaceSecurityForGrassDepth;
        int mapHeightLimit = MapHeight - (yZoneSize + space);
        int mapWidthLimit = MapWidth - (xZoneSize + space);

        for (int y = space; y < mapHeightLimit; y++)
        {
            for (int x = space; x < mapWidthLimit; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                generateSpecificZone.CreateZoneWithMargeSpace(pos);
            }
        }
    }

    private void FinalFormGroundHighGrass(ref GenerateSpecificZone generateSpecificZone)
    {
        List<List<Vector3Int>> zones = generateSpecificZone.GetZones();
        List<List<Vector3Int>> finalZones = generateSpecificZone.ZoneFinalForm(zones);

        foreach (List<Vector3Int> zone in finalZones)
        {
            foreach (Vector3Int zonePos in zone)
            {
                tilemaps[(int)EnumTilemaps.GroundHighGrass].SetTile(zonePos, highGrassTile);
            }
        }

        string[,] sourceArray = generateSpecificZone.GetSubRegionsInTilePos();
        Helper.CopySignificativeValueBidimensionalArrayToAnother(sourceArray, SubRegionNameInTilePos);

    }

    private void PopulateSubRegionNameInTilePosInfo(ref GenerateSpecificZone generateSpecificZone)
    {
        string[,] sourceArray = generateSpecificZone.GetSubRegionsInTilePos();
        Helper.CopySignificativeValueBidimensionalArrayToAnother(sourceArray, SubRegionNameInTilePos);
    }



    private void MargeEarthToWater()
    {
        bool earthNearbyWater = false;

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (RegionNameInTilePos[x, y] == Names.earthName)
                {
                    SetOceanWaterTileFlow(x, y, ref earthNearbyWater);
                }
            }
        }

        if (earthNearbyWater)
            MargeEarthToWater();
    }

    private void SetOceanWaterTileFlow(int posX, int posY, ref bool setted)
    {
        Vector2Int tileCheckPos = new Vector2Int(posX, posY);
        Vector2Int[] neighbourPos = Helper.CheckHorizontalVerticalNeighbourTile(tileCheckPos);


        for (int i = 0; i < neighbourPos.Length; i++)
        {
            Vector2Int actualPos = new Vector2Int(neighbourPos[i].x, neighbourPos[i].y);

            if (RegionNameInTilePos[actualPos.x, actualPos.y] == Names.oceanWaterName)
            {
                tilemaps[(int)EnumTilemaps.UnderGround].SetTile(new Vector3Int(posX, posY, 0), oceanWaterTile);
                RegionNameInTilePos[posX, posY] = Names.oceanWaterName;

                setted = true;
            }
        }
    }

    private void SetTileEarthUnderGround()
    {
        AutoCompleteTile autoCompleteTile = new AutoCompleteTile(Names.earthName);

        Dictionary<Vector2Int, Tile> posAndTileDic = autoCompleteTile.AutoChooseTileInfo(tilemaps[(int)EnumTilemaps.UnderGround].size, RegionNameInTilePos);


        foreach (KeyValuePair<Vector2Int, Tile> posAndTile in posAndTileDic)
        {
            Vector3Int posXYZ = new Vector3Int(posAndTile.Key.x, posAndTile.Key.y, 0);

            tilemaps[(int)EnumTilemaps.UnderGround].SetTile(posXYZ, posAndTile.Value);
        }

    }


    private void SetBigVariationsUnderGroundDetails(out List<Vector3Int[]> bigVariations)
    {
        AutoDetails128x128 autoDetails128x128 = new AutoDetails128x128(Names.earthBigVariation);

        bigVariations = new List<Vector3Int[]>();

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (HasSpaceForBigVariation(pos))
                {
                    Vector3Int[] bigVariation = new Vector3Int[autoDetails128x128.GetTilesCount()];
                    int prng = Random.Range(0, 100);

                    if (prng < 10)
                    {
                        int xTiles = ConstValues.tile128x / ConstValues.worldTile;
                        int yTiles = ConstValues.tile128y / ConstValues.worldTile;
                        int actualTile = 0;

                        for (int j = yTiles; j > 0; j--)
                        {
                            for (int i = 0; i < xTiles; i++)
                            {
                                Vector3Int posXYZ = new Vector3Int(x + i, y + j, 0);
                                bigVariation[actualTile] = posXYZ;
                                tilemaps[(int)EnumTilemaps.UnderGroundDetails].SetTile(posXYZ, autoDetails128x128.GetTiles()[actualTile++]);
                            }
                        }

                        bigVariations.Add(bigVariation);
                    }
                }
            }
        }
    }


    private bool HasSpaceForBigVariation(Vector2Int pos)
    {
        int xTiles = ConstValues.tile128x / ConstValues.worldTile;
        int yTiles = ConstValues.tile128y / ConstValues.worldTile;

        for (int j = 0; j < yTiles; j++)
        {
            for (int i = 0; i < xTiles; i++)
            {
                if (RegionNameInTilePos[pos.x + i, pos.y + j] == Names.earthName)
                {
                    Vector3Int posXYZ = new Vector3Int(pos.x + i, pos.y + j, 0);

                    if (!tilemaps[(int)EnumTilemaps.UnderGroundDetails].HasTile(posXYZ))
                        continue;
                    else
                        return false;
                }
                else
                    return false;

            }

        }

        return true;
    }

    private void SetSmallVariationsUnderGroundDetailsAroundBigVariations(List<Vector3Int[]> bigVariations)
    {
        AutoDetails autoDetails = new AutoDetails(Names.earthSmallVariation);

        foreach (Vector3Int[] bigVariation in bigVariations)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = 0; k < bigVariation.Length; k++)
                    {
                        Vector3Int aroundBigVariation = new Vector3Int(bigVariation[k].x + i, bigVariation[k].y + j, 0);

                        if (!tilemaps[(int)EnumTilemaps.UnderGroundDetails].HasTile(aroundBigVariation))
                        {
                            int prng = Random.Range(0, 100);

                            if (prng < 50)
                            {
                                if (RegionNameInTilePos[aroundBigVariation.x, aroundBigVariation.y] == Names.earthName)
                                    tilemaps[(int)EnumTilemaps.UnderGroundDetails].SetTile(aroundBigVariation, autoDetails.TileRandomlySelected());
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetSmallVariationsUnderGroundDetails()
    {
        AutoDetails autoDetails = new AutoDetails(Names.earthSmallVariation);

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (RegionNameInTilePos[x, y] == Names.earthName)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    if (!tilemaps[(int)EnumTilemaps.UnderGroundDetails].HasTile(pos))
                    {
                        int prng = Random.Range(0, 100);

                        if (prng < 15)
                        {
                            tilemaps[(int)EnumTilemaps.UnderGroundDetails].SetTile(pos, autoDetails.TileRandomlySelected());
                        }
                    }
                }
            }
        }
    }

    private void SetTileOceanWaterUnderGround()
    {
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (RegionNameInTilePos[x, y] == Names.oceanWaterName)
                    tilemaps[(int)EnumTilemaps.UnderGround].SetTile(new Vector3Int(x, y, 0), oceanWaterTile);
            }
        }
    }

    private void SetTileGrassGround()
    {
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (RegionNameInTilePos[x, y] == Names.baseGrassName)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemaps[(int)EnumTilemaps.Ground].SetTile(pos, grassTile);
                }
            }
        }
    }

    private string GetBorderTiles(string regionName, string[,] region)
    {
        string regionBorderName = " ";

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                Vector2Int[] neighbourPos = Helper.CheckFullNeighbourTile(new Vector2Int(x, y));

                if (region[x, y] == regionName)
                {
                    for (int i = 0; i < neighbourPos.Length; i++)
                    {
                        regionBorderName = regionName + Names.border;

                        if (region[neighbourPos[i].x, neighbourPos[i].y] != regionName)
                            region[neighbourPos[i].x, neighbourPos[i].y] = regionBorderName;
                    }
                }
            }
        }

        return regionBorderName;
    }

    private void SetAutoBorder(string regionForAutoBorder, string[,] region)
    {
        int layerBase = int.MaxValue;

        string tileNameBorder = GetBorderTiles(regionForAutoBorder, region);


        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (region[x, y] == tileNameBorder)
                {
                    Vector3Int posXYZ = new Vector3Int(x, y, 0);
                    Dictionary<Vector2IntWithLayer, Tile> infosForSetAutoBorder = autoBorder.GetAutoBorderInfoForSetTile(posXYZ, tileNameBorder, region);


                    foreach (KeyValuePair<Vector2IntWithLayer, Tile> posAndTileInfo in infosForSetAutoBorder)
                    {
                        Vector3Int actualPosXYZ = new Vector3Int(posAndTileInfo.Key.x, posAndTileInfo.Key.y, 0);
                        EnumTilemaps tilemapLayer = TilemapLayer(tileNameBorder, posAndTileInfo.Key.layer, ref layerBase);

                        tilemaps[(int)tilemapLayer].SetTile(actualPosXYZ, posAndTileInfo.Value);
                    }
                }
            }
        }
    }

    private EnumTilemaps TilemapLayer(string borderName, int layer, ref int layerBase)
    {
        int layerOfBorder = layer;
        int layerLimit;

        layerBase = layerBase > layerOfBorder ? layerOfBorder : layerBase;
        layerLimit = layerBase + BorderLayerCount(borderName);

        EnumTilemaps tilemapLayer = layerOfBorder < layerLimit ? (EnumTilemaps)layerOfBorder : EnumTilemaps.Default;


        return tilemapLayer;
    }

    private int BorderLayerCount(string borderName)
    {
        return borderName switch
        {
            Names.baseGrassBorderName => ConstValues.grassLayerCount,
            Names.mediumGrassBorderName => ConstValues.mediumGrassLayerCount,
            Names.highGrassBorderName => ConstValues.highGrassLayerCount,
            _ => 0,
        };
    }

    private void SetTileOverGroundDetails()
    {
        GenerateSmallRocks();
    }

    private void GenerateSmallRocks()
    {
        AutoDetails autoDetails = new AutoDetails(Names.smallRocks);

        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                if (RegionNameInTilePos[x, y] == Names.baseGrassName)
                {
                    if ((SubRegionNameInTilePos[x, y] != Names.mediumGrassBorderName) && (SubRegionNameInTilePos[x, y] != Names.highGrassBorderName))
                    {
                        int prng = Random.Range(0, 100);
                        if (prng < 5)
                        {
                            Vector3Int pos = new Vector3Int(x, y, 0);
                            tilemaps[(int)EnumTilemaps.OverGroundDetails].SetTile(pos, autoDetails.TileRandomlySelected());
                        }
                    }
                }

                if (RegionNameInTilePos[x, y] == Names.earthName)
                {
                    int prng = Random.Range(0, 100);
                    if (prng < 10)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        tilemaps[(int)EnumTilemaps.OverGroundDetails].SetTile(pos, autoDetails.TileRandomlySelected());
                    }
                }
            }
        }
    }
}
