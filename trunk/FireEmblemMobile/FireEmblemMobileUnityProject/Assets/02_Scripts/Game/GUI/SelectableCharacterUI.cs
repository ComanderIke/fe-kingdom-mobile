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
        [SerializeField] private GameObject selected;
        [SerializeField] private GameObject added;
        [SerializeField] private Image frame;
        [SerializeField] private Button button;
        [SerializeField] private Color notUnlockedColor;
        [SerializeField] private GameObject notUnlockedGo;
        [SerializeField] private Image unitImage;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject nameGO;
        private bool isSelected = false;
        public void SetCharacter(Unit unit, bool locked)
        {
            this.unit = unit;
            if (locked)
            {
                notUnlockedGo.SetActive(true);
                unitImage.color = notUnlockedColor;
                SetInteractable(false);
                nameGO.SetActive(false);
                //unitUIIdleAnimation.gameObject.SetActive(false);
            }
            text.SetText(unit.Name);
            //text.gameObject.SetActive(false);
            
            unitUIIdleAnimation.Show(unit);
        }

        public void Add()
        {
            added.gameObject.SetActive(true);

        }

        public void Remove()
        {
            added.gameObject.SetActive(false);
        }
        public void Select()
        {
            selected.gameObject.SetActive(true);
            isSelected = true;
        }
        public void Deselect()
        {
            selected.gameObject.SetActive(false);
            isSelected = false;
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