using System;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class ResurrectionUI : MonoBehaviour
    {
        [SerializeField] private Image faceSprite;
        [SerializeField] private TextMeshProUGUI nameAndLevelText;
        public event Action<Unit> OnClicked;
        private Unit unit;
        public void SetUnit(Unit unit)
        {
            this.unit = unit;
            faceSprite.sprite = unit.FaceSprite;
            nameAndLevelText.text = unit.Name + " Lv." + unit.ExperienceManager.Level;
        }

        public void Clicked()
        {
            OnClicked?.Invoke(unit);
        }
    }
}