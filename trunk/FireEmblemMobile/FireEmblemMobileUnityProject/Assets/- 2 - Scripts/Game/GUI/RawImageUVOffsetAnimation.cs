using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RawImageUVOffsetAnimation : MonoBehaviour
{
    private RawImage rawImage;
    public float xSpeed = 1;
    public float ySpeed = 0;
    public float originalWidth = 50;
    public float originalHeight = 70;
    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        Rect uvRect = rawImage.uvRect;
        uvRect.x = 1;
        uvRect.y = 1;
        rawImage.uvRect = uvRect;
    }
    void OnEnable()
    {
        rawImage = GetComponent<RawImage>();
    }
    public void UpdateBounds()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect uvRect = rawImage.uvRect;
        uvRect.width = rectTransform.rect.width / originalWidth;
        uvRect.height = rectTransform.rect.height / originalHeight;
        rawImage.uvRect = uvRect;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateBounds();
        Rect uvRect = rawImage.uvRect;
        uvRect.x += Time.deltaTime*xSpeed;
        uvRect.y += Time.deltaTime*ySpeed;
        rawImage.uvRect = uvRect;
    }
}
