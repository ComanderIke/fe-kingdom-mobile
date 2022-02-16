using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;

   
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public Image itemIcon;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor)
        {
           UpdateTextWrap(transform.position);
        }
    }

    void UpdateTextWrap(Vector3 position)
    {
        int headerLength = headerText.text.Length;
        int contentLength = descriptionText.text.Length;
        layoutElement.enabled =
            (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;

        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    public void SetValues(string header, string description, Sprite icon, Vector3 position)
    {
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }
        
        descriptionText.text = description;
        itemIcon.sprite = icon;
        
        UpdateTextWrap(position);

    }
}
