using System;
using Game.GameActors.Units.Skills.Base;
using Game.GameMechanics;
using Game.GUI;
using Game.GUI.ToolTips;
using Game.Utility;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameActors.Units.Skills.Active
{
    public class CurseSkillButtonUI : MonoBehaviour
    {
        [SerializeField] private Image curseIcon;
        [SerializeField] private GameObject skillButtonPrefab;
        [SerializeField] private GameObject activeSkillButtonPrefab;
        [SerializeField] private GameObject combatSkillButtonPrefab;
        [SerializeField] private MMF_Player feedbacks;
        public event Action OnClicked;
        private Curse curse;
        public void Clicked()
        {
            OnClicked?.Invoke();
        }

        public void TooltipClicked()
        {
            ToolTipSystem.Show(curse, transform.position);
        }
        public void Show(Curse curse)
        {

            this.curse = curse;
            curseIcon.sprite = curse.Icon;
            if (curse.OverwrittenSkill != null)
            {
                InstantiateSkill(curse.OverwrittenSkill);
            }

            MonoUtility.InvokeNextFrame(() =>
            {
                if (this == null)
                    return;
                if (gameObject == null)
                    return;
                if (gameObject!=null&&this!=null&&feedbacks != null)
                    feedbacks.PlayFeedbacks();
            });
        }
        void InstantiateSkill(Skill skill)
        {
            var prefab = skillButtonPrefab;
            if (skill.activeMixins.Count > 0)
                prefab = activeSkillButtonPrefab;
            else if (skill.CombatSkillMixin != null)
                prefab = combatSkillButtonPrefab;
            var go = Instantiate(prefab, transform);
            go.transform.SetSiblingIndex(0);
            var rectTransform = GetComponent<RectTransform>();
            go.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;
            var skillUI = go.GetComponent<SkillUI>();
            skillUI.SetSkill(skill, false, false, false, false,false, false);
        }
    }
}