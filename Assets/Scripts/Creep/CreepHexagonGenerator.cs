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
    void Start()
    {

        Vector3 rightUpVertexDirection = Quaternion.AngleAxis(60, Vector3.up) * Vector3.forward;
        hexaZ = (rightUpVertexDirection + Vector3.forward) * coefficient;
        hexaX = new Vector3(rightUpVertexDirection.x, 0, 0) * 2 * coefficient;

        UpdateCreep();
    }

  //  [System.Serializable]
    public class Hexagon 
    {
        int coordinatHexaX, coordinatHexaZ;
        CreepHexagonGenerator parentCreep;
        GameObject hexagonGObject;

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
            vertices = new Vector3[6];
            triangles = new int[4 * 3];
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
            for (int i = 0; i < 6; i++)
            {
                vertices[i] = hexagonVertex;
                hexagonVertex = Quaternion.AngleAxis(60, Vector3.up) * hexagonVertex;
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

            UpdateHexagonMesh();
        }

        void UpdateHexagonMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        public void Delete()
        {
            coordinates[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = 0;
            hexagons[coordinatHexaX + matrixCoordinateCenter, coordinatHexaZ + matrixCoordinateCenter] = null;
            parentCreep.meshHexagonMap.Remove(hexagonGObject);
            Destroy(hexagonGObject);
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
        }   
    }
    void ReduceSurface(){



        previousRadius = radius;
    }


    void DeleteHexagon(int x, int y)
    {
        hexagons[x + matrixCoordinateCenter, y + matrixCoordinateCenter].Delete();
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
    }
/*
    private void OnDrawGizmos()
    {
        Vector3[] vertices = null;
        if (hexagons[0,0] != null) {
            vertices = hexagons[0,0].GetComponent<MeshFilter>().mesh.vertices;
        }
        if (vertices == null)
            return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .04f);
            //      Gizmos.DrawIcon(vertices[i] + Vector3.up * 0.3f, "1", false);
            Handles.Label(vertices[i] + Vector3.up * 0.4f, i.ToString());
            //      Handles.DrawSphere(i,vertices[i], this.transform.rotation, .1f);
        }
    }
*/
}
