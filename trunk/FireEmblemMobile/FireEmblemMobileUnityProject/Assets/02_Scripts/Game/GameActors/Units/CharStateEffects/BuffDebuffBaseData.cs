using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBaseData : ScriptableObject
    {
        [SerializeField] public GameObject vfx;
        [field:SerializeField] public Sprite Icon { get; set; }

        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            if (vfx != null)
            {
                var go = Instantiate(vfx, null);
                go.transform.position = target.GameTransformManager.GetCenterPosition();
            }
        }
        public virtual void Unapply(Unit target,int skillLevel)
        {
            
        }
        public virtual void TakeEffect(Unit unit)
        {
        }

        public abstract IEnumerable<EffectDescription> GetEffectDescription(int level);
    }
}