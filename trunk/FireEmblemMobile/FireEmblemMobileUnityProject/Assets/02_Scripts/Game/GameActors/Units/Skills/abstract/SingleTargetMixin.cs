using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
  
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SelfTarget", fileName = "SelfTargetSkillMixin")]
    public class SingleTargetMixin : ActiveSkillMixin
    {
        public virtual void Activate(Unit user, Unit target)
        {

        }

        public virtual void Effect(Unit user, Unit target)
        {
        }

        public bool CanTarget(Unit user, Unit target)
        {
            return true;
        }
     
        

        //
        // protected SingleTargetMixin( int[] maxUses, int[] hpCost,GameObject animationObject) : base(maxUses,hpCost,animationObject)
        // {
        // }
    }
}