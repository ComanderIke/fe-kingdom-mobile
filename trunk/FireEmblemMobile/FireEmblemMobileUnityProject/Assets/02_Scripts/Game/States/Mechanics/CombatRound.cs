using System.Collections.Generic;

namespace Game.Mechanics
{
    public class CombatRound
    {
        public int AttackerHP;
        public int DefenderHP;
        public int AttackerDamage { get; set; }
        public int AttackerHit { get; set; }
        public int AttackerAttackCount { get; set; }
        public int DefenderDamage { get; set; }
        public int DefenderHit { get; set; }
        public int DefenderAttackCount { get; set; }
        public bool AttackerAlive{ get; set; }
        public bool DttackerAlive{ get; set; }
        public int AttackerCrit { get; set; }
        public int DefenderCrit { get; set; }
        public List<AttackData> AttacksData;
    }
}