using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class Skill: ScriptableObject
    {
        private const int MAX_LEVEL = 10;
        public Sprite Icon;

        public GameObject AnimationObject { get; private set; }

        public int Cooldown;

        public int CooldownReduction { get; private set; } = 0;

        public int CurrentCooldown { get; private set; }

        public string Description;

        public int Level;

        public string Name;

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
    }
}