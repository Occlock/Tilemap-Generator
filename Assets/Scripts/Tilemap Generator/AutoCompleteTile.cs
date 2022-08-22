using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoCompleteTile
{
    private SO_Tiles tilesSO;
    private Tile[] tiles;

    private Dictionary<Vector2Int, Tile> tileInPosDic = new Dictionary<Vector2Int, Tile>();

    string regionTarget;

    public AutoCompleteTile(string regionTarget)
    {
        this.regionTarget = regionTarget;

        tilesSO = LoadTileSO(regionTarget);
        tiles = GetTilesInSO(tilesSO);
    }

    private SO_Tiles LoadTileSO(string regionTarget)
    {
        return regionTarget switch
        {
            Names.earthName => Resources.Load<SO_Tiles>("ScriptableObjects/EarthTiles"),

            _ => Resources.Load<SO_Tiles>(""),
        };
    }
    private Tile[] GetTilesInSO(SO_Tiles tilesSO)
    {
        return tilesSO.tiles;
    }


    public Dictionary<Vector2Int, Tile> AutoChooseTileInfo(Vector3Int size, string[,] regionNameInTilePos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (regionNameInTilePos[x, y] == regionTarget)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Vector2Int[] neighbour = Helper.CheckFullNeighbourTile(pos);

                    if (tileInPosDic.ContainsKey(pos))
                        continue;

                    #region Conditions for join tiles correctly


                    if (regionTarget == regionNameInTilePos[neighbour[(int)FullDirections.Bottom].x, neighbour[(int)FullDirections.Bottom].y])
                    {
                        if (tileInPosDic.TryGetValue(neighbour[(int)FullDirections.Bottom], out Tile tileNeighbour))
                        {
                            if (tileNeighbour == tiles[0])
                            {
                                tileInPosDic.Add(pos, tiles[2]);
                                continue;
                            }
                            if (tileNeighbour == tiles[1])
                            {
                                tileInPosDic.Add(pos, tiles[3]);
                                continue;
                            }
                            if (tileNeighbour == tiles[2])
                            {
                                tileInPosDic.Add(pos, tiles[0]);
                                continue;
                            }
                            if (tileNeighbour == tiles[3])
                            {
                                tileInPosDic.Add(pos, tiles[1]);
                                continue;
                            }
                        }
                    }

                    if (regionTarget == regionNameInTilePos[neighbour[(int)FullDirections.Left].x, neighbour[(int)FullDirections.Left].y])
                    {
                        if (tileInPosDic.TryGetValue(neighbour[(int)FullDirections.Left], out Tile tileNeighbour))
                        {
                            if (tileNeighbour == tiles[0])
                            {
                                tileInPosDic.Add(pos, tiles[1]);
                                continue;
                            }
                            if (tileNeighbour == tiles[1])
                            {
                                tileInPosDic.Add(pos, tiles[0]);
                                continue;
                            }
                            if (tileNeighbour == tiles[2])
                            {
                                tileInPosDic.Add(pos, tiles[3]);
                                continue;
                            }
                            if (tileNeighbour == tiles[3])
                            {
                                tileInPosDic.Add(pos, tiles[2]);
                                continue;
                            }
                        }
                    }


                    int leftMap = size.x - pos.x;

                    for (int i = 0; i < leftMap; i++)
                    {
                        Vector2Int alongWidthPos = neighbour[(int)FullDirections.RightBottom] + new Vector2Int(i, 0);

                        
                        if (regionTarget == regionNameInTilePos[alongWidthPos.x, alongWidthPos.y])
                        {
                            if (tileInPosDic.TryGetValue(alongWidthPos, out Tile tileNeighbour))
                            {
                                if (((alongWidthPos.x - pos.x) % 2) == 1)
                                {
                                    if (tileNeighbour == tiles[0])
                                    {
                                        tileInPosDic.Add(pos, tiles[3]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[1])
                                    {
                                        tileInPosDic.Add(pos, tiles[2]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[2])
                                    {
                                        tileInPosDic.Add(pos, tiles[1]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[3])
                                    {
                                        tileInPosDic.Add(pos, tiles[0]);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (tileNeighbour == tiles[0])
                                    {
                                        tileInPosDic.Add(pos, tiles[2]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[1])
                                    {
                                        tileInPosDic.Add(pos, tiles[3]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[2])
                                    {
                                        tileInPosDic.Add(pos, tiles[0]);
                                        break;
                                    }
                                    if (tileNeighbour == tiles[3])
                                    {
                                        tileInPosDic.Add(pos, tiles[1]);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!tileInPosDic.ContainsKey(pos))
                        tileInPosDic.Add(pos, tiles[0]);

                    #endregion Conditions for join tiles correctly
                }
            }
        }

        return tileInPosDic;
    }
}
