using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.UnitState
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
        public StatusEffectManager(Unit unit, StatusEffectManager toClone)
        {
            this.unit = unit;
            // StatusEffects = new List<StatusEffect>();
            Buffs = new List<BuffDebuffBase>(toClone.Buffs);
            StatModifiers = new List<StatModifier>(toClone.StatModifiers);
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
        
        public void AddBuffDebuff(BuffDebuffBase newBuff, Unit caster, int level)
        {
            bool contains = false;
            bool stronger = false;
            BuffDebuffBase replaceBuff=null;
            foreach (var buff in Buffs)
            {
                if (buff.BuffData is DebuffData debuffData)
                {
                    if (newBuff.BuffData is DebuffData newDebuffData)
                    {
                        if (debuffData.debuffType == newDebuffData.debuffType)
                        {
                            contains = true;
                            //Clear weaker buff
                            stronger = buff.GetDuration() < newBuff.duration[level];
                            replaceBuff = buff;
                            break;
                        }
                    }
                }
                
            }

            if (contains)
            {
                if (stronger)
                {
                   
                    Buffs.Remove(replaceBuff);
                    replaceBuff.Unapply(unit);
                    Buffs.Add(newBuff);
                    newBuff.Apply(caster,unit, level);
                    OnStatusEffectAdded?.Invoke(unit, newBuff);
                }
                else
                {
                    MyDebug.LogTest("Adding Weaker Buff... Ignore?");
                }
            }
            else
            {
                newBuff.Apply(caster,unit, level);
                Buffs.Add(newBuff);
                OnStatusEffectAdded?.Invoke(unit, newBuff);
            }
         
        }
        public void RemoveBuff(BuffDebuffBase buff)
        {
            Buffs.Remove(buff);
            buff.Unapply(unit);
            OnStatusEffectRemoved?.Invoke(unit, buff);
        }
        public void RemoveDebuff(DebuffType debuffType)
        {
            List<BuffDebuffBase> toRemove = new List<BuffDebuffBase>();
            foreach (var buff in Buffs)
            {
                if (buff.BuffData is DebuffData debuffData)
                {
                    if (debuffData.debuffType == debuffType)
                    {
                        toRemove.Add(buff);
                    }
                }
            }

            foreach (var buff in toRemove)
            {
                Buffs.Remove(buff);
                buff.Unapply(unit);
                OnStatusEffectRemoved?.Invoke(unit, buff);
            }
            toRemove.Clear();
            
            
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

        public bool HasDebuff(DebuffType debuffTyoe)
        {
            foreach (var buff in Buffs)
            {
                if (buff.BuffData is DebuffData debuffData)
                {
                    if (debuffData.debuffType == debuffTyoe)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}