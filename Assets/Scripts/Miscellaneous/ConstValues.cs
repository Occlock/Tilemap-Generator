using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstValues
{
    public const int grassLayerCount = 2;
    public const int mediumGrassLayerCount = 2;
    public const int highGrassLayerCount = 2;

    public const int grassBaseLayer = (int)EnumTilemaps.Ground;
    public const int mediumGrassLayer = (int)EnumTilemaps.GroundMidGrass;
    public const int highGrassLayer = (int)EnumTilemaps.GroundHighGrass;

    public const int worldTile = 32;
    public const int tile128x = 128;
    public const int tile128y = 128;

    public const int spaceSecurityForGrassDepth = 1;

}
