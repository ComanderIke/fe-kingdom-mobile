using Game.GameActors.Units;
using Game.GameActors.Units.Skills.Base;
using Game.Utility;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

namespace Game.GUI
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
            this.nameText.text = "<shinier><bounce>"+skill.Name;
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
            bool canAffordHPCost = skill.FirstActiveMixin != null && activater.Hp > skill.FirstActiveMixin.GetHpCost(skill.level) || skill.CombatSkillMixin != null && activater.Hp > skill.CombatSkillMixin.GetHpCost(skill.level);
            bool hasUses=skill.FirstActiveMixin != null &&skill.FirstActiveMixin.Uses>0 || skill.CombatSkillMixin != null && skill.CombatSkillMixin.Uses>0;

            skillUI.SetSkill(skill, false, activater.Blessing!=null,canAffordHPCost,hasUses);
        }
    }
}
