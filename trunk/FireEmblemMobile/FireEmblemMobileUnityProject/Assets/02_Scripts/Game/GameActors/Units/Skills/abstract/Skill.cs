using System.Collections.Generic;
using System.Linq;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public class Skill:ITargetableObject
    {
        public Sprite Icon;
        public SkillType SkillType { get; set; }
        private int level;
        public string Name;
        public string Description;
        public int Tier;
        public List<PassiveSkillMixin> passiveMixins;
        public ActiveSkillMixin activeMixin;
        private Dictionary<int, List<PassiveSkillMixin>> mixinsPerLevel;

        public Skill(string Name, string Description, Sprite icon, int tier, List<PassiveSkillMixin> passiveMixins, ActiveSkillMixin activeMixin)
        {
            this.Name = Name;
            this.Icon = icon;
            this.Tier = tier;
            this.Description = Description;
            this.passiveMixins = passiveMixins;
            this.activeMixin = activeMixin;
            mixinsPerLevel = new Dictionary<int, List<PassiveSkillMixin>>();
        }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if(value> level)
                    for (int l= this.level + 1; l <= value; l++)
                    {
                        if(mixinsPerLevel.ContainsKey(l)&&mixinsPerLevel[l]!=null)
                            passiveMixins.AddRange(mixinsPerLevel[l]);
                    }
                else if (value < level)
                {
                    for (int l= this.level; l > value; l--)
                    {
                        if(mixinsPerLevel.ContainsKey(l))
                            foreach (var mixin in mixinsPerLevel[l])
                            {
                                if(mixin!=null)
                                    passiveMixins.Remove(mixin);
                            }
                    }
                }

                this.level = value;
            }
        }
       
        public string GetName()
        {
            return Name;
        }
        public string GetDescription()
        {
            return Description;
        }
        public Sprite GetIcon()
        {
            return Icon;
        }
        
        public virtual void BindSkill(Unit unit)
        {
            Debug.Log("Bind Skill");
        }
        public virtual void UnbindSkill(Unit unit)
        {
            Debug.Log("Unbind Skill");
        }

        public string CurrentUpgradeText()
        {
            return "";
        }

        public string NextUpgradeText()
        {
            return "";
        }
    }
}