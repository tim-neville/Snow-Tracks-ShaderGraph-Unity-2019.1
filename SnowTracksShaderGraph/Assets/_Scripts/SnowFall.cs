using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFall : MonoBehaviour
{
    public CustomRenderTexture _splatMap;

    public Material _snowFallMat;
    public Material _snowTracksMaterial;

    [Range(0.001f, 0.1f)]
    public float _flakeAmount;

    [Range(0f, 1f)]
    public float _flakeOpacity;

    private void Start()
    {
        _splatMap = (CustomRenderTexture) _snowTracksMaterial.GetTexture("_SplatMap");
        _snowFallMat.SetTexture("_SplatMap", _splatMap);
    }

    void Update()
    {
        _snowFallMat.SetFloat("_FlakeAmount", _flakeAmount);
        _snowFallMat.SetFloat("_FlakeOpacity", _flakeOpacity);

    }
}
