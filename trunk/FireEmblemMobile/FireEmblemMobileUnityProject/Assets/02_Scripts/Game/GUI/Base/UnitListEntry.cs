using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Base
{
    public class UnitListEntry : MonoBehaviour
    {
        private UnitBP unitBp;
        [SerializeField] private Image sprite = default;
        [SerializeField] private TextMeshProUGUI nameText = default;

        public void SetUnit(UnitBP unitBp)
        {
            this.unitBp = unitBp;
            sprite.sprite = unitBp.visuals.CharacterSpriteSet.MapSprite;
            nameText.text = unitBp.name;
        }
    }
}
