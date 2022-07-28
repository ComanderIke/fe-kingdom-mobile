using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;

namespace Game.GameActors.Units
{
    public class StatusEffectManager
    {
        private Unit unit;
        private List<Debuff> Debuffs { get; set; }
        private List<Buff> Buffs { get; set; }
        public StatusEffectManager(Unit unit)
        {
            this.unit = unit;
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
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