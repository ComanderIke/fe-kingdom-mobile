using System;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SelectableCharacterUI : MonoBehaviour
    {
        public Unit unit;
        [SerializeField]private UIUnitIdleAnimation unitUIIdleAnimation;
        public event Action<SelectableCharacterUI> onClicked;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Image checkMark;
        [SerializeField] private Image frame;

        public void SetCharacter(Unit unit)
        {
            this.unit = unit;
            unitUIIdleAnimation.Show(unit);
        }

        public void Select()
        {
            checkMark.gameObject.SetActive(true);
            frame.color = selectedColor;
        }
        public void Deselect()
        {
            checkMark.gameObject.SetActive(false);
            frame.color = normalColor;
        }
        public void Clicked()
        {
            onClicked?.Invoke(this);
        }
    }
}