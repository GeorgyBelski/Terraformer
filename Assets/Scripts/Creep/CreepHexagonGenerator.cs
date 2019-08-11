using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreepHexagonGenerator : MonoBehaviour
{
    List<GameObject> hexagons = new List<GameObject>();
    
    void Start()
    {
        hexagons.Add(CreateHexagon(0,0));
    }

    GameObject CreateHexagon(int x, int z)
    {
        Vector3[] vertices = new Vector3[6];
        int[] triangles = new int[4 * 3];
        Mesh mesh;

        GameObject hexagon = new GameObject();
        hexagon.name = "hexagon";
        hexagon.transform.SetParent(this.transform);
        MeshFilter hexagonMeshFilter = hexagon.AddComponent<MeshFilter>();
        MeshRenderer hexagonMeshRenderer = hexagon.AddComponent<MeshRenderer>();
        hexagonMeshRenderer.material = this.GetComponent<MeshRenderer>().material;

        mesh = new Mesh();
        hexagonMeshFilter.mesh = mesh;

        Vector3 hexagonVertex = Vector3.forward;
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

    private void UpdateHexagonMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
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
        if (hexagons.Count > 0) {
            vertices = hexagons[0].GetComponent<MeshFilter>().mesh.vertices;
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
