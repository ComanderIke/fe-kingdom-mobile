﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using Game.GameResources;

namespace Game.GameActors.Units
{
    public class StatusEffectManager
    {
        public event Action<Unit, BuffDebuffBase> OnStatusEffectAdded;
        public event Action<Unit, BuffDebuffBase> OnStatusEffectRemoved;
        private Unit unit;
        //public  List<StatusEffect> StatusEffects { get; private set; }//TODO those will be removed because debuffs are basically them
        public List<Buff> Buffs { get; private set; }
        public List<Debuff> Debuffs { get; private set; }
        public List<StatModifier> StatModifiers { get; private set; }
        public StatusEffectManager(Unit unit)
        {
            this.unit = unit;
           // StatusEffects = new List<StatusEffect>();
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
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
        public void AddBuff(Buff buff)
        {
            Buffs.Add(buff);
            OnStatusEffectAdded?.Invoke(unit, buff);
        }
        public void AddDebuff(Debuff debuff)
        {
            foreach (var negate in debuff.negateTypes)
            {
                var tmpList = Debuffs.Where(d => d.debuffType == negate);
                foreach (var entry in tmpList)
                {
                    Debuffs.Remove(entry);
                }
            }
            Debuffs.Add(debuff);
            OnStatusEffectAdded?.Invoke(unit, debuff);
        }
        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);
            OnStatusEffectRemoved?.Invoke(unit, buff);
        }
        public void RemoveDebuff(Debuff debuff)
        {
            Debuffs.Remove(debuff);
            OnStatusEffectRemoved?.Invoke(unit, debuff);
        }

        public void Update()
        {
            var debuffEnd = Debuffs.Where(d => d.TakeEffect(unit)).ToList();
            var buffEnd = Buffs.Where(b => b.TakeEffect(unit)).ToList();
            var statModifierEnd = StatModifiers.Where(s => s.TakeEffect(unit)).ToList();
            foreach (var d in debuffEnd) RemoveDebuff(d);
            foreach (var b in buffEnd) RemoveBuff(b);
            foreach (var s in statModifierEnd) RemoveStatModifier(s);
        }

        public void AddStatModifier(StatModifier appliedStatModifier)
        {
            
           StatModifiers.Add(appliedStatModifier);
           OnStatusEffectAdded?.Invoke(unit, appliedStatModifier);
        }
        public void RemoveStatModifier(StatModifier debuff)
        {
            StatModifiers.Remove(debuff);
            OnStatusEffectRemoved?.Invoke(unit, debuff);
        }
    }
}