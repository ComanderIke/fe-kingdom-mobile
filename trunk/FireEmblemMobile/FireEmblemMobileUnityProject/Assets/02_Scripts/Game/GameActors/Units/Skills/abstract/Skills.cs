﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class Skill:  ScriptableObject,ITargetableObject
    {
        
        public Sprite Icon;

        public GameObject AnimationObject;

        public int Cooldown;

        public int CooldownReduction { get; private set; } = 0;

        public int CurrentCooldown { get; private set; }
        public int MaxLevel = 1;

        public string Description;
        public string[] UpgradeDescriptions;

        public int Level;

        public string Name;

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