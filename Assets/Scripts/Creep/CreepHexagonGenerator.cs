﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreepHexagonGenerator : MonoBehaviour
{
    [Range(1,49)]
    public int radius = 3;
    public int coefficient = 1;
    int previousRadius;
    int previousCoefficient;
    const int matrixDemension = 101;
    const int matrixCoordinateCenter = matrixDemension / 2;
    static GameObject hexagonPrefab = null;
    Dictionary<GameObject, Hexagon> meshHexagonMap = new Dictionary<GameObject, Hexagon>();
    static int[,] coordinates = new int[matrixDemension, matrixDemension];
    static Hexagon[,] hexagons = new Hexagon[matrixDemension, matrixDemension];
    static Vector3 hexaZ, hexaX;

    //Animation Wave
    public bool isExpanding;
    public float riseTime = 0.8f;
    public float risingHeight = 0.5f;
    public float scaleExternalCircleTime = 0.3f;
    float timerScaleExternalCircleTime;
    float[] timerRiseTime;      // own for each circle
    float[] offset;             // own for each circle
    int circleRadius = 1;
    bool isCircleSelected = false;
    List<Hexagon> hexagonFirstCircle = null;
    List<Hexagon> hexagonSecondCircle = null;
    List<Hexagon> externalCircle = null;
    List<Hexagon>[] Circles = null;
    void Start()
    {

        Vector3 rightUpVertexDirection = Quaternion.AngleAxis(60, Vector3.up) * Vector3.forward;
        hexaZ = (rightUpVertexDirection + Vector3.forward) * coefficient;
        hexaX = new Vector3(rightUpVertexDirection.x, 0, 0) * 2 * coefficient;
        Circles = new List<Hexagon>[matrixCoordinateCenter];
     /* timerRiseTime = new float[radius+1];
        timerPeriod = new float[radius+1];
        offset = new float[radius+1];
        
        UpdateCreep();
        for (int i = 0; i < 360; i++) {
            float j = (float)i / 100;
            Debug.Log("sin("+j+"):" + Mathf.Sin(j));
        }*/
    }

  //  [System.Serializable]
    public class Hexagon 
    {
        int coordinatHexaX, coordinatHexaZ;
        public Vector3 originalPosition;
        CreepHexagonGenerator parentCreep;
        public GameObject hexagonGObject;

        Mesh mesh;
        Vector3[] vertices;
        int[] triangles;

        int coefficient;

        public Hexagon(CreepHexagonGenerator parent, int x, int z)
        {
            if (hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] != null)
            { return; }
            this.parentCreep = parent;
            coordinatHexaX = x;
            coordinatHexaZ = z;
            coefficient = parentCreep.coefficient;

            if (hexagonPrefab == null)
            {
                hexagonGObject = new GameObject("hexagon_" + x + "," + z);
                hexagonGObject.transform.position = hexaX * x + hexaZ * z;
                hexagonPrefab = hexagonGObject;

                MeshFilter hexagonMeshFilter = hexagonGObject.AddComponent<MeshFilter>();
                MeshRenderer hexagonMeshRenderer = hexagonGObject.AddComponent<MeshRenderer>();
                hexagonMeshRenderer.material = parentCreep.GetComponent<MeshRenderer>().material;
                
                vertices = new Vector3[12];
                triangles = new int[4 * 3 + 12 *3];
                mesh = new Mesh();
                hexagonMeshFilter.mesh = mesh;

                Vector3 hexagonVertex = Vector3.forward * coefficient + parentCreep.transform.position;
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        vertices[i + j * 6] = hexagonVertex;
                        hexagonVertex = Quaternion.AngleAxis(60, Vector3.up) * hexagonVertex;
                    }
                    hexagonVertex = Vector3.forward * coefficient + parentCreep.transform.position + Vector3.down;
                }
                triangles[0] = 0;
                triangles[1] = 2;
                triangles[2] = 4;

                triangles[3] = 0;
                triangles[4] = 1;
                triangles[5] = 2;

                triangles[6] = 2;
                triangles[7] = 3;
                triangles[8] = 4;

                triangles[9] = 4;
                triangles[10] = 5;
                triangles[11] = 0;

                for (int i = 0; i < 5; i++)
                {
                    triangles[12 + i * 6 + 0] = i;
                    triangles[12 + i * 6 + 1] = i + 6;
                    triangles[12 + i * 6 + 2] = i + 7;
                    triangles[12 + i * 6 + 3] = i + 7;
                    triangles[12 + i * 6 + 4] = i + 1;
                    triangles[12 + i * 6 + 5] = i;
                }
                triangles[42] = 5;
                triangles[43] = 11;
                triangles[44] = 6;

                triangles[45] = 6;
                triangles[46] = 0;
                triangles[47] = 5;

                UpdateHexagonMesh();
            }
            else
            {
                hexagonGObject = Instantiate(hexagonPrefab, hexaX * x + hexaZ * z, hexagonPrefab.transform.rotation);
                /*
                Mesh mesh = hexagonGObject.GetComponent<MeshFilter>().mesh;
                this.mesh = mesh;
                this.vertices = mesh.vertices;
                vertices[0] += Vector3.up;
                this.triangles = mesh.triangles;
                UpdateHexagonMesh();
               */
            }
            
            coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = 1;
            hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = this;
            parentCreep.meshHexagonMap.Add(hexagonGObject, this);

            
            hexagonGObject.transform.SetParent(parentCreep.transform);
            
            originalPosition = hexagonGObject.transform.position;
        }

        void UpdateHexagonMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            var normals = new List<Vector3>();
            for (int i = 0; i<12; i++)
            {
                if(i<6)
                    normals.Add(Vector3.up);
                else
                    normals.Add(mesh.normals[i]);
            }
            mesh.SetNormals(normals);
        }

        public void Delete()
        {
            coordinates[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = 0;
            hexagons[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = null;
            parentCreep.meshHexagonMap.Remove(hexagonGObject);
            Destroy(hexagonGObject);
        }

        public void ResetPosition()
        {
            if (hexagonGObject)
            { hexagonGObject.transform.position = originalPosition; }
        }
    }

    public void CreateHexagon(int x, int z)
    { 
        if (x * z > 0)
        {
            if (Mathf.Abs(x) + Mathf.Abs(z) <= radius)
            {
                new Hexagon(this, x, z);
            }
        }
        else
        {
            new Hexagon(this, x, z);
        }

    }
    Hexagon GetHexagon(int x, int z)
    {
        return hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter];
    }
    void UpdateCreep()
    {
        if (previousRadius != radius)
        {
            if (radius < previousRadius) {
                externalCircle.Clear();
                ReduceSurface();
                return;
            }

            for (int z = -radius; z <= radius; z++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    CreateHexagon(x, z);  
                }
            }
            previousRadius = radius;

            timerRiseTime = new float[radius+1];
            offset = new float[radius+1];
        }
        
    }

    void ScaleExternalCircle()
    {
        if (timerScaleExternalCircleTime > 0)
        {
            timerScaleExternalCircleTime -= Time.deltaTime;
            
            externalCircle = SelectHexagonCircle(radius);
            externalCircle.ForEach(hexgon => 
            {
                hexgon.hexagonGObject.transform.localScale = (scaleExternalCircleTime - timerScaleExternalCircleTime) / scaleExternalCircleTime * Vector3.one;
            });
        }
    }
    void ReduceSurface(){
        for (int i = radius + 1; i <= previousRadius; i++) {
            Circles[i] = null;
            for (int j = 0; j <= previousRadius; j++)
            {
                DeleteHexagon(i - j, j);
                DeleteHexagon(j - i, -j);

                DeleteHexagon(i, -j);
                DeleteHexagon(j, -i);
                DeleteHexagon(-i, j);
                DeleteHexagon(-j, i);
            }
        }
        previousRadius = radius;
    }


    void DeleteHexagon(int x, int z)
    {
        if (GetHexagon(x, z) != null)
        {
            GetHexagon(x, z).Delete();
        }
    }

    void DeleteHexagon(GameObject hexagonGObject)
    {
        meshHexagonMap.TryGetValue(hexagonGObject, out Hexagon hexagon);
        if (hexagon != null)
        {
            hexagon.Delete();
        }
    }

    void Update()
    {
        
        MakeWave();
        UpdateCreep();
        ScaleExternalCircle();
    }

    void MakeWave()
    {
        if (!isExpanding)
        { return; }

        int i = circleRadius;

        if (!isCircleSelected) {
            hexagonFirstCircle = SelectHexagonCircle(i);
            if(i + 1 <= radius)
            {
                hexagonSecondCircle = SelectHexagonCircle(i+1);
            }
            isCircleSelected = true;
        } 

        timerRiseTime[i] += Time.deltaTime;
        offset[i] = CalculateOffset(i);
        if (timerRiseTime[i] <= 2 * riseTime)
        {
            RiseHexagons(i, hexagonFirstCircle);

            if (timerRiseTime[i] > riseTime && i+1 <= radius)
            {
                timerRiseTime[i+1] += Time.deltaTime;
                offset[i+1] = CalculateOffset(i+1);
                RiseHexagons(i+1, hexagonSecondCircle);
            }
        }
        else if(isExpanding)
        {
            isCircleSelected = false;
            offset[i] = 0;
            timerRiseTime[i] = 0;
            ResetHexagons(hexagonFirstCircle);
            if (i + 1 > radius)
            {
                isExpanding = false;
                circleRadius = 1;
                if(radius < matrixCoordinateCenter)
                {   radius++;
                    timerScaleExternalCircleTime = scaleExternalCircleTime;
                }              
            }
            else
            {
                circleRadius++;
            }
        }           

        
    }
    float CalculateOffset(int i)
    {
      //    return Mathf.Sin(timerRiseTime[i] * 6 / riseTime);
        return Mathf.Sin(timerRiseTime[i] * Mathf.PI / riseTime - Mathf.PI / 2) * 0.5f + 0.5f;
    }
    List<Hexagon> SelectHexagonCircle(int circleRadius)
    {
        //if (circleRadius > radius) { return null; }
        List<Hexagon> hexagonCircle;
        if (Circles[circleRadius] == null)
        {
         //   Debug.Log("Circle: "+ circleRadius);
            hexagonCircle = new List<Hexagon>();
            for (int j = 0; j <= circleRadius; j++)
            {
                hexagonCircle.Add(GetHexagon(circleRadius - j, j));
                hexagonCircle.Add(GetHexagon(-circleRadius + j, -j));
                if (j != 0)
                {
                    hexagonCircle.Add(GetHexagon(circleRadius, -j));
                    hexagonCircle.Add(GetHexagon(-circleRadius, j));
                    hexagonCircle.Add(GetHexagon(j, -circleRadius));
                    hexagonCircle.Add(GetHexagon(-j, circleRadius));
                }
            }
            Circles[circleRadius] = hexagonCircle;
        }
        else {
            return Circles[circleRadius];
        }
        return hexagonCircle;
    }

    void RiseHexagon(int i, Hexagon hexagon)
    {
        //if (i > radius) { return; }
        if (hexagon != null)
        { hexagon.hexagonGObject.transform.position = hexagon.originalPosition + Vector3.up * risingHeight * offset[i]; }
    }

    void RiseHexagons(int i, List<Hexagon> hexagonCircle) {
        //if (i > radius) { return; }
        hexagonCircle.ForEach(hexagon => RiseHexagon(i, hexagon));
      /*  foreach (Hexagon hexagon in hexagonCircle)
        {
            RiseHexagon(i, hexagon);
        }*/
    }

    void ResetHexagons(List<Hexagon> hexagonCircle)
    {
        hexagonCircle.ForEach(hexagon => hexagon.ResetPosition());
      /*  foreach (Hexagon hexagon in hexagonCircle)
        {
            hexagon.ResetPosition();
        }*/
    }

}
