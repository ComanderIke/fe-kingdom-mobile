using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class ChooseDifficultyButtonUI : MonoBehaviour
    {
        [SerializeField] public DifficultyProfile profile;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private new TextMeshProUGUI name;
        public GameObject linePrefab;
        public Transform lineContainer;
        [SerializeField] private Image selectedBorder;
        public event Action<ChooseDifficultyButtonUI> OnClick;
        [SerializeField] private Vector3 selectedScale;
        public void Clicked()
        {
            OnClick?.Invoke(this);
            UpdateUI();
        }
        public void Deselect()
        {
            selectedBorder.enabled = false;
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), .1f).setEaseInQuad();
            
        }

        public void Select()
        {
            LeanTween.scale(gameObject, selectedScale, .1f).setEaseOutQuad();
            selectedBorder.enabled = true;
        }

        public void Hide()
        {
            Deselect();
            gameObject.SetActive(false);
        }

        void UpdateUI()
        {
            lineContainer.DeleteAllChildrenImmediate();
            name.text = profile.name;
            description.text = profile.Description;
            icon.sprite = profile.Icon;
            foreach (DifficultyVariable variable in profile.GetDifficultyVariables())
            {
                var line = GameObject.Instantiate(linePrefab, lineContainer);
                bool upg = variable.textStyle == DifficultyVariableStyle.Increase;
                bool downgrade = variable.textStyle == DifficultyVariableStyle.Decrease;
                line.GetComponent<UISkillEffectLine>().SetValues(variable.label, variable.value, upg,downgrade);
            }
            
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        public void SetProfile(DifficultyProfile difficulty1)
        {
            this.profile = difficulty1;
            gameObject.SetActive(true);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
