using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbiosisVisualLink : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3 point0, point1;
    [Range(2,100)]
    public int positionsNumber = 6;
    public float waveHeight = 3;
    float currentWaveHeight;
    [Range(0.5f, 100)]
    public float switchPeriod = Mathf.PI*2;
    float timerSwitchPeriod;
    public float speed = 1;
    float ordinarySpeed, ordinaryWaveHeight;
    public float breakingTime = 0.6f;
    float timerBreakingTime;
    bool isBreaking = false;
    Vector3 end0, end1, previousEnd0, previousEnd1;
    Vector3 fromEnd0toEnd1, waveDirection;
    Vector3 segment;
    float radiansBySegment;
    Vector3[] stillPositions;
    float[] amplitudeMultipliers;
    float timeOffset;
    float timeRandomizer;
    Gradient gradient;
    GradientAlphaKey[] gradientAlphaKeys;
    GradientColorKey[] gradientColorKeys;

    void Start()
    {
        SetLinePositions();
        timerSwitchPeriod = switchPeriod;
        timeRandomizer = Random.Range(0f, 20f);
        ordinarySpeed = speed;
        ordinaryWaveHeight = waveHeight;



    }
    void Update()
    {
        if (previousEnd0 != point0 || previousEnd1 != point1)
        { SetLinePositions(); }
        Wave();
        BreakingProcess();
    }

    public void SetEndPoints(Vector3 p0, Vector3 p1)
    {
        point0 = p0;
        point1 = p1;
    }

    public void SetEndColors(Color c0, Color c1)
    {
        lineRenderer.startColor = c0;
        lineRenderer.endColor = c1;

        gradient = lineRenderer.colorGradient;
        gradientColorKeys = gradient.colorKeys;
        gradientAlphaKeys = gradient.alphaKeys;
        gradientColorKeys[0].time = 0.0f;
        gradientColorKeys[1].time = 1f;
        gradientAlphaKeys[1].alpha = 0f;
        gradientAlphaKeys[2].alpha = 0f;
    //    gradient.mode = GradientMode.Blend;
    }

    public void SetGradientProgress(float ratio)
    {
        if (ratio < 1)
        {
            MoveGradient(ratio);

        }
        else
        {
            gradientColorKeys[0].time = 0f;
            gradientColorKeys[1].time = 1f;
            gradientAlphaKeys[1].alpha = 1f;
            gradientAlphaKeys[2].alpha = 1f;
        }

        gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);
        lineRenderer.colorGradient = gradient;
    }

    void MoveGradient(float ratio)
    {
        gradientAlphaKeys[1].time = ratio / 2 + 0.01f;
        gradientAlphaKeys[0].time = gradientAlphaKeys[1].time - 0.04f;
        gradientColorKeys[0].time = gradientAlphaKeys[0].time;
        gradientAlphaKeys[2].time = 1 - ratio / 2 - 0.01f;
        gradientAlphaKeys[3].time = gradientAlphaKeys[2].time + 0.04f;
        gradientColorKeys[1].time = gradientAlphaKeys[2].time;
    }

    void SetLinePositions()
    {
        end0 = point0;
        end1 = point1;
        fromEnd0toEnd1 = end1 - end0;
        Vector3 lineDirection = fromEnd0toEnd1.normalized;
        waveDirection = new Vector3(-lineDirection.z, 0, lineDirection.x);
        stillPositions = new Vector3[positionsNumber];
        amplitudeMultipliers = new float[positionsNumber]; // suppress amplitude by position: 0 near the ends;

        lineRenderer.positionCount = positionsNumber;
        
        segment = fromEnd0toEnd1 / (positionsNumber - 1);
        radiansBySegment = 2 * Mathf.PI * segment.magnitude / fromEnd0toEnd1.magnitude;
        for (int i = 0; i < positionsNumber; i++)
        {
            stillPositions[i] = end0 + segment * i;
            lineRenderer.SetPosition(i, stillPositions[i]);
            amplitudeMultipliers[i] =  Mathf.Sin(radiansBySegment/2 * i);
        }
        previousEnd0 = end0;
        previousEnd1 = end1;
    }
    void Wave()
    {
        if (switchPeriod == 0)
        { return; }

        float timeArgument = (Time.time + timeRandomizer) / switchPeriod;
        timeOffset = Mathf.Sin(timeArgument);
        float periodicalAmplitudeChanger = Mathf.Cos(timeArgument);
        float periodicalSuppressorOfAmplitude = Mathf.Min(1, periodicalAmplitudeChanger);
        for (int i = 0; i < positionsNumber; i++)
        {
            float finalMultiplier = waveHeight * amplitudeMultipliers[i] * periodicalSuppressorOfAmplitude;
            lineRenderer.SetPosition(i, stillPositions[i] + waveDirection * Mathf.Sin(radiansBySegment * i + timeOffset * speed) * finalMultiplier);
        }
    }

    public void BreakVisualLink()
    {
        isBreaking = true;
        timerBreakingTime = breakingTime;
        speed *= 2;
        gradientAlphaKeys[1].alpha = 0f;
        gradientAlphaKeys[2].alpha = 0f;
    }

    void BreakingProcess()
    {
        if (isBreaking)
        {
            timerBreakingTime -= Time.deltaTime;
            if (timerBreakingTime > 0)
            {
                speed *= 1 + Time.deltaTime * 2;
                waveHeight *= 1 + Time.deltaTime * 2;

                float ratio = timerBreakingTime / breakingTime;
                MoveGradient(ratio);
                gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);
                lineRenderer.colorGradient = gradient;
            }
            else
            {
                gameObject.SetActive(false);
                speed = ordinarySpeed;
                waveHeight = ordinaryWaveHeight;
                isBreaking = false;
            }
        }
    }
}
