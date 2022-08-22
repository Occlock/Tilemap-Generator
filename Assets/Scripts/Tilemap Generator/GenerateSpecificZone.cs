using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GenerateSpecificZone
{
    private int xZoneSize;
    private int yZoneSize;

    private string[,] regionBelowZoneNameInTilePos;
    private string[,] subRegionNameInTilePos;
    private string regionBelowZoneTarget;
    private string subRegionTarget;


    private Vector3Int[] perimeter;
    private List<List<Vector3Int>> zones = new List<List<Vector3Int>>();

    public GenerateSpecificZone(Tilemap tilemapLayer, int xZoneSize, int yZoneSize, string[,] regionNameInTilePos, string regionTarget, string[,] subRegionNameInTilePos, string subRegionTarget)
    {
        this.xZoneSize = xZoneSize;
        this.yZoneSize = yZoneSize;

        this.regionBelowZoneNameInTilePos = regionNameInTilePos;
        this.subRegionNameInTilePos = subRegionNameInTilePos;
        this.regionBelowZoneTarget = regionTarget;
        this.subRegionTarget = subRegionTarget;

        perimeter = new Vector3Int[(xZoneSize * 2) + (yZoneSize * 2)];
    }


    public List<List<Vector3Int>> ZoneFinalForm(List<List<Vector3Int>> zones)
    {
        List<List<Vector3Int>> finalZones = zones;
        finalZones = RemoveSpaceZones(finalZones);
        RemoveElementListOfList(finalZones);

        foreach (List<Vector3Int> zone in finalZones)
        {
            List<Vector3Int> infoForCut = CutElementsZoneInfo(zone);

            foreach (Vector3Int pos in infoForCut)
            {
                if (zone.Contains(pos))
                {
                    zone.Remove(pos);
                    subRegionNameInTilePos[pos.x, pos.y] = "";
                }
            }
        }

        RemoveElementListOfList(finalZones);

        return finalZones;
    }

    private void RemoveElementListOfList(List<List<Vector3Int>> zones)
    {
        foreach (List<Vector3Int> zone in zones)
        {
            System.Predicate<Vector3Int> removePos000OfInternalList = IsPos000;
            zone.RemoveAll(removePos000OfInternalList);
        }
    }

    private bool IsPos000(Vector3Int pos)
    {
        return pos == new Vector3Int(0, 0, 0);
    }

    private List<List<Vector3Int>> RemoveSpaceZones(List<List<Vector3Int>> finalZones)
    {
        foreach (List<Vector3Int> zone in finalZones)
        {
            int space = ConstValues.spaceSecurityForGrassDepth;
            int mediaSize = (xZoneSize + yZoneSize) / 2;
            int halfMedia = mediaSize / 2;

            int yZoneWithSpaces = yZoneSize + space + space;
            int xZoneWithSpaces = xZoneSize + space + space;

            for (int y = 0; y < yZoneWithSpaces; y++)
            {
                for (int x = 0; x < xZoneWithSpaces; x++)
                {
                    int index = x + (y * yZoneWithSpaces);

                    if (y == 0)
                    {
                        subRegionNameInTilePos[zone[index].x, zone[index].y] = "";
                        zone[index] = new Vector3Int(0, 0, 0);
                        continue;
                    }
                    if (x == xZoneWithSpaces - 1)
                    {
                        subRegionNameInTilePos[zone[index].x, zone[index].y] = "";
                        zone[index] = new Vector3Int(0, 0, 0);
                        continue;
                    }
                    if (x == 0)
                    {
                        subRegionNameInTilePos[zone[index].x, zone[index].y] = "";
                        zone[index] = new Vector3Int(0, 0, 0);
                        continue;
                    }
                    if (y == yZoneWithSpaces - 1)
                    {
                        subRegionNameInTilePos[zone[index].x, zone[index].y] = "";
                        zone[index] = new Vector3Int(0, 0, 0);
                        continue;
                    }
                }
            }
        }

        return finalZones;
    }

    private List<Vector3Int> CutElementsZoneInfo(List<Vector3Int> zone)
    {
        Vector3Int[] zoneExtremities = SortArraySDWA<Vector3Int>(zone);
        int[] cutZoneExtremities = Cuts();

        List<Vector3Int> infoForCut = new List<Vector3Int>();

        for (int i = 0; i < zoneExtremities.Length; i++)
        {
            if (i < Mathf.Sqrt(zone.Count) - 1)
            {
                for (int cut = 0; cut <= cutZoneExtremities[i] - 1; cut++)
                {
                    Vector3Int pos = zoneExtremities[i];
                    pos += new Vector3Int(0, +cut, 0);
                    infoForCut.Add(pos);
                }
            }
            else if (i < Mathf.Sqrt(zone.Count) * 2 - 2)
            {
                for (int cut = 0; cut <= cutZoneExtremities[i] - 1; cut++)
                {
                    Vector3Int pos = zoneExtremities[i];
                    pos += new Vector3Int(-cut, 0, 0);
                    infoForCut.Add(pos);
                }
            }
            else if (i < Mathf.Sqrt(zone.Count) * 3 - 3)
            {
                for (int cut = 0; cut <= cutZoneExtremities[i] - 1; cut++)
                {
                    Vector3Int pos = zoneExtremities[i];
                    pos += new Vector3Int(0, -cut, 0);
                    infoForCut.Add(pos);
                }
            }
            else if (i < Mathf.Sqrt(zone.Count) * 4 - 4)
            {
                for (int cut = 0; cut <= cutZoneExtremities[i] - 1; cut++)
                {
                    Vector3Int pos = zoneExtremities[i];
                    pos += new Vector3Int(-cut, 0, 0);
                    infoForCut.Add(pos);
                }
            }
        }

        return infoForCut;
    }

    private int[] Cuts()
    {
        int perimeter = (xZoneSize * 2) + (yZoneSize * 2);
        int side = 4;
        int corner = 4;
        int cut = 3;
        int countCutMin = 0;
        int countCutMax = perimeter / side / cut;
        int value = (countCutMax + countCutMin) / 2;

        int[] cuts = new int[perimeter - corner];

        for (int i = 0; i < cuts.Length; i++)
        {
            cuts[i] = value;

            if (Randomization(85))
            {
                if (Randomization(50))
                    value++;
                else
                    value--;
            }

            value = Mathf.Clamp(value, countCutMin, countCutMax);
        }

        cuts = CutsSmoothly(cuts);

        return cuts;
    }

    private int[] CutsSmoothly(int[] cuts)
    {
        int lastIndex = cuts.Length - 1;
        int firstIndex = 0;

        if (Mathf.Abs(cuts[lastIndex] - cuts[firstIndex]) > 1)
        {
            if (cuts[firstIndex] > cuts[lastIndex])
            {
                while (Mathf.Abs(cuts[lastIndex] - cuts[firstIndex]) > 1)
                {
                    cuts[firstIndex]--;
                }

                for (int i = 0; i < cuts.Length - 1; i++)
                {
                    while (Mathf.Abs(cuts[i] - cuts[i + 1]) > 1)
                    {
                        cuts[i + 1]--;
                    }
                }
            }
            else
            {
                while (Mathf.Abs(cuts[lastIndex] - cuts[firstIndex]) > 1)
                {
                    cuts[lastIndex]--;
                }
                for (int i = cuts.Length - 1; i > 0; i--)
                {
                    while (Mathf.Abs(cuts[i] - cuts[i - 1]) > 1)
                    {
                        cuts[i - 1]--;
                    }
                }
            }
        }

        return cuts;
    }

    private T[] SortArraySDWA<T>(List<T> disordenedArray)
    {
        int corners = 4;
        T[] array = new T[(((xZoneSize * 2) + (yZoneSize * 2)) - corners)];
        int index = 0;

        for (int y = 0; y < yZoneSize; y++)
        {
            for (int x = 0; x < xZoneSize; x++)
            {
                if (y == 0)
                {
                    array[index++] = disordenedArray[x + (y * yZoneSize)];
                    continue;
                }
                if (x == xZoneSize - 1)
                {
                    array[index++] = disordenedArray[x + (y * yZoneSize)];
                }
            }
        }

        for (int y = yZoneSize - 1; y > 0; y--)
        {
            for (int x = xZoneSize - 2; x >= 0; x--)
            {
                if (y == yZoneSize - 1)
                {
                    array[index++] = disordenedArray[x + (y * yZoneSize)];
                    continue;
                }
                if (x == 0)
                {
                    array[index++] = disordenedArray[x + (y * yZoneSize)];
                }
            }
        }

        return array;
    }

    public void CreateZoneWithMargeSpace(Vector3Int pos)
    {
        List<Vector3Int> zone = new List<Vector3Int>();
        int space = ConstValues.spaceSecurityForGrassDepth;

        for (int j = -space; j < yZoneSize + space; j++)
        {
            for (int i = -space; i < xZoneSize + space; i++)
            {
                Vector3Int zonePos = new Vector3Int(pos.x + i, pos.y + j, 0);

                if (regionBelowZoneNameInTilePos[zonePos.x, zonePos.y] == regionBelowZoneTarget)
                {
                    if (!(subRegionNameInTilePos[zonePos.x, zonePos.y] == subRegionTarget))
                    {
                        zone.Add(zonePos);
                    }
                }
            }
        }


        int horizontalSpace = space + space;
        int verticalSpace = space + space;

        if (zone.Count == (xZoneSize + horizontalSpace) * (yZoneSize + verticalSpace))
        {
            zones.Add(zone);

            for (int k = 0; k < zone.Count; k++)
                subRegionNameInTilePos[zone[k].x, zone[k].y] = subRegionTarget;
        }
    }

    public List<List<Vector3Int>> GetZones()
    {
        return zones;
    }
    public string[,] GetSubRegionsInTilePos()
    {
        return subRegionNameInTilePos;
    }

    private bool Randomization(int chance)
    {
        int prng = Random.Range(0, 100);

        if (prng < chance)
            return true;

        return false;
    }
}
