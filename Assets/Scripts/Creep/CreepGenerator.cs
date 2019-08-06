using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreepGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;

    int radius = 20;
    int diameter;

    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        diameter = 2 * radius;
/*
        vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 0, 1);
        vertices[2] = new Vector3(1, 0, 0);
        vertices[3] = new Vector3(1, 0, 1);

        triangles = new int[]
        {   0, 1, 2,
            1, 3, 2
        };
*/
        vertices = new Vector3[(diameter + 1) * (diameter + 1)];

        for(int i = 0, z = -radius; z <= radius; z++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        triangles = new int[diameter * diameter * 6];
        int vertexIndex = 0;
        int triangleIndex = 0;
        for (int z = 0; z < diameter; z++)
        {
            for (int x = 0; x<diameter; x++)
            {
                if (vertices[vertexIndex + 0].magnitude < radius
                    && vertices[vertexIndex + diameter + 1].magnitude < radius
                    && vertices[vertexIndex + 1].magnitude < radius)
                {
                    
                    triangles[triangleIndex + 0] = vertexIndex + diameter + 1;
                    triangles[triangleIndex + 1] = vertexIndex + 1;
                    triangles[triangleIndex + 2] = vertexIndex;
                }
                if (vertices[vertexIndex + 1].magnitude < radius
                    && vertices[vertexIndex + diameter + 1].magnitude < radius
                    && vertices[vertexIndex + diameter + 2].magnitude < radius)
                {
                    triangles[triangleIndex + 3] = vertexIndex + 1;
                    triangles[triangleIndex + 4] = vertexIndex + diameter + 1;
                    triangles[triangleIndex + 5] = vertexIndex + diameter + 2;
                }
                

                vertexIndex++;
                triangleIndex += 6;
            }
            vertexIndex++;
        }
        
        
        /*    for (int i=0; i < 3; i++)
            {
                triangles[i] = i;
            }
    */
    }

    private void UpdateMesh()
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
        if (vertices == null)
            return;
        for(int i = 0; i < vertices.Length;i++) {
            Gizmos.DrawSphere(vertices[i], .04f);
      //      Gizmos.DrawIcon(vertices[i] + Vector3.up * 0.3f, "1", false);
            Handles.Label(vertices[i] + Vector3.up * 0.4f, i.ToString());
      //      Handles.DrawSphere(i,vertices[i], this.transform.rotation, .1f);
        }
    }
}
