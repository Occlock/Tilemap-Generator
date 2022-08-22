using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Noise : MonoBehaviour
{
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public int Seed { get; set; }
    public int Octaves { get; set; }
    public float Scale { get; set; }
    public float Lacunarity { get; set; }


    public int MargeSeed { get; set; }
    public float MargeScale { get; set; }
    public float MargeLacunarity { get; set; }
    public int MargeMaxRemove { get; set; }


    private void OnValidate()
    {
        OnScale();
        OnMapSize();
    }

    private void OnScale()
    {
        if (Scale <= 0)
            Scale = 0.0001f;

        if (MargeScale <= 0)
            MargeScale = 0.0001f;
    }

    private void OnMapSize()
    {
        if (MapWidth < 1)
            MapWidth = 1;

        if (MapHeight < 1)
            MapHeight = 1;
    }


    public float[,] GenerateNoiseMap()
    {
        Vector2 randomization = Helper.GenerateSeed(Seed);
        
        float[,] noiseMap = new float[MapWidth, MapHeight];


        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float frequency = Mathf.Pow(Lacunarity, Octaves);

                float sampleX = x / Scale * frequency + randomization.x;
                float sampleY = y / Scale * frequency + randomization.y;
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }


    public float[] GenerateNoiseMargeMap()
    {
        Vector2 randomization = Helper.GenerateSeed(MargeSeed);

        float[] noiseMargeMap = new float[MapWidth + MapWidth + MapHeight + MapHeight];


        for (int i = 0; i < noiseMargeMap.Length; i++)
        {
            float frequency = MargeLacunarity;

            float sampleX = i / MargeScale * frequency + randomization.x;
            float sampleY = i / MargeScale * frequency - randomization.y;
            float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

            noiseMargeMap[i] = perlinValue;
        }

        return noiseMargeMap;
    }

}


