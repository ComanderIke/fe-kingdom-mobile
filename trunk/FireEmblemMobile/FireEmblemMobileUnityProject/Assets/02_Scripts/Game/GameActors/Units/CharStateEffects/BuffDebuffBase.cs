using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum BuffType
    {
        CurseResistance,
        MagicResistance,
        Cleansing,
        Regeneration,
        Stealth,
        AbsorbingDmg,
        Invulnerability,
        Stride,
        Rage
        //ClearMovement
        //Add as needed
    }

    [Serializable]
    public enum DebuffType
    {
        Stunned,
        Silenced,
        Poisened,
        Enwebed,
        Slept,
        Tempted,
        Frog
    }

    public abstract class BuffDebuffBase : ScriptableObject
    {

        public BuffDebuffBaseData BuffData;
        [SerializeField] public int[] duration;
       
        protected int level;
        public List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("For "+duration[level]+" Turns: ", "", ""));
            list.AddRange(BuffData.GetEffectDescription(level));
           
            return list;
        }
        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            BuffData.Apply(caster,target,skilllevel);
          

            Debug.Log("SPAWNED VFX");
            this.level = skilllevel;
        }
        public void IncreaseCurrentDuration()
        {
            duration[level]++;
        }
        public virtual bool TakeEffect(Unit unit)
        {
            BuffData.TakeEffect(unit);
           
            Debug.Log("TAKE EFFECT"+duration[level]);
            duration[level]--;
            return duration[level] <= 0;
        }

        public virtual void Unapply(Unit target)
        {
            level = 0;
        }

        public int GetDuration()
        {
            return duration[level];
        }
    }
}