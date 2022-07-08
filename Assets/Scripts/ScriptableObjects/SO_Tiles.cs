using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "so_Tiles", menuName = "ScriptableObjects/Tiles")]
public class SO_Tiles : ScriptableObject
{
    public List<Tile> tiles = new List<Tile>();
}
