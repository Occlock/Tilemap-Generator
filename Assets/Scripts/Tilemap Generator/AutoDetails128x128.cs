using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDetails128x128 : AutoDetails
{
    public AutoDetails128x128(string detailName)
    {
        tileSO = ChooseDetail(detailName);
        tiles = GetTilesInSO(tileSO);
    }

    protected override SO_Tiles ChooseDetail(string detailName)
    {
        return detailName switch
        {
            Names.earthBigVariation => Resources.Load<SO_Tiles>("ScriptableObjects/BigVariation"),
            _ => Resources.Load<SO_Tiles>(""),
        };
    }

    public int GetTilesCount()
    {
        return tiles.Length;
    }

    public UnityEngine.Tilemaps.Tile[] GetTiles()
    {
        return tiles;
    }
}
