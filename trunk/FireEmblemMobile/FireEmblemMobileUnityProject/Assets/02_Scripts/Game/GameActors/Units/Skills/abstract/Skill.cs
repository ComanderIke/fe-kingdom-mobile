using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    // public abstract class SkillMixin
    // {
    //     
    // }
    // public abstract class PassiveSkillMixin
    // {
    //     
    // }
    // public abstract class ActiveSkillMixin
    // {
    //     
    // }
    [System.Serializable]
    public abstract class Skill : ITargetableObject
    {
        public Sprite Icon;
        public GameObject AnimationObject;
        public SkillType SkillType { get; set; }
        public string Description;
        public int Level;
        public string Name;
        public string[] UpgradeDescr;
        public int Tier;
        //public List<SkillMixin> mixins;
        public abstract List<EffectDescription> GetEffectDescription();

        public Skill(string Name, string description, Sprite icon, GameObject animationObject, int tier, string []upgradeDescr)
        {
            this.Name = Name;
            this.Description = description;
            this.Icon = icon;
            this.AnimationObject = animationObject;
            this.Tier = tier;
            this.UpgradeDescr = upgradeDescr;
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

        public virtual void Update()
        {
        }
        

        public bool CanUseSkill(Unit user)
        {
            return true;
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
            return UpgradeDescr[Level];
        }

        public string NextUpgradeText()
        {
            if(Level+1< UpgradeDescr.Length)
                return UpgradeDescr[Level+1];
            return "Maxed";
        }
    }
}