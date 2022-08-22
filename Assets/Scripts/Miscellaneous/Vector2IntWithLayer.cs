using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Vector2IntWithLayer
{
    public int x;
    public int y;
    public int layer;

    public Vector2IntWithLayer(int x, int y, int layer)
    {
        this.x = x;
        this.y = y;
        this.layer = layer;
    }

}
