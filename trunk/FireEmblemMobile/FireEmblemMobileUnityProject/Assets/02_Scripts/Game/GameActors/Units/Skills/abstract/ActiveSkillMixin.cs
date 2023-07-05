using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
   [Serializable]
    public class ActiveSkillMixin : SkillMixin
    {
        public GameObject AnimationObject;
        public int[] currentUses;
        public int[] maxUsesPerLevel;
        public int[] hpCostPerLevel;

        // public ActiveSkillMixin(int[] maxUsesPerLevel, int[] hpCostPerLevel, GameObject animationObject):base()
        // {
        //     this.AnimationObject = animationObject;
        //     this.maxUsesPerLevel = maxUsesPerLevel;
        //     this.currentUses = maxUsesPerLevel;
        //     this.hpCostPerLevel = hpCostPerLevel;
        // }
        private void OnEnable()
        {
            OnValidate();
        }

        protected void OnValidate()
        {
            if (currentUses==null||currentUses.Length != MAXLEVEL)
            {
                Array.Resize(ref currentUses, MAXLEVEL);
            }
            if (maxUsesPerLevel == null||maxUsesPerLevel.Length != MAXLEVEL)
            {
                Array.Resize(ref maxUsesPerLevel, MAXLEVEL);
            }
            if (hpCostPerLevel == null||hpCostPerLevel.Length != MAXLEVEL)
            {
                Array.Resize(ref hpCostPerLevel, MAXLEVEL);
            }
            
            
        }
        public int GetCurrentUses(int level)
        {
            return currentUses[level];
        }
        public int GetMaxUses(int level)
        {
            return maxUsesPerLevel[level];
        }
        public int GetHpCost(int level)
        {
            return hpCostPerLevel[level];
        }
    }
}