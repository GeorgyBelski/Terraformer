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
//    List<GameObject> hexagons = new List<GameObject>();
    int[,] coordinates = new int[matrixDemension, matrixDemension];
    GameObject[,] hexagons = new GameObject[matrixDemension, matrixDemension];
    Vector3 hexaZ, hexaX;
    void Start()
    {
        previousRadius = radius;
        previousCoefficient = coefficient;

        Vector3 rightUpVertexDirection = Quaternion.AngleAxis(60, Vector3.up) * Vector3.forward;
        hexaZ = (rightUpVertexDirection + Vector3.forward) * coefficient;
        hexaX = new Vector3(rightUpVertexDirection.x, 0, 0) * 2 * coefficient;
/*
        hexagons.Add(CreateHexagon(0, 0));
        hexagons.Add(CreateHexagon(1, 0));
        hexagons.Add(CreateHexagon(0, 1));
*/
        UpdateCreep();
    }

    GameObject CreateHexagon(int x, int z)
    {
        if (coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] == 1)
        { return null; }

        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[4 * 3];
        GameObject hexagon = new GameObject("hexagon_" + x + "," + z);
        coordinates[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = 1;
        hexagons[x + matrixCoordinateCenter, z + matrixCoordinateCenter] = hexagon;
        hexagon.transform.position = hexaX * x + hexaZ * z;
        hexagon.transform.SetParent(this.transform);
        MeshFilter hexagonMeshFilter = hexagon.AddComponent<MeshFilter>();
        MeshRenderer hexagonMeshRenderer = hexagon.AddComponent<MeshRenderer>();
        hexagonMeshRenderer.material = this.GetComponent<MeshRenderer>().material;

        Mesh mesh = new Mesh();
        hexagonMeshFilter.mesh = mesh;

        Vector3 hexagonVertex = Vector3.forward * coefficient;
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

        UpdateHexagonMesh(mesh, vertices, triangles);

        return hexagon;
    }


    void UpdateCreep()
    {
        for (int z = -radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * z > 0)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(z) <= radius) {
                        CreateHexagon(x, z);
                    }
                }
                else
                {
                    CreateHexagon(x, z);
                }
            }
        }
    }

    void UpdateHexagonMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void Update()
    {
        
    }

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
}
