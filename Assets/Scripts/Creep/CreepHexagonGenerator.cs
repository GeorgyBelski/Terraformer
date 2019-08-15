using System.Collections;
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
    Dictionary<GameObject, Hexagon> meshHexagonMap = new Dictionary<GameObject, Hexagon>();
    static int[,] coordinates = new int[matrixDemension, matrixDemension];
    static Hexagon[,] hexagons = new Hexagon[matrixDemension, matrixDemension];
    static Vector3 hexaZ, hexaX;

    //Animation Wave
    float period =2;
    public float riseTime = 0.8f;
    float[] timerRiseTime;
    float[] timerPeriod;
    float[] offset;
    int circleRadius = 1;
    bool isCircleSelected = false;
    List<Hexagon> hexagonFirstCircle = null;
    List<Hexagon> hexagonSecondCircle = null;

    void Start()
    {

        Vector3 rightUpVertexDirection = Quaternion.AngleAxis(60, Vector3.up) * Vector3.forward;
        hexaZ = (rightUpVertexDirection + Vector3.forward) * coefficient;
        hexaX = new Vector3(rightUpVertexDirection.x, 0, 0) * 2 * coefficient;
     /* timerRiseTime = new float[radius+1];
        timerPeriod = new float[radius+1];
        offset = new float[radius+1];
        */
        UpdateCreep();
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
            vertices = new Vector3[12];
            triangles = new int[4 * 3 + 12 *3];
            hexagonGObject = new GameObject("hexagon_" + x + "," + z);
            
            coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = 1;
            hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = this;
            parentCreep.meshHexagonMap.Add(hexagonGObject, this);

            hexagonGObject.transform.position = hexaX * x + hexaZ * z;
            hexagonGObject.transform.SetParent(parentCreep.transform);
            MeshFilter hexagonMeshFilter = hexagonGObject.AddComponent<MeshFilter>();
            MeshRenderer hexagonMeshRenderer = hexagonGObject.AddComponent<MeshRenderer>();
            hexagonMeshRenderer.material = parentCreep.GetComponent<MeshRenderer>().material;

            mesh = new Mesh();
            hexagonMeshFilter.mesh = mesh;

            Vector3 hexagonVertex = Vector3.forward * coefficient + parentCreep.transform.position;
            for(int j = 0; j < 2; j++) { 
                for (int i = 0; i < 6; i++)
                {
                    vertices[i+j*6] = hexagonVertex;
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
            originalPosition = hexagonGObject.transform.position;
        }

        void UpdateHexagonMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
      /*      mesh.RecalculateNormals();*/
            var normals = new List<Vector3>();
            foreach (Vector3 vertex in vertices)
            {
                normals.Add(Vector3.up);
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
            hexagonGObject.transform.position = originalPosition;
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
            timerPeriod = new float[radius+1];
            offset = new float[radius+1];
        }   
    }
    void ReduceSurface(){
        for (int i = radius + 1; i <= previousRadius; i++) {
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
        if (hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] != null)
        {
            hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter].Delete();
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
        UpdateCreep();
        MakeWave();
    }

    void MakeWave()
    {
        int i = circleRadius;
        //i - circle radius

        if (!isCircleSelected) {
            hexagonFirstCircle = SelectHexagonCircle(i);
            if(i + 1 <= radius)
            {
                hexagonSecondCircle = SelectHexagonCircle(i+1);
            }
      //      Debug.Log("hexagonFirstCircle: " + hexagonFirstCircle.Count);
      //      Debug.Log("hexagonSecondCircle: " + hexagonSecondCircle.Count);
            isCircleSelected = true;
        } 
        if (timerPeriod[i] <= 0)
        {
            timerRiseTime[i] += Time.deltaTime;
            offset[i] = Mathf.Sin(timerRiseTime[i] * 6 / riseTime);
            if (offset[i] > 0)
            {
                RiseHexagons(i, hexagonFirstCircle);

                if (timerRiseTime[i] > riseTime/8 && i+1 <= radius)
                {
                    timerRiseTime[i+1] += Time.deltaTime;
                    offset[i+1] = Mathf.Sin(timerRiseTime[i+1] * 5 / riseTime);
                    RiseHexagons(i+1, hexagonSecondCircle);
                }
            }
            else
            {
                isCircleSelected = false;
                offset[i] = 0;
                timerRiseTime[i] = 0;
                ResetHexagons(hexagonFirstCircle);
                if (i + 1 > radius)
                {
                    timerPeriod[1] = period;
                    circleRadius = 1;
                }
                else
                {
                    circleRadius++;
                }

            }
            
        }
        else
        {
            
            timerPeriod[i] -= Time.deltaTime;
            if (timerPeriod[i] < 0) {
                timerPeriod[i] = 0;
            }
        }
        
    }

    List<Hexagon> SelectHexagonCircle(int circleRadius)
    {
        //if (circleRadius > radius) { return null; }
        var hexagonCircle = new List<Hexagon>();
        for (int j = 0; j <= circleRadius; j++)
        {
            hexagonCircle.Add(GetHexagon(circleRadius - j, j));
        }
        return hexagonCircle;
    }

    void RiseHexagon(int i, Hexagon hexagon)
    {
        //if (i > radius) { return; }
        hexagon.hexagonGObject.transform.position = hexagon.originalPosition + Vector3.up * offset[i];
    }

    void RiseHexagons(int i, List<Hexagon> hexagonCircle) {
        //if (i > radius) { return; }

        foreach (Hexagon hexagon in hexagonCircle)
        {
            RiseHexagon(i, hexagon);
        }
    }

    void ResetHexagons(List<Hexagon> hexagonCircle)
    {
        foreach (Hexagon hexagon in hexagonCircle)
        {
            hexagon.ResetPosition();
        }
    }




/*
    void OnDrawGizmos()
    {
      //  Debug.Log("GetHexagon(0,0) : " + GetHexagon(0, 0));
        Vector3[] vertices = null;
        if (GetHexagon(0, 0) != null) {
            vertices = GetHexagon(0, 0).hexagonGObject.GetComponent<MeshFilter>().mesh.vertices;
         //   Debug.Log("vertices.Length : " + vertices.Length);
            Gizmos.DrawSphere(vertices[0], .04f);
        }
        if (vertices == null)
            return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .04f);
          //        Gizmos.DrawIcon(vertices[i] + Vector3.up * 0.3f, "1", false);
            Handles.Label(vertices[i] + Vector3.up * 0.4f, i.ToString());
          //        Handles.DrawSphere(i,vertices[i], this.transform.rotation, .1f);
        }
    }
*/
}
