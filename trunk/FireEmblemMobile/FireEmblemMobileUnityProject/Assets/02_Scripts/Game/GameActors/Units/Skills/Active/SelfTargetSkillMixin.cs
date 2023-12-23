using System.Collections;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SelfTarget", fileName = "SelfTargetSkillMixin")]
    public class SelfTargetSkillMixin : ActiveSkillMixin
    {
        public virtual void Activate(Unit user)
        {
            base.Activate(user);

            if (user != null)
            {
               SpawnAnimation(user);
            }

            base.PayActivationCost();
        }

        // protected SelfTargetSkillMixin( int[] maxUses, int[] hpCost,GameObject animationObject) : base(maxUses,hpCost,animationObject)
        // {
        // }
        public void Deactivate(Unit unit)
        {
            base.Deactivate(unit);
        }
        
    }
}