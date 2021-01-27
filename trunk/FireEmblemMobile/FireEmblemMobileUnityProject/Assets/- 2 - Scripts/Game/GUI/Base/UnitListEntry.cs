using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Base
{
    public class UnitListEntry : MonoBehaviour
    {
        private Unit unit;
        [SerializeField] private Image sprite = default;
        [SerializeField] private TextMeshProUGUI nameText = default;

        public void SetUnit(Unit unit)
        {
            this.unit = unit;
            sprite.sprite = unit.visuals.CharacterSpriteSet.MapSprite;
            nameText.text = unit.name;
        }
    }
}
