﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.SpecialAttacks
{
    public abstract class SpecialAttack
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public SpecialAttackVisual SpecialAttackVisual { get; set; }

        public SpecialAttack(string name, string description, int cooldown, int iconSpriteId=0, int animationId=0)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            SpecialAttackVisual = new SpecialAttackVisual(iconSpriteId, animationId);
        }
        public abstract void UseSpecial(LivingObject user,int normalAttackDamage, LivingObject defender);
        public abstract int GetSpecialDmg(LivingObject user, int normalHitRate, LivingObject defender);
        public abstract int GetSpecialHit(LivingObject user, int normalAttackDamage, LivingObject defender);
    }
}
