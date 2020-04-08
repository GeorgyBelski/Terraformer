using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CreepHexagonGenerator;

public class TerrainManager : MonoBehaviour
{
    public Terrain terrain;
    public static TerrainData terrainData;
    public Texture2D splatMap;
    public Texture2D newSplatMap;
    public bool flipVertical =true;
    public Image image;
    public static float alphaMapUnitPixelFactorX;

    public CreepHexagonGenerator creep;
    int previousCreepRadius = 0;
    public static Dictionary<Hexagon, float> newHexagonsTimeMap = new Dictionary<Hexagon, float>(); // Hexagon wih it's timerSpread

    public static float SpreadTime = 1.5f;
    public static List<HexagonImpact> impacts = new List<HexagonImpact>();
    int skipIndex = 0;
    List<HexagonImpact> readyToRemove = new List<HexagonImpact>();
    public int impactCount;

    public static List<Hexagon> newHexagons = new List<Hexagon>();
    List<Hexagon> readyToRemoveFromNewList = new List<Hexagon>();

    public static int hexagonRadius = 5;

    public int alphamapWidth;
    public int alphamapHeight;
    int centerX, centerY;
    public static Vector2 terrainOffset;
    // float[,,] map;

    public class HexagonImpact 
    {
        public TerrainManager terrainManager;
        public float[,,] map =null;
        public float[,,] originalMap = null;
        public int alphaMapX;
        public int alphaMapY;
        public float timerSpread;
        public int radius = 1;
        public Vector2 position;

        public HexagonImpact(Hexagon hexagon) 
        {
            position = new Vector2(hexagon.hexagonGObject.transform.position.x, hexagon.hexagonGObject.transform.position.z);
            timerSpread = SpreadTime;

            alphaMapX = (int)Mathf.Round((hexagon.hexagonGObject.transform.position.x - terrainOffset.x) * alphaMapUnitPixelFactorX) - hexagonRadius;
            alphaMapY = (int)Mathf.Round((hexagon.hexagonGObject.transform.position.z - terrainOffset.y) * alphaMapUnitPixelFactorX) - hexagonRadius;
            int hexDiameter = hexagonRadius * 2;
            //    map = new float[hexDiameter, hexDiameter, 4];
            originalMap = terrainData.GetAlphamaps(alphaMapX, alphaMapY, hexDiameter, hexDiameter);
        }
    }

    void Start()
    {
        terrainData = terrain.terrainData;
        alphamapWidth = terrainData.alphamapWidth;
        alphamapHeight = terrainData.alphamapHeight;
        centerX = alphamapWidth / 2;
        centerY = alphamapHeight / 2;
        alphaMapUnitPixelFactorX = alphamapWidth / terrainData.size.x;
        terrainOffset = new Vector2(terrain.transform.position.x, terrain.transform.position.z);
        hexagonRadius = (int)Mathf.Round(creep.coefficient * alphaMapUnitPixelFactorX * 1.3f);

        splatMap = terrainData.GetAlphamapTexture(0);

        ImportTexture();

        image.material.SetTexture("_MainTex", splatMap);

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
        UpdateCreepImpact();
        impactCount = impacts.Count;
    }
    public void ImportTexture() 
    {
        if (!newSplatMap) { return; }

        int w = newSplatMap.width;
        var pixels = newSplatMap.GetPixels();
        if (flipVertical)
        {
            var h = w; // always square in unity
            for (var y = 0; y < h / 2; y++)
            {
                var otherY = h - y - 1;
                for (var x = 0; x < w; x++)
                {
                    var swapval = pixels[y * w + x];
                    pixels[y * w + x] = pixels[otherY * w + x];
                    pixels[otherY * w + x] = swapval;
                }
            }
        }
      //  splatMap.Resize(newSplat.width, newSplat.height, newSplat.format, true);
        splatMap.SetPixels(pixels);
        splatMap.Apply();
    }


    public static void AddHexagonImpact(Hexagon hexagon) 
    {
        // newHexagonsTimeMap.Add(hexagon, SpreadTime);
        newHexagons.Add(hexagon);
    //    impacts.Add(new HexagonImpact(hexagon));
        

    }
    void UpdateCreepImpact()
    {
        
        if(newHexagons.Count > 0)
        {
            newHexagons.ForEach(hexagon => impacts.Add(new HexagonImpact(hexagon)) );
            newHexagons.Clear();
        }

        if (impacts.Count > 0) 
        {
            if (skipIndex >= impacts.Count) { skipIndex = 0; }
            impacts.ForEach(impact => impact.timerSpread -= Time.deltaTime);
            impacts.Skip(skipIndex).Take(5).ToList().ForEach(impact => {
              //  impact.timerSpread -= Time.deltaTime;
                if(impact.timerSpread <= 0) 
                { readyToRemove.Add(impact); }
                UpdateHexagonImpact(impact);
            });
            RemoveImpactFromList();

            skipIndex++;
        }
    }
    void RemoveImpactFromList() 
    {/*
        if (readyToRemoveFromNewList.Count > 0)
        { 
            readyToRemoveFromNewList.ForEach(hexagon => newHexagonsTimeMap.Remove(hexagon));
            readyToRemoveFromNewList.Clear();
        }
        */
        if (readyToRemove.Count > 0)
        {
            readyToRemove.ForEach(impact => impacts.Remove(impact));
            readyToRemove.Clear();
        }
    }

    void UpdateHexagonImpact(HexagonImpact impact) 
    {
        int diameter = hexagonRadius * 2;
        int alphaMapX = impact.alphaMapX;
        int alphaMapY = impact.alphaMapY;
        impact.map = terrainData.GetAlphamaps(alphaMapX, alphaMapY, diameter, diameter);
        float impactAmount = 1 - impact.timerSpread / SpreadTime;
        for (int i = 0; i < hexagonRadius; i++)
        {
            for (int j = 0; j < hexagonRadius; j++)
            {
                if((i <= hexagonRadius* 0.7f && j <= hexagonRadius * 0.7f) || (i + j<= hexagonRadius)) { 
                    int x = i+ hexagonRadius;
                    int nx = -i + hexagonRadius;
                    int y = j+ hexagonRadius;
                    int ny = -j + hexagonRadius;

                    impact.map[x, y, 0] = impactAmount;
                    impact.map[nx, y, 0] = impactAmount;
                    impact.map[nx, ny, 0] = impactAmount;
                    impact.map[x, ny, 0] = impactAmount;
                    for(int l=1; l< 4; l++) { 
                        impact.map[x, y, l] = impact.originalMap[x, y, l] - impactAmount/2;
                        impact.map[nx, y, l] = impact.originalMap[nx, y, l] - impactAmount/2;
                        impact.map[nx, ny, l] = impact.originalMap[nx, ny, l] - impactAmount/2;
                        impact.map[x, ny, l] = impact.originalMap[x, ny, l] - impactAmount/2;
                    }
                }
                //   Debug.Log(x+","+y+ " : "+ map[x, y, 0]);

            }
        }
    //    Debug.Log("hexagon: "+hexagon.hexagonGObject.transform.position.x + "," + hexagon.hexagonGObject.transform.position.z);
    //    Debug.Log("terrain: " + terrain.transform.position.x + "," + terrain.transform.position.z);
    //    Debug.Log("alphaMap: "+alphaMapX + "," + alphaMapY);
        terrainData.SetAlphamaps(impact.alphaMapX, impact.alphaMapY, impact.map);
    }
}
