using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(GenerateMap), true)]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GenerateMap generateMap = (GenerateMap)target;

        //if (mapDisplay.GetAutoUpdate())
        //    mapDisplay.DrawNoiseMap();


        if (GUILayout.Button("Generate"))
        {
            //mapDisplay.SetAutoUpdate(true);
            generateMap.Generate();
        }

        if (GUILayout.Button("Delete"))
        {
            //mapDisplay.SetAutoUpdate(false);
            generateMap.ClearTiles();
        }
    }
}
