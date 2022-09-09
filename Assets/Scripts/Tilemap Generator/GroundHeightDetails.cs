using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GroundHeightDetails
{
    private string[,] subRegion;

    private GameObject go;

    //private int cut;

    public GroundHeightDetails(string[,] regions, int cut, string subRegionName)
    {
        go = Resources.Load<GameObject>("Prefabs/Text (TMP)");

        //this.cut = cut;

        subRegion = AddGroundToSubRegion(regions);
        RemoveBordersNameOfSubRegion();
        Cutting(ref cut);
        SetNameSubRegion(subRegionName);
    }

    private string[,] AddGroundToSubRegion(string[,] regions)
    {
        string[,] subRegion = new string[regions.GetLength(0), regions.GetLength(1)];

        for (int y = 0; y < regions.GetLength(1); y++)
        {
            for (int x = 0; x < regions.GetLength(0); x++)
            {
                if (regions[x, y] == Names.baseGrassName)
                    subRegion[x, y] = regions[x, y];

                if (regions[x, y] == Names.baseGrassBorderName)
                    subRegion[x, y] = regions[x, y];
            }
        }

        return subRegion;
    }

    private void RemoveBordersNameOfSubRegion()
    {
        for (int y = 0; y < subRegion.GetLength(1); y++)
        {
            for (int x = 0; x < subRegion.GetLength(0); x++)
            {
                if (subRegion[x, y] != null)
                {
                    if (subRegion[x, y].Contains(" border"))
                        subRegion[x, y] = string.Empty;
                }

            }

        }
    }

    private void Cutting(ref int cut)
    {
        List<Vector2Int> posForCut = new List<Vector2Int>();



        for (int y = 1; y < subRegion.GetLength(1) - 1; y++)
        {
            for (int x = 1; x < subRegion.GetLength(0) - 1; x++)
            {
                if (subRegion[x, y] != string.Empty && subRegion[x, y] != null)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    Vector2Int[] neighbourPos = Helper.CheckFullNeighbourTile(pos);

                    for (int p = 0; p < neighbourPos.Length; p++)
                    {
                        if (subRegion[neighbourPos[p].x, neighbourPos[p].y] == string.Empty || subRegion[neighbourPos[p].x, neighbourPos[p].y] == null)
                        {
                            posForCut.Add(pos);
                        }
                    }
                }
            }
        }

        for (int y = 0; y < subRegion.GetLength(1); y++)
        {
            for (int x = 0; x < subRegion.GetLength(0); x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (posForCut.Contains(pos))
                    subRegion[x, y] = string.Empty;

            }
        }

        if (cut > 0)
        {
            cut--;
            Cutting(ref cut);
        }
    }

    private void SetNameSubRegion(string subRegionName)
    {
        for (int y = 0; y < subRegion.GetLength(1); y++)
        {
            for (int x = 0; x < subRegion.GetLength(0); x++)
            {
                if (subRegion[x, y] != string.Empty && subRegion[x, y] != null)
                    subRegion[x, y] = subRegionName;
            }
        }
    }

    public string[,] GetGroundHeightDetailsRegion()
    {
        return subRegion;
    }

    private void DrawSubRegionPos()
    {
        GameObject canvas = new GameObject("Canvas", typeof(Canvas));
        Transform canvasTransform = canvas.GetComponent<Canvas>().transform;
        //Transform canvasTransform = GameObject.FindObjectOfType<Canvas>().transform;
        GameObject tes = new GameObject("Parent");
        tes.transform.SetParent(canvasTransform);


        for (int y = 0; y < subRegion.GetLength(1); y++)
        {
            for (int x = 0; x < subRegion.GetLength(0); x++)
            {
                if (subRegion[x, y] != string.Empty && subRegion[x, y] != null)
                {
                    Vector3 pos = new Vector3(x, y, 0);
                    StartUpGameObject(tes.transform, pos, subRegion[x, y]);
                }
            }
        }
    }

    private void StartUpGameObject(Transform parent, Vector3 pos, string regionName)
    {

        //GameObject test = GameObject.Instantiate(go, pos, Quaternion.identity);
        //test.transform.SetParent(parent);
        //test.GetComponent<TextMeshProUGUI>().text = regionName;


        GameObject go = new GameObject("Test Pos", typeof(TextMeshProUGUI));

        go.transform.SetParent(parent);

        TextMeshProUGUI textMeshPro = go.GetComponent<TextMeshProUGUI>();
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.pivot = new Vector2(0.0f, 0.0f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
        textMeshPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        textMeshPro.color = Color.cyan;
        textMeshPro.fontSize = 0.3f;
        textMeshPro.text = regionName;
        textMeshPro.transform.position = pos;
    }

}
