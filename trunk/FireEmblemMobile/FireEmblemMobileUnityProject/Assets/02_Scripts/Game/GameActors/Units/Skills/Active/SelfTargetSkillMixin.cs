using Game.GameActors.Units.Skills.Base;
using Game.Manager;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Active
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Active/SelfTarget", fileName = "SelfTargetSkillMixin")]
    public class SelfTargetSkillMixin : ActiveSkillMixin
    {
        public virtual void Activate(Unit user)
        {
            base.Activate(user);

            if (user != null&& GridGameManager.Instance!=null)
            {
               SpawnAnimation(user.GameTransformManager.GetCenterPosition());
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