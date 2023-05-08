using Game.GameActors.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class SmithingSlot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color emptyColor;
    [SerializeField] Color normalColor;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Image backGround;
    [SerializeField] Color bGNormalColor;
    [SerializeField] Color bGSelectedColor;
    [SerializeField] private Image gem;
    [SerializeField] private GameObject slot;
    private bool selected = false;
    private EquipableItem equipable;
    public void Show(EquipableItem equipable, bool selected=false)
    {
        this.selected = selected;
        this.equipable = equipable;
       UpdateUI();
      
    }

    void UpdateUI()
    {
        if (equipable == null)
        {
            image.color = emptyColor;
            image.sprite = emptySprite;
            slot.gameObject.SetActive(false);
            gem.sprite = null;
            gem.enabled = false;
            
        }
        else
        {
            image.sprite = equipable.Sprite;
            image.color = normalColor;
            if (equipable is Relic relic)
            {
                if(relic.slotCount>0)
                    slot.gameObject.SetActive(true);
                if (relic.GetGem(0) != null)
                {
                    gem.sprite = relic.GetGem(0).Sprite;
                    gem.enabled = true;
                }
                else
                {
                    gem.sprite = null;
                    gem.enabled = false;
                }
            }
            else
            {
                slot.gameObject.SetActive(false);
                gem.sprite = null;
                gem.enabled = false;
            }
        }

        if (selected)
        {
            backGround.color = bGSelectedColor;
        }
        else
        {
            backGround.color = bGNormalColor;
        }
    }
    public void Hide()
    {
        throw new System.NotImplementedException();
    }

    public void Select()
    {
        selected = true;
        UpdateUI();
    }

    public void Deselect()
    {
        selected = false;
        UpdateUI();
    }

    public void Highlight()
    {
        Debug.Log("Highlight Slot somehow");
    }
}