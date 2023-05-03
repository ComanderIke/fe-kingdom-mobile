using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Skills;

using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.Humans
{
    [System.Serializable]
    public class SkillManager : ICloneable
    {
        public Action<int> SkillPointsUpdated;
        [SerializeField] private List<SkillBP> startSkills;
        private List<Skill> skills;
      


        public List<Skill> Skills
        {
            get
            {
                if (skills == null)
                {
                    skills = new List<Skill>();
                    if(startSkills!=null)
                        foreach (var skillbp in startSkills)
                        {
                            skills.Add(skillbp.Create());
                        }
                }

                return skills;
            }
        }
        public Skill Favourite;
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

        public List<Skill> ActiveSkills
        {
            get { return skills.FindAll(s => !(s is PassiveSkill)); }
        }

       // [SerializeField]
     // [HideInInspector]
      //  public SkillTree[] SkillBuildTrees;

         

        public SkillManager(SkillManager sm)
        {
            foreach (var skill in sm.Skills)
            {
                Skills.Add(skill);
            }
        }


        public Skill GetSkill(string name)
        {
            return Skills.Find(s => s.Name == name);
        }

        public object Clone()
        {
            var clone = (SkillManager)MemberwiseClone();
            foreach (Skill skill in Skills)
            {
                clone.Skills.Add(skill);
            }

            return clone;
        }

        public void LearnSkill(SkillTreeEntry clickedSkill)
        {
            if (SkillPoints >= 1)
            {
                if (Skills.Contains(clickedSkill.Skill))
                {
                    Skills.Find(s => clickedSkill.Skill.Name == s.Name).Level++;
                }
                else
                {
                    Skills.Add((clickedSkill.Skill));
                    
                    clickedSkill.Skill.Level++;

                }

                SkillPoints--;
            }
            else
            {
                Debug.LogError("Not enough SkillPoints to learn new skill!");
            }
        }
      
        public void Init()
        {
            // if(SkillBuildTrees!=null)
            //     foreach(var skillTree in SkillBuildTrees)
            //     {
            //         skillTree.Init();
            //     }

        }
    }
}