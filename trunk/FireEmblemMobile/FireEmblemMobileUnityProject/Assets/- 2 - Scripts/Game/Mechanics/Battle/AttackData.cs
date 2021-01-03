using System.Collections.Generic;
using Game.GameActors.Units;

namespace Game.Mechanics.Battle
{
    public class AttackData
    {
        public Unit Attacker { get; set; }
        public float AttackMultiplier { get; set; }
        public int Dmg { get; set; }
        public List<AttackAttributes> AttackAttributes { get; set; }

        public AttackData(Unit attacker, int dmg, float attackMultiplier = 1.0f,
            List<AttackAttributes> attackAttributes = null)
        {
            Attacker = attacker;
            Dmg = dmg;
            AttackMultiplier = attackMultiplier;
        }
    }
}