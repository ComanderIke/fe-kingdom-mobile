using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Skills;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.Humans
{
    [System.Serializable]
    public class SkillManager : ICloneable
    {
        public Action<int> SkillPointsUpdated;
        [SerializeField] public List<Skill> Skills;
        [SerializeField] public Skill Favourite;
        [SerializeField] private int skillPoints = 1;

        public int SkillPoints
        {
            get { return skillPoints; }
            set
            {
                skillPoints = value;
                Debug.Log("SkillPoints value changed");
                SkillPointsUpdated?.Invoke(skillPoints);
            }
        }

        [FormerlySerializedAs("SkillTree")] [SerializeField]
        public SkillTree[] SkillTrees;


        public SkillManager(SkillManager sm)
        {
            Skills = new List<Skill>();
            foreach (var skill in sm.Skills)
            {
                Skills.Add(skill);
            }
        }


        public Skill GetSkill(string name)
        {
            return Skills.Find(s => s.name == name);
        }

        public object Clone()
        {
            var clone = (SkillManager)MemberwiseClone();
            clone.Skills = new List<Skill>();
            foreach (Skill skill in Skills)
            {
                clone.Skills.Add(skill);
            }

            return clone;
        }

        public void LearnSkill(Skill clickedSkill)
        {
            if (SkillPoints >= 1)
            {
                if (Skills.Contains(clickedSkill))
                {
                    Skills.Find(s => clickedSkill.name == s.name).Level++;
                }
                else
                {
                    Skills.Add((clickedSkill));
                    clickedSkill.Level++;
                }

                SkillPoints--;
            }
            else
            {
                Debug.LogError("Not enough SkillPoints to learn new skill!");
            }
        }
        public void UpdateSkillState(SkillTreeEntry skillEntry)
        {
            SkillTreeEntry entry = null;
            foreach (var skilltree in SkillTrees)
            {
                entry = skilltree.skillEntries.Find(se => se.skill == skillEntry.skill);
            }

            Debug.Log(skillEntry.skill.name);
            entry.SkillState = SkillState.NotLearnable;
        
            if (Skills.Contains(entry.skill))
            {
                entry.SkillState = SkillState.Learned;
                if ( entry.skill.Level ==  entry.skill.MaxLevel)
                    entry.SkillState = SkillState.Maxed;
            }
            entry.SkillState = SkillState.Learnable;
        }
        public void Init()
        {
            for(int i=0; i < SkillTrees.Length; i++)
            {
                SkillTrees[i].Init();
            }

        }
    }
}