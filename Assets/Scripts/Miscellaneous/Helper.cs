using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Vector2Int[] CheckHorizontalVerticalNeighbourTile(Vector2Int pos)
    {
        Vector2Int[] result = new Vector2Int[4];

        result[0] = pos + new Vector2Int(+1, 0);
        result[1] = pos + new Vector2Int(0, -1);
        result[2] = pos + new Vector2Int(-1, 0);
        result[3] = pos + new Vector2Int(0, +1);

        return result;
    }
    public static Vector2Int[] CheckFullNeighbourTile(Vector2Int pos)
    {
        Vector2Int[] result = new Vector2Int[8];

        result[0] = pos + new Vector2Int(+1, 0);
        result[1] = pos + new Vector2Int(+1, -1);
        result[2] = pos + new Vector2Int(0, -1);
        result[3] = pos + new Vector2Int(-1, -1);
        result[4] = pos + new Vector2Int(-1, 0);
        result[5] = pos + new Vector2Int(-1, +1);
        result[6] = pos + new Vector2Int(0, +1);
        result[7] = pos + new Vector2Int(+1, +1);

        return result;
    }

    public static Vector2 GenerateSeed(int seed)
    {
        System.Random prng = new System.Random(seed);

        float randomizationX = prng.Next(-100000, 100000);
        float randomizationY = prng.Next(-100000, 100000);

        Vector2 randomization = new Vector2(randomizationX, randomizationY);

        return randomization;
    }

    public static int Map01ToMinBMaxB(float value, float minB, float maxB)
    {
        float percentage;

        float minA = 0f;
        float maxA = 1f;
        float securityMaxA = maxA - 0.00001f;

        value = Mathf.Clamp(value, minA, securityMaxA);

        percentage = (value - minA) * 100 / (maxA - minA);
        value = ((percentage * (maxB - minB)) + minB * 100) / 100;


        return Mathf.FloorToInt(value);
    }

    public static Vector3Int[] ArrayVector2IntToArrayVector3Int(Vector2Int[] oldArray)
    {
        Vector3Int[] newArray = new Vector3Int[oldArray.Length];

        for (int i = 0; i < oldArray.Length; i++)
            newArray[i] = new Vector3Int(oldArray[i].x, oldArray[i].y, 0);

        return newArray;
    }
}
