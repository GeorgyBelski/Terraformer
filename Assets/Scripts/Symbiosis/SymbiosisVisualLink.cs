﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbiosisVisualLink : MonoBehaviour
{
    public LineRenderer link;
    public Transform point0, point1;
    [Range(2,100)]
    public int positionsNumber = 6;
    public float waveHeight = 3;
    float currentWaveHeight;
    [Range(0.5f, 100)]
    public float switchPeriod = Mathf.PI*2;
    float timerSwitchPeriod;
    public float speed = 1;
    Vector3 end0, end1, previousEnd0, previousEnd1;
    Vector3 fromEnd0toEnd1, waveDirection;
    Vector3 segment;
    float radiansBySegment;
    Vector3[] stillPositions;
    float[] amplitudeMultipliers;
    float timeOffset;

    void Start()
    {
        SetLinePositions();
        timerSwitchPeriod = switchPeriod;
    }
    void Update()
    {
        if (previousEnd0 != point0.transform.position || previousEnd1 != point1.transform.position)
        { SetLinePositions(); }
        Wave();
    }
    void SetLinePositions()
    {
        end0 = point0.transform.position;
        end1 = point1.transform.position;
        fromEnd0toEnd1 = end1 - end0;
        Vector3 lineDirection = fromEnd0toEnd1.normalized;
        waveDirection = new Vector3(-lineDirection.z, 0, lineDirection.x);
        stillPositions = new Vector3[positionsNumber];
        amplitudeMultipliers = new float[positionsNumber]; // suppress amplitude by position: 0 near the ends;

        link.positionCount = positionsNumber;
        
        segment = fromEnd0toEnd1 / (positionsNumber - 1);
        radiansBySegment = 2 * Mathf.PI * segment.magnitude / fromEnd0toEnd1.magnitude;
        for (int i = 0; i < positionsNumber; i++)
        {
            stillPositions[i] = end0 + segment * i;
            link.SetPosition(i, stillPositions[i]);
            amplitudeMultipliers[i] =  Mathf.Sin(radiansBySegment/2 * i);
        }
        previousEnd0 = end0;
        previousEnd1 = end1;
    }
    void Wave()
    {
        if (switchPeriod == 0)
        { return; }

        timeOffset = Mathf.Sin(Time.time / switchPeriod);
        float periodicalAmplitudeChanger = Mathf.Cos(Time.time / switchPeriod);
        float periodicalSuppressorOfAmplitude = Mathf.Min(1, periodicalAmplitudeChanger);
        for (int i = 0; i < positionsNumber; i++)
        {
            float finalMultiplier = waveHeight * amplitudeMultipliers[i] * periodicalSuppressorOfAmplitude;
            link.SetPosition(i, stillPositions[i] + waveDirection * Mathf.Sin(radiansBySegment * i + timeOffset * speed) * finalMultiplier);
        }
    }

}
