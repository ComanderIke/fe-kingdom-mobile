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
        private List<Skill> skills;

        public List<Skill> Skills
        {
            get
            {
                if (skills == null)
                    skills = new List<Skill>();
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

        [SerializeField]
     // [HideInInspector]
        public SkillTree[] SkillBuildTrees;

         

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
                    foreach (var skilltree in SkillBuildTrees)
                    {
                
                        var entry = skilltree.skillEntries.Find(se => se.Skill.Name == clickedSkill.Skill.Name);
                        if (entry != null)
                        {
                            if(clickedSkill.row+1 > skilltree.currentDepth)
                                skilltree.currentDepth = clickedSkill.row+1;
                            break;
                    
                        }
                    }
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
            SkillTree tree = null;
            foreach (var skilltree in SkillBuildTrees)
            {
                
                entry = skilltree.skillEntries.Find(se => se.Skill.Name == skillEntry.Skill.Name
                                                          &&se.tree==skillEntry.tree
                                                          && se.row==skillEntry.row);
                if (entry != null)
                {
                    tree = skilltree;
                    break;
                    
                }
            }

            Debug.Log(skillEntry.Skill.Name+" "+tree.currentDepth+" "+skillEntry.row);
            if(tree.currentDepth<skillEntry.row)
                entry.SkillState = SkillState.NotLearnable;
            else
            {
                entry.SkillState = SkillState.Learnable;
            }
        
            if (Skills.Contains(entry.Skill))
            {
                entry.SkillState = SkillState.Learned;
                if ( entry.Skill.Level ==  entry.Skill.MaxLevel)
                    entry.SkillState = SkillState.Maxed;
            }

           
            Debug.Log(entry.SkillState);
        }
        public void Init()
        {
            if(SkillBuildTrees!=null)
                foreach(var skillTree in SkillBuildTrees)
                {
                    skillTree.Init();
                }

        }
    }
}