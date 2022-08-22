using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoDetails 
{
    protected SO_Tiles tileSO;
    protected Tile[] tiles;

    public AutoDetails() { }
    public AutoDetails(string detailName)
    {
        tileSO = ChooseDetail(detailName);
        tiles = GetTilesInSO(tileSO);
    }


    protected virtual Tile[] GetTilesInSO(SO_Tiles tilesSO)
    {
        return tilesSO.tiles;
    }

    protected virtual SO_Tiles ChooseDetail(string detailName)
    {
        return detailName switch
        {
            Names.earthSmallVariation => Resources.Load<SO_Tiles>("ScriptableObjects/SmallVariations"),
            Names.smallRocks => Resources.Load<SO_Tiles>("ScriptableObjects/SmallRocks"),
            _ => Resources.Load<SO_Tiles>(""),
        };
    }


    public Tile TileRandomlySelected()
    {
        int prng = Random.Range(0,tiles.Length);

        return tiles[prng];
    }
}
