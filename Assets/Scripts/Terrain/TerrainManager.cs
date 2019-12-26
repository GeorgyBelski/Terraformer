using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainManager : MonoBehaviour
{
    public Terrain terrain;
    TerrainData terrainData;
    public Texture2D mapTexture;
    public Texture2D texture;
    public Image image;

    public int alphamapWidth;
    public int alphamapHeight;
    int centerX, centerY;
    float[,,] map;

    void Start()
    {
        terrainData = terrain.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;
        centerX = alphamapWidth / 2;
        centerY = alphamapHeight / 2;
        mapTexture = terrainData.GetAlphamapTexture(0);

        //    image.material.SetColor("_BaseColor", Color.red);
        image.material.SetTexture("_MainTex", mapTexture);
        
        map = new float[alphamapWidth, alphamapHeight, 2];

        for (int y = 0; y < alphamapHeight; y++)
        {
            for (int x = 0; x < alphamapWidth; x++)
            {
                map[x, y, 0] = 0;
                map[x, y, 1] = 1;
            }
        }

        for (int i = -40; i < 40; i++) 
        {
            for (int j = -10; j < 10; j++) 
            {
                int x = centerX + i;
                int y = centerY + j;
                map[x, y, 0] = 1;
                map[x, y, 1] = 0;
                Debug.Log(x+","+y+ " : "+ map[x, y, 0]);
                
            }
        }
        terrainData.SetAlphamaps(0, 0, map);
        /*
        float[,,] map = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, 2];

        // For each point on the alphamap...
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Get the normalized terrain coordinate that
                // corresponds to the the point.
                float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                // Get the steepness value at the normalized coordinate.
                var angle = terrainData.GetSteepness(normY, normX);

                // Steepness is given as an angle, 0..90 degrees. Divide
                // by 90 to get an alpha blending value in the range 0..1.
                var frac = angle / 90.0;
                map[x, y, 0] = (float)frac;
                map[x, y, 1] = (float)(1 - frac);
            }
        }
        terrainData.SetAlphamaps(0, 0, map);
        */
    }

    void Update()
    {
        
    }
}
