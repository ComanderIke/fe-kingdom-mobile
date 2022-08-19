using Game.GameActors.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class SmithingSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    public void Show(EquipableItem equipable)
    {
        image.sprite = equipable.Sprite;
    }
}