using System;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LostGrace
{
    public class SelectableCharacterUI : MonoBehaviour
    {
        [FormerlySerializedAs("unitBp")] public Unit unit;
        [SerializeField]private UIUnitIdleAnimation unitUIIdleAnimation;
        public event Action<SelectableCharacterUI> onClicked;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Image checkMark;
        [SerializeField] private Image frame;
        [SerializeField] private Button button;
        [SerializeField] private Color notUnlockedColor;
        [SerializeField] private GameObject notUnlockedGo;
        [SerializeField] private Image unitImage;
        [SerializeField] private TextMeshProUGUI text;
        private bool selected = false;
        public void SetCharacter(Unit unit, bool locked)
        {
            this.unit = unit;
            if (locked)
            {
                notUnlockedGo.SetActive(true);
                unitImage.color = notUnlockedColor;
                SetInteractable(false);
            }
            text.gameObject.SetActive(false);
            unitUIIdleAnimation.Show(unit);
        }

        public void Select()
        {
            checkMark.gameObject.SetActive(true);
            frame.color = selectedColor;
            selected = true;
        }
        public void Deselect()
        {
            checkMark.gameObject.SetActive(false);
            frame.color = normalColor;
            selected = false;
        }
        public void Clicked()
        {
            onClicked?.Invoke(this);
        }

        public bool IsSelected()
        {
            return selected;
        }

        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }
}