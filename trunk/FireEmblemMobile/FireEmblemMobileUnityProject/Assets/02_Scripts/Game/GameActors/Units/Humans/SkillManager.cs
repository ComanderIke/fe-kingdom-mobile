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

        public void LearnSkill(SkillTreeEntry clickedSkill)
        {
            if (SkillPoints >= 1)
            {
                if (Skills.Contains(clickedSkill.skill))
                {
                    Skills.Find(s => clickedSkill.skill.name == s.name).Level++;
                }
                else
                {
                    Skills.Add((clickedSkill.skill));
                    
                    clickedSkill.skill.Level++;
                    foreach (var skilltree in SkillTrees)
                    {
                
                        var entry = skilltree.skillEntries.Find(se => se.skill.name == clickedSkill.skill.name);
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
            foreach (var skilltree in SkillTrees)
            {
                
                entry = skilltree.skillEntries.Find(se => se.skill.name == skillEntry.skill.name
                                                          &&se.tree==skillEntry.tree
                                                          && se.row==skillEntry.row);
                if (entry != null)
                {
                    tree = skilltree;
                    break;
                    
                }
            }

            Debug.Log(skillEntry.skill.name+" "+tree.currentDepth+" "+skillEntry.row);
            if(tree.currentDepth<skillEntry.row)
                entry.SkillState = SkillState.NotLearnable;
            else
            {
                entry.SkillState = SkillState.Learnable;
            }
        
            if (Skills.Contains(entry.skill))
            {
                entry.SkillState = SkillState.Learned;
                if ( entry.skill.Level ==  entry.skill.MaxLevel)
                    entry.SkillState = SkillState.Maxed;
            }

           
            Debug.Log(entry.SkillState);
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