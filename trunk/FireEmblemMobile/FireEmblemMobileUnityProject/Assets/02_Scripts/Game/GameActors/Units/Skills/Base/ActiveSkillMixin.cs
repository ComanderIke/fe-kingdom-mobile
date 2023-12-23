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
        public void SpawnAnimation(Unit user)
        {
            if (AnimationObject != null)
            {
                var go = Instantiate(AnimationObject);
                go.transform.position = user.GameTransformManager.GetCenterPosition();

            }
        }
        public void PayActivationCost()
        {
            if (skill.owner != null)
            {
               
                skill.owner.SkillManager.PaySkillPoints(hpCostPerLevel[skill.level],costIsSkillPoints);
                    
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
            var blessing = GetBlessing(skill.owner);
            int costReduction = 0;
            if (blessing != null)
                costReduction = synergies[blessing].costReduction;
            return hpCostPerLevel[level]-costReduction;
        }

        public void RefreshUses(int level)
        {
            Uses = GetMaxUses(level);
        }
    }
}