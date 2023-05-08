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

    [SerializeField] private UIEquipmentController equipmentController;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public Image itemIcon;

    public Button useButton;
    public TextMeshProUGUI useButtonText;
    public Button dropButton;
    private Item item;
    [SerializeField]
    private RectTransform rectTransform;
    private Unit itemOwner;
    public LayoutElement frame;

    [SerializeField] UICharacterViewController charView;
    // Start is called before the first frame update
  

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor)
        {
           UpdateTextWrap(transform.position);
        }
    }

    void RelicEquipClicked(Relic relic)
    {
      
        Unit human =Player.Instance.Party.ActiveUnit;
        if (equipmentController.selectedSlot == null)
        {
            charView.Show(human);
            equipmentController.HighlightRelicSlots();
        }
        else
        {
            EquipRelicOnSelectedSlot(human, relic);
        }
        
    }

    void EquipRelicOnSelectedSlot(Unit human, Relic relic)
    {
        if (equipmentController.selectedSlotNumber == 1)
        {
            if (human.HasEquipped(relic))
            {
                human.UnEquip((relic));
                Player.Instance.Party.Convoy.AddItem(item);
            }
            else
            {
                if(human.EquippedRelic1!=null)
                    Player.Instance.Party.Convoy.AddItem(human.EquippedRelic1);
                human.Equip((relic), equipmentController.selectedSlotNumber);
                Player.Instance.Party.Convoy.RemoveItem(item);
            }
        }
        else if (equipmentController.selectedSlotNumber == 2)
        {
            if (human.HasEquipped(relic))
            {
                human.UnEquip((relic));
                Player.Instance.Party.Convoy.AddItem(item);
            }
            else
            {
                if(human.EquippedRelic2!=null)
                    Player.Instance.Party.Convoy.AddItem(human.EquippedRelic2);
                human.Equip((relic), equipmentController.selectedSlotNumber);
                Player.Instance.Party.Convoy.RemoveItem(item);
            }
        }
    }
    public void UseClicked()
    {
        Debug.Log("Use Item Clicked TODO dont Remove here also just in ItembaseClass!");
        
        if (item is Relic eitem)
        {
           RelicEquipClicked(eitem);
        }
        else 
        {
            if (item is ConsumableItem cItem)
            {
                cItem.Use(Player.Instance.Party.ActiveUnit, Player.Instance.Party.Convoy);
            }
            else
            {
                Player.Instance.Party.Convoy.RemoveItem(item);
            }
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
        
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(Item item, string header, string description, Sprite icon, Vector3 position)
    {
        this.item = item;
        useButton.gameObject.SetActive(true);
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
            if (item is ConsumableItem uitem)
            {
                useButton.interactable = true;
            }
            else
            {
                useButton.interactable = false;
                useButton.gameObject.SetActive(false);
            }
            
        }

        if (human.HasEquipped(item))
        {
            if (item is Weapon)
            {
       
                useButton.interactable = false;
                useButtonText.text = "Equipped";
                dropButton.gameObject.SetActive(false);
            }

            if (item is Relic)
            {
         
                useButton.interactable = true;
                useButtonText.text = "Unequip";
                dropButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // useButton.interactable = 
            dropButton.gameObject.SetActive(true);
            useButtonText.text = item is EquipableItem ? "Equip" : "Use";
        }

        descriptionText.text = description;
        itemIcon.sprite = icon;
        rectTransform.anchoredPosition = position+ new Vector3(0,200,0);
        UpdateTextWrap(position);

    }
}
