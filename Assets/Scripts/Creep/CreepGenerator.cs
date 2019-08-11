using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreepGenerator : MonoBehaviour
{
    enum PoligonOrientation {LeftDown, LeftUp, RightDown, RightUp };
    Mesh mesh;

    Vector3[] vertices;

    int[] triangles;
    [Range(1,100)]
    public int radius = 10;
    public int coefficient = 1;
    int previousRadius;
    int previousCoefficient;
    int diameter;

    void Start()
    {
        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        previousRadius = radius;
        previousCoefficient = coefficient;
        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        diameter = 2 * radius * coefficient;
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

        for(int i = 0, z = -radius * coefficient; z <= radius * coefficient; z++)
        {
            for (int x = -radius * coefficient; x <= radius * coefficient; x++)
            {
                vertices[i] = new Vector3((float)x / coefficient, 0, (float)z / coefficient);
                i++;
            }
        }

        triangles = new int[diameter * diameter * 6];
        int vertexIndex = 0;
        int angleIndex = 0;
        for (int z = -radius * coefficient; z < radius * coefficient; z++)
        {
            for (int x = -radius * coefficient; x < radius * coefficient; x++)
            {
                if (x < 0 && z < 0)
                {
                    if (vertices[vertexIndex + 0].magnitude < radius) // (x*x + z*z < radius*radius)
                    {
                        FillTriangle(true, PoligonOrientation.LeftDown, vertexIndex, angleIndex);
                        FillTriangle(false, PoligonOrientation.LeftDown, vertexIndex, angleIndex);
                    }
                    else if (vertices[vertexIndex + diameter + 1].magnitude < radius
                                && vertices[vertexIndex + 1].magnitude < radius)
                    {
                        FillTriangle(false, PoligonOrientation.LeftDown, vertexIndex, angleIndex);
                    }
                }
                else if (x >= 0 && z < 0)
                {
                    if (vertices[vertexIndex + 1].magnitude < radius) // (x*x + z*z < radius*radius)
                    {
                        FillTriangle(true, PoligonOrientation.RightDown, vertexIndex, angleIndex);
                        FillTriangle(false, PoligonOrientation.RightDown, vertexIndex, angleIndex);
                    }
                    else if (vertices[vertexIndex + diameter + 2].magnitude < radius
                                && vertices[vertexIndex].magnitude < radius)
                    {
                        FillTriangle(false, PoligonOrientation.RightDown, vertexIndex, angleIndex);
                    }
                }
                else if (x < 0 && z >= 0)
                {
                    if (vertices[vertexIndex + diameter + 1].magnitude < radius) // (x*x + z*z < radius*radius)
                    {
                        FillTriangle(true, PoligonOrientation.LeftUp, vertexIndex, angleIndex);
                        FillTriangle(false, PoligonOrientation.LeftUp, vertexIndex, angleIndex);
                    }
                    else if (vertices[vertexIndex + diameter + 2].magnitude < radius
                                && vertices[vertexIndex].magnitude < radius)
                    {
                        FillTriangle(false, PoligonOrientation.LeftUp, vertexIndex, angleIndex);
                    }
                }
                else //if(x >= 0 && z >= 0 )
                {
                    if (vertices[vertexIndex + diameter + 2].magnitude < radius) // (x*x + z*z < radius*radius)
                    {
                        FillTriangle(true, PoligonOrientation.RightUp, vertexIndex, angleIndex);
                        FillTriangle(false, PoligonOrientation.RightUp, vertexIndex, angleIndex);
                    }
                    else if (vertices[vertexIndex + diameter + 1].magnitude < radius
                                && vertices[vertexIndex + 1].magnitude < radius)
                    {
                        FillTriangle(false, PoligonOrientation.RightUp, vertexIndex, angleIndex);
                    }
                }
/*
                if (vertices[vertexIndex + 0].magnitude < radius
                    && vertices[vertexIndex + diameter * coefficient + 1].magnitude < radius
                    && vertices[vertexIndex + 1].magnitude < radius)
                {

                    triangles[angleIndex + 0] = vertexIndex + diameter * coefficient + 1;
                    triangles[angleIndex + 1] = vertexIndex + 1;
                    triangles[angleIndex + 2] = vertexIndex;
                }

                if (vertices[vertexIndex + 1].magnitude < radius
                    && vertices[vertexIndex + diameter * coefficient + 1].magnitude < radius
                    && vertices[vertexIndex + diameter * coefficient + 2].magnitude < radius)
                {
                    triangles[angleIndex + 3] = vertexIndex + 1;
                    triangles[angleIndex + 4] = vertexIndex + diameter * coefficient + 1;
                    triangles[angleIndex + 5] = vertexIndex + diameter * coefficient + 2;
                }
*/

                    vertexIndex++;
                angleIndex += 6;
            }
            vertexIndex++;
        }
        
        
        /*    for (int i=0; i < 3; i++)
            {
                triangles[i] = i;
            }
    */
    }

    private void FillTriangle(bool isFurther, PoligonOrientation orientation, int vertexIndex, int angleIndex)
    {
        int[] i = new int[3];
        if (!isFurther)
        {
            i[0] = 0; i[1] = 1; i[2] = 2;
        }
        else
        {
            i[0] = 3; i[1] = 4; i[2] = 5;
        }

        if ((orientation == PoligonOrientation.LeftDown && isFurther)||
            (orientation == PoligonOrientation.RightUp && !isFurther))
        {
            triangles[angleIndex + i[0]] = vertexIndex;
            triangles[angleIndex + i[1]] = vertexIndex + diameter + 1;
            triangles[angleIndex + i[2]] = vertexIndex + 1;
        }
        else if ((orientation == PoligonOrientation.RightDown && isFurther)||
                (orientation == PoligonOrientation.LeftUp && !isFurther))
        {
            triangles[angleIndex + i[0]] = vertexIndex + 1;
            triangles[angleIndex + i[1]] = vertexIndex;
            triangles[angleIndex + i[2]] = vertexIndex + diameter + 2;
        }
        else if ((orientation == PoligonOrientation.LeftUp && isFurther)|| 
                (orientation == PoligonOrientation.RightDown && !isFurther))
        {
            triangles[angleIndex + i[0]] = vertexIndex + diameter + 1;
            triangles[angleIndex + i[1]] = vertexIndex + diameter + 2;
            triangles[angleIndex + i[2]] = vertexIndex;
        }
        else if((orientation == PoligonOrientation.RightUp && isFurther)||
                (orientation == PoligonOrientation.LeftDown && !isFurther))
        {
            triangles[angleIndex + i[0]] = vertexIndex + diameter + 2;
            triangles[angleIndex + i[1]] = vertexIndex + 1;
            triangles[angleIndex + i[2]] = vertexIndex + diameter + 1;
        }
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
        if (previousRadius != radius || previousCoefficient!= coefficient) {
            previousRadius = radius;
            previousCoefficient = coefficient;
            CreateShape();
            UpdateMesh();
        }
    }
/*
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
*/
}
