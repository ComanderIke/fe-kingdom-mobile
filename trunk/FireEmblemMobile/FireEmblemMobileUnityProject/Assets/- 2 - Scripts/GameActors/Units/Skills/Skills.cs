using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters.Skills
{
    [System.Serializable]
    public abstract class Skill
    {
        const int MAX_LEVEL = 10;
        public String name;
        public String description;
        public int Level;
        public int CurrentCooldown;
        public int Cooldown;
		public int CooldownReduction = 0;
        public List<int> AttackRanges;
        public int manacost;
        public Sprite sprite;
        public Sprite sprite_hovered;
		public Sprite sprite_lvlup;
		public Sprite sprite_lvlup_pressed;
        public Sprite sprite_disabled;
        public SkillTarget target;
        public GameObject animationObject;
        protected float skillTime = 0;
        protected bool inAnimation = false;
        protected float animationTime = 0.0f;
        public Skill()
        {
            AttackRanges = new List<int>();
        }
        public abstract bool CanTargetCharacters();
        public virtual void Update()
        {
            if (inAnimation)
            {
                skillTime += Time.deltaTime;
                if (skillTime > animationTime)
                {
                    inAnimation = false;
                }

            }
        }
        public bool IsInAnimation()
        {
            return inAnimation;
        }
        public virtual void UpdateCD()
        {
            if (CurrentCooldown != 0)
                CurrentCooldown -= 1;
        }
        public abstract int getDamage(Unit user, bool justToShow);
        public bool CanUseSkill(Unit user)
        {
            if (CurrentCooldown == 0)
                return true;
            return false;
        }
        private bool OnCoolDown()
        {
            return CurrentCooldown != 0;
        }
    }
}
