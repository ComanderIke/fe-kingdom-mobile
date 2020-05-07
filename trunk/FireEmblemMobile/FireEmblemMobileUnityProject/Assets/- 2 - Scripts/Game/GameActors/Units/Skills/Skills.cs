using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameActors.Units.Skills
{
    [Serializable]
    public abstract class Skill
    {
        private const int MAX_LEVEL = 10;

        protected Skill()
        {
            AttackRanges = new List<int>();
        }

        public GameObject AnimationObject { get; private set; }

        public List<int> AttackRanges { get; private set; }

        public int Cooldown { get; private set; }

        public int CooldownReduction { get; private set; } = 0;

        public int CurrentCooldown { get; private set; }

        public string Description { get; private set; }

        public int Level { get; private set; }

        public string Name { get; private set; }

        public SkillTarget Target { get; private set; }

        public SkillSpriteSet SpriteSet { get; }

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