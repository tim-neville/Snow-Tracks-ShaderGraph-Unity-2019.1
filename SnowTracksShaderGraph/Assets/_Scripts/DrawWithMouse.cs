using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public Camera _camera;
    public CustomRenderTexture _splatmap;

    public Material _drawMaterial;
    public Material _snowMaterial;

    private RaycastHit _hit;

    [Range(1, 500)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;

    //private List<CustomRenderTextureUpdateZone> updateZones;

    void Start()
    {
        _drawMaterial.SetVector("_DrawColor", Color.red);

        //_splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);

        _snowMaterial.SetTexture("_SplatMap", _splatmap);
        _drawMaterial.SetTexture("_SplatMap", _splatmap);

        //_splatmap.GetUpdateZones(updateZones);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out _hit, 1000f))
            {
                //The object needs a MESH COLLIDER in order to get texture Coords
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_BrushStrength", _brushStrength);
                _drawMaterial.SetFloat("_BrushSize", _brushSize);

            
                // updateZones[0].updateZoneCenter = _hit.textureCoord;
                // updateZone.updateZoneSize = Vector3.one * _brushSize;

                // RenderTexture temp = RenderTexture.GetTemporary(1024, 1024, 0, RenderTextureFormat.ARGBFloat);

                // Graphics.Blit(_splatmap, temp);
                // Graphics.Blit(temp, _splatmap, _drawMaterial);

                // _snowMaterial.SetTexture("_SplatMap", _splatmap);

                // RenderTexture.ReleaseTemporary(temp);
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.StretchToFill, false, 1);
    }
}
