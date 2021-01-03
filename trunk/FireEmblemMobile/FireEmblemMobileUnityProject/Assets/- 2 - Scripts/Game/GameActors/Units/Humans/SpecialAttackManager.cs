using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.SpecialAttacks;

namespace Game.GameActors.Units.Humans
{
    public class SpecialAttackManager
    {
        public SpecialAttack EquippedSpecial;
        public List<SpecialAttack> Specials;

        public SpecialAttackManager()
        {
            Specials = new List<SpecialAttack>();
        }

        public void AddSpecial(SpecialAttack special)
        {
            Specials.Add(special);
            if (Specials.Count == 1)
                EquippedSpecial = special;
        }

        public bool HasSpecial<T>()
        {
            return Specials.OfType<T>().Any();
        }

        public T GetSpecial<T>()
        {
            foreach (var s in Specials.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }
    }
}