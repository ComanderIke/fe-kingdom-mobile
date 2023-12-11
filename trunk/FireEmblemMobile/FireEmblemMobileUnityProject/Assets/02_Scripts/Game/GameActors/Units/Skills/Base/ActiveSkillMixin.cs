﻿using System;
using System.Security.Cryptography.X509Certificates;
using _02_Scripts.Game.GameActors.Items.Consumables;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
   [Serializable]
    public class ActiveSkillMixin : SkillMixin
    {
        public GameObject AnimationObject;
        public bool spawnAnimationAtCaster;
        public bool useScreenPosition;
        public EffectType effectType;
        public float effectDelay = 0f;
        public float logicDelay = 0f;
      //this cannot be in a ScriptableObject...

        // Solution 1) create Instance of this SO before assigning it to a skill
        // Solution 2) save currentUses in Skill class (weird because only active skills use it, but some passives might use it(so it triggers only once per combat etc..)
        // Solution 3) 
        public int[] maxUsesPerLevel;
        public int[] hpCostPerLevel;
        public bool costIsSkillPoints= true;
        [HideInInspector]public int Uses { get; set; }
       
        // public ActiveSkillMixin(int[] maxUsesPerLevel, int[] hpCostPerLevel, GameObject animationObject):base()
        // {
        //     this.AnimationObject = animationObject;
        //     this.maxUsesPerLevel = maxUsesPerLevel;
        //     this.currentUses = maxUsesPerLevel;
        //     this.hpCostPerLevel = hpCostPerLevel;
        // }
        public void PayActivationCost()
        {
            if (skill.owner != null)
            {
                if (costIsSkillPoints)
                {
                    skill.owner.SkillManager.SkillPoints -= hpCostPerLevel[skill.level];
                }
                else
                {
                    skill.owner.Hp -= hpCostPerLevel[skill.level];
                }
            }
                
            Uses--;
        }

        private void OnEnable()
        {


        }


        public int GetMaxUses(int level)
        {
            return maxUsesPerLevel[level];
        }
        public int GetHpCost(int level)
        {
            return hpCostPerLevel[level];
        }

        public void RefreshUses(int level)
        {
            Uses = GetMaxUses(level);
        }
    }
}