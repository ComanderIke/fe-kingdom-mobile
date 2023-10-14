using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class SkillActivatedUI : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject skillPlaceHolder;
        [SerializeField] private MMFeedbacks skillActivationFeedback;
        [SerializeField] private GameObject passiveSkillPrefab;
        [SerializeField] private GameObject activeSkillPrefab;
        [SerializeField] private GameObject combatSkillPrefab;
        public void SetSkill(Unit activater, Skill skill)
        {
            this.nameText.text = skill.Name;
           // icon.sprite = skill.GetIcon();
            InstantiateSkill(activater, skill);
            MonoUtility.InvokeNextFrame(()=>skillActivationFeedback.PlayFeedbacks());
           
        }
        void InstantiateSkill(Unit activater, Skill skill)
        {
            var prefab = passiveSkillPrefab;
            if (skill.activeMixins.Count > 0)
                prefab = activeSkillPrefab;
            else if (skill.CombatSkillMixin != null)
                prefab = combatSkillPrefab;
            var go = Instantiate(prefab, skillPlaceHolder.transform);
            var skillUI = go.GetComponent<SkillUI>();
            skillUI.SetSkill(skill, false, activater.Blessing!=null);
        }
    }
}
