using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
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

    public Button useButton;
    public TextMeshProUGUI useButtonText;
    public Button dropButton;
    private Item item;
    private RectTransform rectTransform;
    private Human itemOwner;
    public LayoutElement frame;
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

    public void UseClicked()
    {
        Debug.Log("Use Item Clicked TODO dont Remove here also just in ItembaseClass!");
        if (item is EquipableItem eitem)
        {
            Human human =(Human) Player.Instance.Party.ActiveUnit;

            if (human.HasEquipped(eitem))
            {
                human.UnEquip((eitem));
                Player.Instance.Party.Convoy.AddItem(item);
            }
            else
            {
                human.Equip((eitem));
                Player.Instance.Party.Convoy.RemoveItem(item);
            }
        }
        else
        {
            Player.Instance.Party.Convoy.RemoveItem(item);
        }
        gameObject.SetActive(false);
    }

    public void DropClicked()
    {
        Debug.Log("Drop Item Clicked");
        if (Player.Instance.Party.Convoy.ContainsItem(item))
        {
            Player.Instance.Party.Convoy.RemoveItem(item);
        }

        gameObject.SetActive(false);
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

        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(Item item, string header, string description, Sprite icon, Vector3 position)
    {
        this.item = item;
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }
        Human human = (Human)Player.Instance.Party.ActiveUnit;
        if (item is EquipableItem eitem)
        {
            
            if (human.CanEquip(eitem))
            {
                useButton.interactable = true;
            }
            else
            {
                useButton.interactable = false;
            }
        }
        else
        {
            useButton.interactable = true;
        }

        if (human.HasEquipped(item))
        {
            useButtonText.text = "Unequip";
            dropButton.gameObject.SetActive(false);
        }
        else
        {
            // useButton.interactable = 
            dropButton.gameObject.SetActive(true);
            useButtonText.text = item is EquipableItem ? "Equip" : "Use";
        }

        descriptionText.text = description;
        itemIcon.sprite = icon;
        
        UpdateTextWrap(position);

    }
}
