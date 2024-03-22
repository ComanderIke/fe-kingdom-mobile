using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum BuffType
    {
        Custom,
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
        Frog,
        Berserk
    }

    [CreateAssetMenu(menuName = "GameData/Buffs/BuffDebuffBase")]
    public class BuffDebuffBase : ScriptableObject
    {

        public BuffDebuffBaseData BuffData;
        [SerializeField] public int[] duration;
        [SerializeField] private List<UnitTargetSkillEffectMixin> mixins;
        private Unit caster;
        protected int level;
        public List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("For "+duration[level]+" Turns: ", "", ""));
            list.AddRange(BuffData.GetEffectDescription(level));
            foreach(var mixin in mixins)
                list.AddRange(mixin.GetEffectDescription(caster, level));
            // Debug.Log("TEST");
            return list;
        }
        public virtual void Apply(Unit caster, Unit target, int skilllevel)
        {
            this.caster = caster;
            BuffData.Apply(caster,target,skilllevel);
            foreach(var mixin in mixins)
               mixin.Activate(target, caster, skilllevel);


            Debug.Log("SPAWNED VFX");
            this.level = skilllevel;
        }
        public void IncreaseCurrentDuration()
        {
            duration[level]++;
        }
        public virtual bool TakeEffect(Unit unit)
        {
            if(duration[level]>0)
                BuffData.TakeEffect(unit);
           
            // Debug.Log("TAKE EFFECT"+duration[level]);
            duration[level]--;
            return duration[level] < 0;
        }

        public virtual void Unapply(Unit target)
        {
            MyDebug.LogTest("unapply in BuffDebuffBase");
            BuffData.Unapply(target,level);
            foreach(var mixin in mixins)
                mixin.Deactivate(target, caster, level);
            this.caster = null;
            level = 0;

        }

        public int GetDuration()
        {
            return duration[level];
        }
    }
}