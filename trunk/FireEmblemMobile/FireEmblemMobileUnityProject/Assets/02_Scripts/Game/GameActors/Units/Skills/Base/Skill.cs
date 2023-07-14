using System.Collections.Generic;
using System.Linq;
using Game.GameInput;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public class Skill:ITargetableObject
    {
        protected int maxLevel;
        public Sprite Icon;
        public SkillType SkillType { get; set; }
        protected int level;
        public string Name;
        public string Description;
        public int Tier;
        public List<PassiveSkillMixin> passiveMixins;
        public ActiveSkillMixin activeMixin;
        public int ActiveMixinUses;
        private Unit owner;

        public Skill(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, ActiveSkillMixin activeMixin)
        {
            this.Name = Name;
            this.Icon = icon;
            this.Tier = tier;
            this.Description = Description;
            this.passiveMixins = passiveMixins;
            this.activeMixin = activeMixin;
            foreach (var passive in passiveMixins)
                passive.skill = this;

            if (activeMixin)
            {
                activeMixin.skill = this;
                ActiveMixinUses = activeMixin.GetMaxUses(Level);
            }

            this.maxLevel = maxLevel;


        }

        public override bool Equals(object obj)
        {
            if (obj is Skill skill)
            {
                return skill.Name == Name;
            }
            return base.Equals(obj);
        }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
               
                this.level = value;
                if(activeMixin)
                    ActiveMixinUses = activeMixin.GetMaxUses(level);
                //TODO Unbind and rebind all skills (So that multipliers etc will get updated)
                if(owner!=null)
                    foreach (var passive in passiveMixins)
                    {
                        passive.UnbindFromUnit(owner,this);
                        passive.BindToUnit(owner, this);
                    }
                
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
            
            owner = unit;
            foreach (var passive in passiveMixins)
            {
                passive.BindToUnit(owner, this);
            }
        }
        public virtual void UnbindSkill(Unit unit)
        {
            foreach (var passive in passiveMixins)
            {
                passive.UnbindFromUnit(owner,this);
            }
            owner = null;
        }

        public string CurrentUpgradeText()
        {
            return "";
        }

        public string NextUpgradeText()
        {
            return "";
        }


        public virtual Skill Clone()
        {
            var newSkill = new Skill(Name, Description, Icon, Tier, maxLevel,passiveMixins, activeMixin);
            newSkill.level = Level;
            return newSkill;
        }

        public bool Upgradable()
        {
            if (this is Curse)
                return false;
            // if (this is Blessing)
            //     return false;
            return Level < maxLevel;
        }
    }
}