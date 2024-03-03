using System.Collections.Generic;
using Game.DataAndReferences.Data;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.SerializedData
{
    [System.Serializable]
    public class SkillData
    {
        [SerializeField]
        public List<string> skillIds;
        [SerializeField]
        public List<int> skillLevels;

        public SkillData(SkillManager unitSkillManager)
        {
            skillIds = new List<string>();
            skillLevels = new List<int>();
            foreach (var skill in unitSkillManager.Skills)
            {
                skillIds.Add(skill.Name);
                skillLevels.Add(skill.Level);
            }
        }

        public void Load(SkillManager skillManager)
        {
            int skillIndex = 0;
            foreach (var skillId in skillIds)
            {
                if (skillId != "")
                {
                    Skill skill = GameBPData.Instance.GetSkill(skillId);
                    skill.Level = skillLevels[skillIndex]; 
                    skillManager.Skills.Add(skill);
                }

                skillIndex++;
                

            }
        }
    }
}