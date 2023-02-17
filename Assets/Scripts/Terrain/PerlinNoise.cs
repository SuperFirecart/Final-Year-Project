using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : Noise
{
    public override float GetNoiseMap(float x, float y, float scale = 1f)
    {
        x = x * scale;
        y = y * scale;
        return Mathf.PerlinNoise(x, y);
    }
}