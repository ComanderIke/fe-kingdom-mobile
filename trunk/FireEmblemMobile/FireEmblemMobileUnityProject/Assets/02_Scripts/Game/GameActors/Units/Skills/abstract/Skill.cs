﻿using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    public abstract class Skill : ITargetableObject
    {
        public Sprite Icon;

        public GameObject AnimationObject;

        public int Cooldown;

        public int CooldownReduction { get; private set; } = 0;

        public int CurrentCooldown { get; private set; }
       // public int MaxLevel = 5;

        public string Description;
        public string[] UpgradeDescriptions;

        public int Level;

        public string Name;

        public Skill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr)
        {
            this.Name = Name;
            this.Description = description;
            this.Icon = icon;
            this.AnimationObject = animationObject;
            this.Cooldown = cooldown;
            //this.MaxLevel = maxLevel;
            this.UpgradeDescriptions = upgradeDescr;
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

        // public SkillTarget Target;
       

        public abstract bool CanTargetCharacters();

        public virtual void Update()
        {
        }

        public virtual void UpdateCd()
        {
            if (CurrentCooldown != 0)
                CurrentCooldown -= 1;
        }

        public abstract int GetDamage(Unit user, bool justToShow);

        public bool CanUseSkill(Unit user)
        {
            return CurrentCooldown == 0;
        }

        public bool OnCoolDown()
        {
            return CurrentCooldown != 0;
        }

        public string NextUpgradeText()
        {
            if(Level<UpgradeDescriptions.Length)
                return UpgradeDescriptions[Level];
            return "";
        }

        public string CurrentUpgradeText()
        {
            if (Level - 1 < 0)
                return "";
            return UpgradeDescriptions[Level-1];
        }
    }
}