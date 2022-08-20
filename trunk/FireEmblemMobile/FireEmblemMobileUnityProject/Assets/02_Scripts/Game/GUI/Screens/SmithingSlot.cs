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
    public void Show(EquipableItem equipable, bool selected=false)
    {
        if (equipable == null)
        {
            image.color = emptyColor;
            image.sprite = emptySprite;
        }
        else
        {
            image.sprite = equipable.Sprite;
            image.color = normalColor;
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