using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the fog of war.
/// Generates the texture with the alpha "holes" for visable units.
/// Will also disable other players unity that are no longer visable. (todo)
/// </summary>
public class FogOfWarManager : MonoBehaviour
{
    #region Private
    /// <summary>
    /// The size of the texture in BOTH x and y.
    /// Should be a power of 2.
    /// </summary>
    [SerializeField]
    private int _textureSize = 256;
    [SerializeField]
    private Color _fogOfWarColor;
    [SerializeField]
    private LayerMask _fogOfWarLayer;

    private Texture2D _texture;
    private Color[] _pixels;
    private List<Revealer> _revealers;
    private int _pixelsPerUnit;
    private Vector2 _centerPixel;

    private static FogOfWarManager _instance;
    #endregion

    #region Public
    /// <summary>
    /// Note this is NOT a singleton!
    /// This just needs to be globally accessable AND still be a MonoBehaviour.
    /// </summary>
    public static FogOfWarManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;

        if (!SystemInfo.supportsComputeShaders)
        {
            Debug.LogWarning("No Compute Shader support!");
        }

        var renderer = GetComponent<Renderer>();
        Material fogOfWarMat = null;
        if (renderer != null)
        {
            fogOfWarMat = renderer.material;
        }

        if (fogOfWarMat == null)
        {
            Debug.LogError("Material for Fog Of War not found!");
            return;
        }

        _texture = new Texture2D(_textureSize, _textureSize, TextureFormat.RGBA32, false);
        _texture.wrapMode = TextureWrapMode.Clamp;

        _pixels = _texture.GetPixels();
        ClearPixels();

        fogOfWarMat.mainTexture = _texture;

        _revealers = new List<Revealer>();

        _pixelsPerUnit = Mathf.RoundToInt(_textureSize / transform.lossyScale.x);

        _centerPixel = new Vector2(_textureSize * 0.5f, _textureSize * 0.5f);
    }

    public void RegisterRevealer(Revealer revealer)
    {
        _revealers.Add(revealer);
    }

    private void ClearPixels()
    {
        for (var i = 0; i < _pixels.Length; i++)
        {
            _pixels[i] = _fogOfWarColor;
        }
    }

    /// <summary>
    /// Sets the pixels in _pixels to clear a circle.
    /// </summary>
    /// <param name="originX">in pixels</param>
    /// <param name="originY">in pixels</param>
    /// <param name="radius">in unity units</param>
    private void CreateCircle(int originX, int originY, int radius)
    {
        for (var y = -radius * _pixelsPerUnit; y <= radius * _pixelsPerUnit; ++y)
        {
            for (var x = -radius * _pixelsPerUnit; x <= radius * _pixelsPerUnit; ++x)
            {
                if (x * x + y * y <= (radius * _pixelsPerUnit) * (radius * _pixelsPerUnit))
                {
                    _pixels[(originY + y) * _textureSize + originX + x] = new Color(0, 0, 0, 0);
                    
                }
            }
        }
    }

    private void Update()
    {
        ClearPixels();

        foreach (var revealer in _revealers)
        {
            // should do a raycast from the revealer to the camera.
            var screenPoint = Camera.main.WorldToScreenPoint(revealer.transform.position);
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, _fogOfWarLayer.value))
            {
                // Translates the revealer to the center of the fog of war.
                // This way the position lines up with the center pixel and can be converted easier.
                var translatedPos = hit.point - transform.position;

                var pixelPosX = Mathf.RoundToInt(translatedPos.x * _pixelsPerUnit + _centerPixel.x);
                var pixelPosY = Mathf.RoundToInt(translatedPos.z * _pixelsPerUnit + _centerPixel.y);
                CreateCircle(pixelPosX, pixelPosY, revealer.radius);
            }
        }


        _texture.SetPixels(_pixels);
        _texture.Apply(false);
    }
}