using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
   [Serializable]
    public class ActiveSkillMixin : SkillMixin
    {
        public GameObject AnimationObject;
      //this cannot be in a ScriptableObject...
        // Solution 1) create Instance of this SO before assigning it to a skill
        // Solution 2) save currentUses in Skill class (weird because only active skills use it, but some passives might use it(so it triggers only once per combat etc..)
        // Solution 3) 
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
            if (maxUsesPerLevel == null||maxUsesPerLevel.Length != MAXLEVEL)
            {
                Array.Resize(ref maxUsesPerLevel, MAXLEVEL);
            }
            if (hpCostPerLevel == null||hpCostPerLevel.Length != MAXLEVEL)
            {
                Array.Resize(ref hpCostPerLevel, MAXLEVEL);
            }
            
            
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