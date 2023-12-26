using System.Collections.Generic;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBaseData : ScriptableObject
    {
        [FormerlySerializedAs("vfx")] [SerializeField] public GameObject vfxApplied;
        [FormerlySerializedAs("vfx")] [SerializeField] public GameObject vfxTakeEffect;
        [field:SerializeField] public Sprite Icon { get; set; }

        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            if (vfxApplied != null)
            {
                var go = Instantiate(vfxApplied, null);
                go.transform.position = target.GameTransformManager.GetCenterPosition();
            }
            if (vfxTakeEffect != null)
            {
                var go = Instantiate(vfxTakeEffect, null);
                go.transform.position = target.GameTransformManager.GetCenterPosition();
            }
        }
        public virtual void Unapply(Unit target,int skillLevel)
        {
            
        }
        public virtual void TakeEffect(Unit unit)
        {
            if (vfxTakeEffect != null)
            {
                var go = Instantiate(vfxTakeEffect, null);
                go.transform.position = unit.GameTransformManager.GetCenterPosition();
            }
        }

        public abstract IEnumerable<EffectDescription> GetEffectDescription(int level);
    }
}