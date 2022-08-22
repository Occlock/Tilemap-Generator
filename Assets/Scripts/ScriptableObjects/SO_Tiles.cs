using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tiles", menuName = "ScriptableObjects/Tiles")]
public class SO_Tiles : ScriptableObject
{
    public Tile[] tiles;
}
