using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLights : MonoBehaviour
{
    public Light redLight;
    public Light blueLight;

    public MeshRenderer redEmissive;
    public MeshRenderer blueEmissive;

    [ColorUsageAttribute(true, true)]
    public Color redColor;
    [ColorUsageAttribute(true, true)]
    public Color blueColor;

    public float flashingSpeed = 5f;

    public float intensityBase = 1f;

    void Update()
    {
        float sineRed = Mathf.Sin(Time.time * flashingSpeed);
        float sineBlue = Mathf.Sin(-Time.time * flashingSpeed);

        redLight.intensity = sineRed + intensityBase;
        blueLight.intensity = sineBlue + intensityBase;

        redEmissive.material.SetColor("_EmissionColor", new Vector4(redColor.r, redColor.g, redColor.b, 0) * sineRed * 2);
        blueEmissive.material.SetColor("_EmissionColor", new Vector4(blueColor.r, blueColor.g, blueColor.b, 0) * sineBlue * 2);
    }
}
