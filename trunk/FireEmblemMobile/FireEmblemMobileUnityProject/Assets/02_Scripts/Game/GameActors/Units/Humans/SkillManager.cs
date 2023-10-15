﻿using System;
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
        [SerializeField] private BlessingBP startBlessing;
        private List<Skill> skills;
        private Unit unit;
        private Blessing blessing;
        public int maxSkillCount = 4;
        

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
            Debug.Log("Add Start Skills/Blessings");
            if (startBlessing != null)
                blessing = (Blessing)startBlessing.Create();
            Debug.Log("Blessing: "+blessing);

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

            startBlessing = sm.startBlessing;
            blessing = sm.blessing;

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

            clone.blessing = blessing;
            return clone;
        }
        public void LearnSkill(Skill skill)
        {
            if (skill is Blessing blessing)
            {
                this.blessing = blessing;
            }
            else
            {
                skill.BindSkill(unit);
                Skills.Add(skill);
            }

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
          //  Debug.Log("Return Blessing: "+blessing);
            return blessing;
        }

        public void UpgradeSkill(bool randomly)
        {
            if (randomly)
            {
                var upgradeableSkills = skills.Select(s => s.Upgradable());
                int randomIndex = UnityEngine.Random.Range(0, upgradeableSkills.Count());
                if (skills[randomIndex].Upgradable())
                {
                    skills[randomIndex].Upgrade();
                }
            }
        }

        public void RefreshSkills()
        {
            foreach (var skill in skills)
            {
                if (skill.FirstActiveMixin != null)
                {
                    skill.FirstActiveMixin.RefreshUses(skill.level);
                }
                if (skill.CombatSkillMixin != null)
                {
                    skill.CombatSkillMixin.RefreshUses(skill.level);
                }
            }
        }

        public List<Curse> GetCurses()
        {
            if (Skills.Count == 0)
                return null;
            return Skills.OfType<Curse>().ToList();
        }

        public bool HasSkill(Skill skill)
        {
            return skills.Any(s => s.Name == skill.Name);
        }
    }
}