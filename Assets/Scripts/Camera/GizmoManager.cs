using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GizmoManager : MonoBehaviour
{
    public Material gizmoMaterial;
    [Range(10, 100)]
    public float gizmoRotationSpeed = 10f;
    Vector3 rotatingCompassDirection;
    public GizmoSubset gizmoSubset;
    void Start()
    {
        rotatingCompassDirection = Vector3.forward;
    }

    void Update()
    {
        rotatingCompassDirection = Quaternion.AngleAxis(gizmoRotationSpeed * Time.deltaTime, Vector3.up) * rotatingCompassDirection;
     //   ScriptableRenderContext.DrawGizmos();
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
        gizmoSubset = new GizmoSubset();
        GL.Begin(GL.LINES);
        if (gizmoMaterial)
        { 
            gizmoMaterial.SetPass(0);
        }
        
        foreach (Tower tower in TowerManager.towers)
        {
            DrawRange(tower, tower.rangeColor, tower.range, Vector3.forward);
            if (tower.isHighlighted) {
                DrawRange(tower, Color.yellow, TowerManager.selectedTowerRange , rotatingCompassDirection, 15);
            }
        }
        
        
        GL.End();
 
    }



    void DrawRange(Tower tower, Color lineColor , float radius , Vector3 compassDirection, float degree = 5) // compassDirection = Vector3.forward
    {
        
        if (tower)
        {
            
            Vector3 compass = radius * compassDirection;
            GL.Color(lineColor);
            for (int i = 0; i < (int)360f/degree; i++)
            {
                Vector3 circlPoint = tower.transform.position - Vector3.up + compass;
                GL.Vertex(circlPoint);
                compass = Quaternion.AngleAxis(degree, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 5 degrees.
            }
        }
    }
}
