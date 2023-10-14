using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ItemToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public UIConvoyItemController itemIcon;
  
    [SerializeField]
    private RectTransform rectTransform;
    private Unit itemOwner;
    
    
    // Start is called before the first frame update
  

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
        if(rectTransform==null)
            rectTransform = GetComponent<RectTransform>();
        float pivotX = 0;//position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(StockedItem item, Vector3 position)
    {
        headerText.text = item.item.Name;
        descriptionText.text = item.item.Description;
        itemIcon.SetValues(item, 0);
        rectTransform.anchoredPosition = position+ new Vector3(0,280,0);
        UpdateTextWrap(position);
    }
}
