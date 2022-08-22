using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_Tests", menuName = "ScriptableObjects/Tests")]
public class SO_Tests : ScriptableObject
{
    public List<int> testingNumbers = new List<int>();

    public int nun;
}
