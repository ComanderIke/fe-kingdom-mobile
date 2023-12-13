using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Units
{
    public class StatusEffectManager
    {
        public event Action<Unit, BuffDebuffBase> OnStatusEffectAdded;
        public event Action<Unit, BuffDebuffBase> OnStatusEffectRemoved;
        private Unit unit;
        //public  List<StatusEffect> StatusEffects { get; private set; }//TODO those will be removed because debuffs are basically them
        public List<BuffDebuffBase> Buffs { get; private set; }
        public List<StatModifier> StatModifiers { get; private set; }
        public StatusEffectManager(Unit unit)
        {
            this.unit = unit;
           // StatusEffects = new List<StatusEffect>();
            Buffs = new List<BuffDebuffBase>();
            StatModifiers = new List<StatModifier>();
        }
        // public void AddStatusEffect(StatusEffect statusEffect)
        // {
        //     StatusEffects.Add(statusEffect);
        //     OnStatusEffectAdded?.Invoke(statusEffect);
        // }
        //
        // public void RemoveStatusEffect(StatusEffect statusEffect)
        // {
        //     StatusEffects.Remove(statusEffect);
        //     OnStatusEffectRemoved?.Invoke(statusEffect);
        // }
        
        public void AddBuffDebuff(BuffDebuffBase debuff, Unit caster, int level)
        {
            
            debuff.Apply(caster,unit, level);
            Buffs.Add(debuff);
            OnStatusEffectAdded?.Invoke(unit, debuff);
        }
        public void RemoveBuff(BuffDebuffBase buff)
        {
            Buffs.Remove(buff);
            buff.Unapply(unit);
            OnStatusEffectRemoved?.Invoke(unit, buff);
        }
        public void RemoveBuff(BuffDebuffBaseData buffData)
        {
            for (int i = Buffs.Count - 1; i >= 0; i--)
            {
                if(Buffs[i].BuffData == buffData)
                    RemoveBuff(Buffs[i]);
            }
        }


        
        public void UpdateTurn()
        {
            var buffEnd = Buffs.Where(b => b.TakeEffect(unit)).ToList();
            var statModifierEnd = StatModifiers.Where(s => s.TakeEffect(unit)).ToList();
            //Debug.Log("UPDATE TURN BUFF END: "+buffEnd.Count);
            foreach (var b in buffEnd) RemoveBuff(b);
            foreach (var s in statModifierEnd) RemoveStatModifier(s);
        }

        public void AddStatModifier(StatModifier appliedStatModifier, int level )
        {
            appliedStatModifier.Apply(unit,unit,level);
           
            StatModifiers.Add(appliedStatModifier);
            Debug.Log("ADD STATUS MODIFIER: "+unit.name+" "+appliedStatModifier.name+" "+level);
            OnStatusEffectAdded?.Invoke(unit, appliedStatModifier);
        }
        //Stay Private stat modifers should either be removed by duration end or all at oncewill be removed by cleansing skill
        public void RemoveStatModifier(StatModifier appliedStatModifier)
        {
            Debug.Log("TODO how to check if clone is equal? add id to stat modifiers?");
            StatModifiers.First(a=>a.Equals(appliedStatModifier)).Unapply(unit);
            StatModifiers.Remove(appliedStatModifier);
            OnStatusEffectRemoved?.Invoke(unit, appliedStatModifier);
        }
    }
}