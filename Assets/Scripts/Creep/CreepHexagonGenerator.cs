using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum HexCoordinatStatus {Void, Attend, Occupied,  Damaged };

public class CreepHexagonGenerator : MonoBehaviour
{
    public static CreepHexagonGenerator creepHexagonGenerator;
    public static int creepLayer = 14;
    public static int creepLayerMask = 1 << creepLayer;

    public static int hexagonsCount = 0;
    int previosHexagonCount;

    [Range(1,49)]
    public int radius = 3;
    public static int expansionCost, repairingCost;
    public int buildCost = 1, repairHexagonCost = 1; 

    public float coefficient = 1;
    int previousRadius;
    int previousCoefficient;
    const int matrixDemension = 101;
    const int matrixCoordinateCenter = matrixDemension / 2;
    static GameObject hexagonPrefab = null;
    public static Dictionary<GameObject, Hexagon> meshHexagonMap = new Dictionary<GameObject, Hexagon>();
    static HexCoordinatStatus[,] coordinates = new HexCoordinatStatus[matrixDemension, matrixDemension];
    static Hexagon[,] hexagons = new Hexagon[matrixDemension, matrixDemension];

    static Vector3 hexaZ, hexaX;
    public static List<Hexagon> damagedHexagons;

    //Animation Wave
    public bool isExpanding;
    bool isExpandingFinished = true;
    public bool isRepairing;
    public float riseTime = 0.8f;
    public float risingHeight = 0.5f;
    public float scaleExternalCircleTime = 0.3f;
    float timerScaleExternalCircleTime;
    bool isLockExpancion = false;
    float[] timerRiseTime;      // own for each circle
    float[] offset;             // own for each circle
    int circleRadius = 1;
    int expandedRadius = 0;
    bool isCircleSelected = false;
    List<Hexagon> hexagonFirstCircle = null;
    public List<Hexagon> hexagonSecondCircle = null;
    List<Hexagon> externalCircle = null;
    List<Hexagon>[] Circles = null;
    public static void Restart() 
    {
        hexagonsCount = 0;
        
        meshHexagonMap.Clear();
        hexagons = new Hexagon[matrixDemension, matrixDemension];
        coordinates = new HexCoordinatStatus[matrixDemension, matrixDemension];
        damagedHexagons.Clear();

        creepHexagonGenerator.radius = 0;
        creepHexagonGenerator.previousRadius = 0;
        creepHexagonGenerator.hexagonFirstCircle = null;
        creepHexagonGenerator.hexagonSecondCircle = null;
        creepHexagonGenerator.externalCircle = null;
        creepHexagonGenerator.Circles = null;
        creepHexagonGenerator.isExpanding = false;
        creepHexagonGenerator.isExpandingFinished = true;
        creepHexagonGenerator.isRepairing = false;
        creepHexagonGenerator.isLockExpancion = false;
        creepHexagonGenerator.isCircleSelected = false;
    }
    void Start()
    {
        creepHexagonGenerator = this;
        
        Vector3 rightUpVertexDirection = Quaternion.AngleAxis(60, Vector3.up) * Vector3.forward;
        hexaZ = (rightUpVertexDirection + Vector3.forward) * coefficient;
        hexaX = new Vector3(rightUpVertexDirection.x, 0, 0) * 2 * coefficient;
        Circles = new List<Hexagon>[matrixCoordinateCenter];
        timerRiseTime = new float[radius+1];
        UpdateExpansionCost();
        damagedHexagons = new List<Hexagon>();
        UpdateCreep();
        // Occupied by Terraformer
        GetHexagon(0, 0).SetStatus(HexCoordinatStatus.Occupied);
        // Occupied by Terraformer
        SelectHexagonCircle(1).ForEach(hexagon => hexagon.SetStatus(HexCoordinatStatus.Occupied));
     /*
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
        public int coordinatHexaX, coordinatHexaZ;
        public Vector3 originalPosition;
        public bool isTarget;
        CreepHexagonGenerator parentCreep;
        public GameObject hexagonGObject;
        public static float texRes = 2014;

        Mesh mesh;
        Vector3[] vertices;
        Vector2[] uvs;
        int[] triangles;

        float coefficient;

        public Hexagon(CreepHexagonGenerator parent, int x, int z)
        {
            
            this.parentCreep = parent;
            coordinatHexaX = x;
            coordinatHexaZ = z;
            coefficient = parentCreep.coefficient;
            hexagonsCount++;
            parentCreep.CalculateCreepIncome();
            if (hexagonPrefab == null)
            {
                hexagonGObject = new GameObject("hexagon_" + x + "," + z);
                hexagonGObject.transform.position = hexaX * x + hexaZ * z;
                hexagonPrefab = hexagonGObject;

                MeshFilter hexagonMeshFilter = hexagonGObject.AddComponent<MeshFilter>();
                MeshRenderer hexagonMeshRenderer = hexagonGObject.AddComponent<MeshRenderer>();
                hexagonMeshRenderer.material = parentCreep.GetComponent<MeshRenderer>().material;

                // vertices = new Vector3[12];
                vertices = new Vector3[30];
                uvs = new Vector2[vertices.Length];
                triangles = new int[4 * 3 + 12 *3];
                mesh = new Mesh();
                hexagonMeshFilter.mesh = mesh;

                Vector3 hexagonVertex = Vector3.forward * coefficient + parentCreep.transform.position;
                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (j == 0)
                        {
                            vertices[i + j * 6] = hexagonVertex;  
                        }
                        else 
                        {
                            vertices[i * 4 + j * 6] = hexagonVertex;                           
                            vertices[i * 4 + 1 + j * 6] = hexagonVertex + Vector3.down;

                            vertices[i * 4 +2 + j * 6] = hexagonVertex;
                            vertices[i * 4 + 3 + j * 6] = hexagonVertex + Vector3.down;

                            uvs[i * 4 + j * 6] = new Vector2(1774 / texRes, 1 - 570/ texRes);
                            uvs[i * 4 + 1 + j * 6] = new Vector2(1362 / texRes, 1 - 1287 / texRes);

                            uvs[i * 4 + 2 + j * 6] = new Vector2(1030 / texRes, 1 - 136/ texRes);
                            uvs[i * 4 + 3 + j * 6] = new Vector2(621 / texRes, 1 - 848/ texRes);
                        }
                        hexagonVertex = Quaternion.AngleAxis(60, Vector3.up) * hexagonVertex;
                    }
                    uvs[0] = new Vector2(1010 / texRes, 1 - 20 / texRes);
                    uvs[1] = new Vector2(1860 / texRes, 1 - 521 / texRes);
                    uvs[2] = new Vector2(1860 / texRes, 1 - 1500 / texRes);
                    uvs[3] = new Vector2(1010 / texRes, 1 - 1990 / texRes);
                    uvs[4] = new Vector2(152 / texRes, 1 - 1500 / texRes);
                    uvs[5] = new Vector2(152 / texRes, 1 - 521 / texRes);
                    // hexagonVertex = Vector3.forward * coefficient + parentCreep.transform.position + Vector3.down;
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
                    int n = 8 + i * 4;
                    triangles[12 + i * 6 + 0] = n;      //i;
                    triangles[12 + i * 6 + 1] = n + 1;  //i + 6;
                    triangles[12 + i * 6 + 2] = n + 3;  //i + 7;
                    triangles[12 + i * 6 + 3] = n + 3;  // i + 7;
                    triangles[12 + i * 6 + 4] = n + 2;  //i + 1;
                    triangles[12 + i * 6 + 5] = n;      //i;
                }
                triangles[42] = 28; //5;
                triangles[43] = 29; //11;
                triangles[44] = 7;  //6;

                triangles[45] = 7;  //6;
                triangles[46] = 6;  //0;
                triangles[47] = 28; //5;

                UpdateHexagonMesh();
                hexagonGObject.layer = creepLayer;
                MeshCollider meshCollider = hexagonGObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }
            else
            {
                hexagonGObject = Instantiate(hexagonPrefab, hexaX * x + hexaZ * z, hexagonPrefab.transform.rotation);
                
                // place for landscape alinement!
                /*
                Mesh mesh = hexagonGObject.GetComponent<MeshFilter>().mesh;
                this.mesh = mesh;
                this.vertices = mesh.vertices;
                vertices[0] += Vector3.up;
                this.triangles = mesh.triangles;
                UpdateHexagonMesh();
               */
            }
            
            coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = HexCoordinatStatus.Attend;
            hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = this;
            meshHexagonMap.Add(hexagonGObject, this);

            
            hexagonGObject.transform.SetParent(parentCreep.transform);
            
            originalPosition = hexagonGObject.transform.position;
            TerrainManager.AddHexagonImpact(this);
        }

        void UpdateHexagonMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

           /* var normals = new List<Vector3>();
            for (int i = 0; i<12; i++)
            {
                if(i<6)
                    normals.Add(Vector3.up);
                else
                    normals.Add(mesh.normals[i]);
            }
            mesh.SetNormals(normals);*/
        }

        public void Delete()
        {
            coordinates[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = HexCoordinatStatus.Void;
            hexagons[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = null;
            meshHexagonMap.Remove(hexagonGObject);
            Destroy(hexagonGObject);
            if (damagedHexagons.Contains(this))
            {
                damagedHexagons.Remove(this);
            }
            hexagonsCount--;
            parentCreep.CalculateCreepIncome();
            // hexagonGObject.SetActive(false);
        }

        public void DamageHexagon()
        {
            if (GetStatus() == HexCoordinatStatus.Damaged)
            { return; }
            damagedHexagons.Add(this);
            SetStatus(HexCoordinatStatus.Damaged);
            hexagonGObject.transform.localScale = Vector3.one * 0.4f;
            // hexagon.Delete();
            hexagonsCount--;
            parentCreep.UpdateRepairingCost();
            parentCreep.CalculateCreepIncome();
        }

        public void ResetPosition()
        {
            if (hexagonGObject)
            { hexagonGObject.transform.position = originalPosition; }
        }
        public void SetStatus(HexCoordinatStatus status)
        {
            int x = this.coordinatHexaX, z = this.coordinatHexaZ;
            coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = status;
        }
        public HexCoordinatStatus GetStatus() 
        {
            return parentCreep.GetCoordinateStatus(coordinatHexaX, coordinatHexaZ);
        }
    }

    void Update()
    {
        ExpandCreep();
        UpdateCreep();
        ScaleExternalCircle();
        RepairHexagons();
    }
    public static void DisplayExpensionCost()
    {
        ResourceManager.resourceCost.text = "-" + expansionCost;
    }
    public static void DisplayRepairingCost()
    {
        if (repairingCost > 0)
        { ResourceManager.resourceCost.text = "-" + repairingCost; }
    }
    void UpdateExpansionCost()
    {
        expansionCost = 6 * (buildCost + radius) * (radius + 1);
    }
    void UpdateRepairingCost()
    {
        repairingCost = repairHexagonCost * damagedHexagons.Count;
    }
    public static void Expand()
    {
        if (ResourceManager.RemoveResource(expansionCost))
        { creepHexagonGenerator.isExpanding = true; }
        else
        { ResourceManager.CostIsTooHighSignal(); }
    }
    public static void Repair()
    {
        if (ResourceManager.RemoveResource(repairingCost))
        { creepHexagonGenerator.isRepairing = true;}
        else
        { ResourceManager.CostIsTooHighSignal(); }
    }
    public void CalculateCreepIncome()
    {
        ResourceManager.income = hexagonsCount * ResourceManager.incomeFromHexagon;
    }
    public void CreateHexagon(int x, int z)
    {
        if (GetHexagon(x, z) != null)
        { return; }

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
    HexCoordinatStatus GetCoordinateStatus(int x, int z)
    {
        return coordinates[x + matrixCoordinateCenter,z + matrixCoordinateCenter];
    }


    void UpdateCreep()
    {
        if (previousRadius != radius)
        {
            if (radius < previousRadius) {
             //   externalCircle.Clear();
                ReduceSurface();
                UpdateExpansionCost();
                return;
            }

            for (int z = -radius; z <= radius; z++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    CreateHexagon(x, z);  
                }
            }
            
            float externalTimerRiseTime = timerRiseTime[previousRadius];
            timerRiseTime = new float[radius+1];
            timerRiseTime[previousRadius] = externalTimerRiseTime;
            offset = new float[radius+1];
            previousRadius = radius;
        }
        
    }

    void ScaleExternalCircle()
    {
        if (timerScaleExternalCircleTime > 0)
        {
            timerScaleExternalCircleTime -= Time.deltaTime;
            if (externalCircle == null)
            {   externalCircle = SelectHexagonCircle(radius); }

            float hexagonScale = 0;
            externalCircle.ForEach(hexgon => 
            {
                hexagonScale = (scaleExternalCircleTime - timerScaleExternalCircleTime) / scaleExternalCircleTime;
                if (hexagonScale > 1)
                {
                    hexagonScale = 1;
                   // hexagonsCount++;
                }

                hexgon.hexagonGObject.transform.localScale = hexagonScale * Vector3.one;
            });
            if (hexagonScale == 1)
            {
                externalCircle = null;
                
                if (!isExpandingFinished)
                { isExpanding = true; }
                
            }
            
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
    /*
    void DeleteHexagon(GameObject hexagonGObject)
    {
        meshHexagonMap.TryGetValue(hexagonGObject, out Hexagon hexagon);
        if (hexagon != null)
        {
            hexagon.Delete();
        }
    }
    */

    public void RepairHexagons()
    {
        if (!isRepairing)
        { return; }
        if (damagedHexagons.Count == 0)
        { isRepairing = false; }
        damagedHexagons.ForEach(hexagon =>
        {
            coordinates[hexagon.coordinatHexaX + matrixCoordinateCenter, hexagon.coordinatHexaZ + matrixCoordinateCenter] = HexCoordinatStatus.Attend;
            // hexagon.hexagonGObject.transform.localScale =Vector3.one;
            if (externalCircle == null)
            {
                externalCircle = new List<Hexagon>();
                timerScaleExternalCircleTime = scaleExternalCircleTime;
            }
            externalCircle.Add(hexagon);
        });
        hexagonsCount += damagedHexagons.Count;
        damagedHexagons.Clear();
        UpdateRepairingCost();
        CalculateCreepIncome();
      //  isRepairing = false;
    }




    void ExpandCreep()
    {
        if (!isExpanding)
        { return;}

        if (damagedHexagons.Count > 0)
        {
            isExpanding = false;
            return;
        }else
        { isExpandingFinished = false;}

        int i = circleRadius;

        if (!isLockExpancion)
        {
            isLockExpancion = true;
            expandedRadius = radius + 1;
        }

        if (!isCircleSelected) {
            hexagonFirstCircle = SelectHexagonCircle(i);
            isCircleSelected = true;
        } 

        timerRiseTime[i] += Time.deltaTime;
        offset[i] = CalculateOffset(i);
        if (timerRiseTime[i] <= 2 * riseTime)
        {
            RiseHexagons(i, hexagonFirstCircle);

            if (timerRiseTime[i] > riseTime && i + 1 <= radius)
            {
                timerRiseTime[i + 1] += Time.deltaTime;
                offset[i + 1] = CalculateOffset(i + 1);
                hexagonSecondCircle = SelectHexagonCircle(i + 1);
                RiseHexagons(i + 1, hexagonSecondCircle);
            }
            if (timerRiseTime[i] > 2*riseTime - scaleExternalCircleTime && i + 1 == expandedRadius)
            {              
                if (radius < matrixCoordinateCenter && timerScaleExternalCircleTime <= 0)
                {
                    radius = expandedRadius;
                    timerScaleExternalCircleTime = scaleExternalCircleTime;
                    
                }
            }  
        }
        else
        {
            isCircleSelected = false;
            offset[i] = 0;
            timerRiseTime[i] = 0;
            ResetHexagons(hexagonFirstCircle);
            if (i+1 > expandedRadius)
            {
                isExpanding = false;
                isExpandingFinished = true;
                isLockExpancion = false;
                circleRadius = 1;
                UpdateExpansionCost();
            }
            else
            {
                circleRadius++;
            }
        }             
    }

    float CalculateOffset(int i)
    {
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
                    if(j < circleRadius) 
                    { 
                        hexagonCircle.Add(GetHexagon(j, -circleRadius));
                        hexagonCircle.Add(GetHexagon(-j, circleRadius));
                    }
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
    }

    void ResetHexagons(List<Hexagon> hexagonCircle)
    {
        hexagonCircle.ForEach(hexagon => hexagon.ResetPosition());
    }

}
