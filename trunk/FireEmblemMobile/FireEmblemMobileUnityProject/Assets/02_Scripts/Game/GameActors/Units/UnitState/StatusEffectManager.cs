using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using Game.GameResources;

namespace Game.GameActors.Units
{
    public class StatusEffectManager
    {
        public event Action<StatusEffect> OnStatusEffectAdded;
        public event Action<StatusEffect> OnStatusEffectRemoved;
        private Unit unit;
        public  List<StatusEffect> StatusEffects { get; private set; }
        private List<Buff> Buffs { get; set; }
        private List<Debuff> Debuffs { get; set; }
        public StatusEffectManager(Unit unit)
        {
            this.unit = unit;
            StatusEffects = new List<StatusEffect>();
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
        }
        public void AddStatusEffect(StatusEffect statusEffect)
        {
            StatusEffects.Add(statusEffect);
            OnStatusEffectAdded?.Invoke(statusEffect);
        }
       
        public void RemoveStatusEffect(StatusEffect statusEffect)
        {
            StatusEffects.Remove(statusEffect);
            OnStatusEffectRemoved?.Invoke(statusEffect);
        }
        public void AddBuff(Buff buff)
        {
            Buffs.Add(buff);
        }
        public void AddDebuff(Debuff debuff)
        {
            Debuffs.Add(debuff);
        }
        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);
        }
        public void RemoveDebuff(Debuff debuff)
        {
            Debuffs.Remove(debuff);
        }

        public void Update()
        {
            var debuffEnd = Debuffs.Where(d => d.TakeEffect(unit)).ToList();
            var buffEnd = Buffs.Where(b => b.TakeEffect(unit)).ToList();
            foreach (var d in debuffEnd) RemoveDebuff(d);
            foreach (var b in buffEnd) RemoveBuff(b);
        }
    }
}