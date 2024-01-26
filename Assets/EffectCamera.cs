using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EffectCamera : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Vector2Int _res;

    private RenderTexture _renderTexture;
    [SerializeField] private Material _mat;

    // Start is called before the first frame update
    void Start()
    {
        //_cam.depthTextureMode = DepthTextureMode.Depth;
        _renderTexture = new RenderTexture(_res.x, _res.y, 24);
        RenderTexture.active = _renderTexture;
    }
    
    void Update () {
    }

    private void OnPreRender()
    {
        _cam.targetTexture = _renderTexture;
    }

    void OnPostRender()
    {
        _cam.targetTexture = null;
        Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _renderTexture);
    }

    /*void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }*/
}
