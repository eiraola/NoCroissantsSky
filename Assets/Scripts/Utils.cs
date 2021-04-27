using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    static int maxHeight = 150;
    static float smooth = 0.01f;
    static int octaves = 6;
    static float persistence = 0.5f;

    public static int GenerateHeight(float x, float y) {
        float height = Map(0, maxHeight, 0, 1, fBM(x * smooth, y* smooth, octaves, persistence));
        return (int)height;
    }

    static float Map(float newMin, float newMax, float originmin, float originmax, float value) {
        return Mathf.Lerp(newMin,newMax, Mathf.InverseLerp(originmin,originmax,value));
    }

    static float fBM(float x, float z , int oct, float pers) {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i = 0; i < oct; i++) {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= pers;
            frequency *= 2;
        }
        return total / maxValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
