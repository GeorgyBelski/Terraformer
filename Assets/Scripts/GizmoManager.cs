using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    public Material gizmoMaterial;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /*
    void OnDrawGizmos()
    {
        if (TowerManager.towers.Count != 0) {
            
            GL.Begin(GL.LINES);
            material.SetPass(0);
            foreach (Tower tower in TowerManager.towers)
            {
                if (tower) {
                    Vector3 compass = tower.range * Vector3.forward;
                    GL.Color(tower.color);
                    for (int i = 0; i < 72; i++)
                    {
                        Vector3 circlPoint = tower.transform.position  - Vector3.up + compass;
                        GL.Vertex(circlPoint);
                        compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
                    }
                }
                    
            }
            GL.End();
        }  
    }
    */
    void OnPostRender()
    {
        
        GL.Begin(GL.LINES);
        if(gizmoMaterial)
            gizmoMaterial.SetPass(0);
        foreach (Tower tower in TowerManager.towers)
        {
            if (tower)
            {
                Vector3 compass = tower.range * Vector3.forward;
                GL.Color(tower.color);
                for (int i = 0; i < 72; i++)
                {
                    Vector3 circlPoint = tower.transform.position - Vector3.up + compass;
                    GL.Vertex(circlPoint);
                    compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
                }
            }

        }
        GL.End();
 
    }
}
