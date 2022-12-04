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
    public void Show(EquipableItem equipable, bool selected=false)
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
            Debug.Log("Equipable: "+equipable+name);
            image.sprite = equipable.Sprite;
            image.color = normalColor;
            if (equipable is Relic relic)
            {
                if(relic.slotCount>0)
                    slot.gameObject.SetActive(true);
                if (relic.GetGem(0) != null)
                {
                    Debug.Log("Relic with Gem");
                    gem.sprite = relic.GetGem(0).Sprite;
                    gem.enabled = true;
                }
                else
                {
                    Debug.Log("Relic without Gem");
                    gem.sprite = null;
                    gem.enabled = false;
                }
            }
            else
            {
                Debug.Log("No Relic");
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
}