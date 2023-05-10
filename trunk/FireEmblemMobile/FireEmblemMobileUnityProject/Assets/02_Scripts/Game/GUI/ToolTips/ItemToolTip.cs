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


    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public Image itemIcon;
  
    [SerializeField]
    private RectTransform rectTransform;
    private Unit itemOwner;
    public LayoutElement frame;
    
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
        frame.enabled = false;
        frame.enabled = true;
        if(rectTransform==null)
            rectTransform = GetComponent<RectTransform>();
        int headerLength = headerText.text.Length;
        int contentLength = descriptionText.text.Length;
        layoutElement.enabled =
            (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;


        float pivotX = 0;//position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(Item item, string header, string description, Sprite icon, Vector3 position)
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
        Unit human = Player.Instance.Party.ActiveUnit;
        

        descriptionText.text = description;
        itemIcon.sprite = icon;
        rectTransform.anchoredPosition = position+ new Vector3(0,280,0);
        UpdateTextWrap(position);

    }
}
