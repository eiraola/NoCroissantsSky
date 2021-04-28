using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    static int maxHeight = 150;
    static float smooth = 0.01f;
    static int octaves = 1;
    static float persistence = 0.5f;


    public static int GenerateStoneHeight(float x, float y)
    {
        float height = Map(0, maxHeight - 20, 0, 1, fBM(x * smooth*2, y * smooth * 2, octaves+1, persistence));
        return (int)height;
    }
    public static int GenerateHeight(float x, float y) {
        float height = Map(0, maxHeight, 0, 1, fBM(x * smooth, y* smooth, octaves, persistence));
        return (int)height;
    }

    static float Map(float newMin, float newMax, float originmin, float originmax, float value) {
        return Mathf.Lerp(newMin,newMax, Mathf.InverseLerp(originmin,originmax,value));
    }
    public static float fBM3D(float x, float y, float z, float sm, int oct) {
        float XY = fBM(x * smooth * sm, y * smooth, oct, 0.5f);
        float YZ = fBM(y * smooth * sm, z * smooth, oct, 0.5f);
        float XZ = fBM(x * smooth * sm, z * smooth, oct, 0.5f);

        float YX = fBM(y * smooth * sm, x * smooth, oct, 0.5f);
        float ZY = fBM(z * smooth * sm, y * smooth, oct, 0.5f);
        float ZX = fBM(z * smooth * sm, x * smooth, oct, 0.5f);
        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }
    static float fBM(float x, float z , int oct, float pers) {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = 1000;
        for (int i = 0; i < oct; i++) {
            total += Mathf.PerlinNoise((offset+x) * frequency, (offset + z) * frequency) * amplitude;
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
