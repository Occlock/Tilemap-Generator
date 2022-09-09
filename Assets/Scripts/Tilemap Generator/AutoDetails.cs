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
            Names.allGrassDetails => Resources.Load<SO_Tiles>("ScriptableObjects/AllGrassDetails"),
            Names.baseGrassDetails => Resources.Load<SO_Tiles>("ScriptableObjects/BaseGrassDetails"),
            Names.midGrassDetails => Resources.Load<SO_Tiles>("ScriptableObjects/MidGrassDetails"),
            Names.highGrassDetails => Resources.Load<SO_Tiles>("ScriptableObjects/HighGrassDetails"),
            _ => Resources.Load<SO_Tiles>(""),
        };
    }


    public Tile TileRandomlySelected()
    {
        int prng = Random.Range(0,tiles.Length);

        return tiles[prng];
    }
}
