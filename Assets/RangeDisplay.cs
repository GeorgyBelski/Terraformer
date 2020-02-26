using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDisplay : MonoBehaviour
{
    public static int range = 5;
    public Color rangeColor;
    public Material rangeLineMaterial;
    [Header("References")]
    public LineRenderer rangeline;

    int previousRange;
    Vector3 previousPosition;
    
    void Start()
    {
        if (!rangeline)
        {
            rangeline = gameObject.AddComponent<LineRenderer>();
        }
        rangeline.positionCount = 72;
        rangeline.material = rangeLineMaterial;
        rangeline.material.SetColor("_BaseColor", rangeColor);
        rangeline.textureMode = LineTextureMode.RepeatPerSegment;
        rangeline.widthMultiplier = 0.1f;
        rangeline.loop = true;
    }

    void Update()
    {
        ShowRange();
    }

    void ShowRange()
    {
        if (previousRange != range || previousPosition != transform.position)
        {
            Vector3 compass = range * Vector3.forward;
            for (int i = 0; i < 72; i++)
            {
                Vector3 circlPoint = transform.position + compass;
                circlPoint.y += 0.1f;
                if (rangeline) rangeline.SetPosition(i, circlPoint);
                compass = Quaternion.AngleAxis(5, Vector3.up) * compass;//  —\|/—\|/ rotate the radius vector around planeNormal axis on 10 degrees.
            }
            previousRange = range;
            previousPosition = transform.position;
        }
    }
}
