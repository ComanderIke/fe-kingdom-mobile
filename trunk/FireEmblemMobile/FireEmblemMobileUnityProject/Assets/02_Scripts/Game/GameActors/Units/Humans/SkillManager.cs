using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Game.GameActors.Units.Humans
{
    [System.Serializable]
    public class SkillManager : ICloneable
    {
        public Action<int> SkillPointsUpdated;
        [SerializeField] private List<SkillBp> startSkills;
        private List<Skill> skills;
        private Unit unit;
        public int maxSkillCount = 5;

        public void AddStartSkills()
        {
            if (startSkills != null)
            {
                foreach (var skillbp in startSkills)
                {
                    Skills.Add(skillbp.Create());
                    Debug.Log("Create Start Skill");
                }
            }
        }
        public List<Skill> Skills
        {
            get
            {
                if (skills == null)
                {
                    skills = new List<Skill>();
                   
                }

                return skills;
            }
        }
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
            get { return Skills.FindAll(s => s.activeMixins.Count>0); }
        }

       // [SerializeField]
     // [HideInInspector]
      //  public SkillTree[] SkillBuildTrees;

         

        public SkillManager(SkillManager sm)
        {
            startSkills = sm.startSkills;
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
        public void LearnSkill(Skill skill)
        {
            skill.BindSkill(unit);
            Skills.Add(skill);
            OnSkillsChanged?.Invoke();
        }

        
     
        public void Init(Unit u)
        {
            this.unit = u;
            foreach (var skill in Skills)
            {
                skill.BindSkill(u);
            }
        }

        public void RemoveSkill(Skill skill)
        {
            skills.Remove(skill);
            OnSkillsChanged?.Invoke();
        }

        public bool IsFull()
        {
            return skills.Count >= maxSkillCount;
        }

        public void RemoveRandomSkill()
        {
            RemoveSkill(skills[UnityEngine.Random.Range(0, skills.Count)]);
        }

        public event Action OnSkillsChanged;

        public Blessing GetBlessing()
        {
            var first = Skills.First(s => s is Blessing);
            if(first!=null)
                return (Blessing)first;
            return null;
        }
    }
}