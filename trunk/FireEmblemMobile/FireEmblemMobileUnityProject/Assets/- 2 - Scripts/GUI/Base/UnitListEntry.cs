using Assets.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI.Base
{
    public class UnitListEntry : MonoBehaviour
    {
        private Unit unit;
        [SerializeField] private Image sprite = default;
        [SerializeField] private TextMeshProUGUI nameText = default;

        public void SetUnit(Unit unit)
        {
            this.unit = unit;
            sprite.sprite = unit.CharacterSpriteSet.MapSprite;
            nameText.text = unit.Name;
        }
    }
}
