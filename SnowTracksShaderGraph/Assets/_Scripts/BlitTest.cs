using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitTest : MonoBehaviour
{
    public Texture2D _baseTexture;

    public RenderTexture _splatmap;
    public Material _drawMaterial;

    public Material _planeMaterial;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
            //RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, _splatmap.format);

            Graphics.Blit(_baseTexture, _splatmap, _drawMaterial);
            //Graphics.Blit(temp, _splatmap);

            _planeMaterial.SetTexture("_Splat", _splatmap);

            //RenderTexture.ReleaseTemporary(temp);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Graphics.Blit(_baseTexture, _splatmap);

            _planeMaterial.SetTexture("_Splat", _splatmap);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);

            Graphics.Blit(null, _splatmap);

            _planeMaterial.SetTexture("_Splat", _splatmap);
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatmap, ScaleMode.ScaleToFit, false, 1);
    }
}
