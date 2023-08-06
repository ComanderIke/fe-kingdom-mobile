using UnityEngine;
using UnityEngine.SubsystemsImplementation;

namespace Game.Mechanics.Battle
{
    [System.Serializable]
    public class BonusStats
    {
        public enum CombatStatType
        {
            Attack,
            Hit,
            Avoid,
            AttackSpeed,
            Crit,
            Critavoid,
            PhysicalResistance,
            MagicResistance
        }
        [field:SerializeField]
        public int Hit { get; set; }
        [field:SerializeField]
        public int Avoid { get; set; }
        [field:SerializeField]
        public int CritAvoid { get; set; }
        [field:SerializeField]
        public int Crit { get; set; }
        [field:SerializeField]
        public int AttackSpeed { get; set; }
        [field:SerializeField]
        public int Armor { get; set; }
        [field:SerializeField]
        public int MagicResistance { get; set; }
        [field:SerializeField]
        public int Attack { get; set; }

        public void Reset()
        {
            Debug.Log("TODO Reset Stats");
        }

        public BonusStats()
        {
            
        }
        public BonusStats(BonusStats bonusStats)
        {
            Hit = bonusStats.Hit;
            Avoid = bonusStats.Avoid;
            CritAvoid = bonusStats.CritAvoid;
            Crit = bonusStats.Crit;
            AttackSpeed = bonusStats.AttackSpeed;
            Armor = bonusStats.Armor;
            MagicResistance = bonusStats.MagicResistance;
            Attack = bonusStats.Attack;
           
        }
        public static string GetAsText(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return "Attack";
                case CombatStatType.Avoid: return "Avoid";
                case CombatStatType.Crit: return "Critical";
                case CombatStatType.Critavoid: return "Critavoid";
                case CombatStatType.Hit: return "Hitrate";
                case CombatStatType.MagicResistance: return "M. Defense";
                case CombatStatType.PhysicalResistance: return "Ph. Defense";
                case CombatStatType.AttackSpeed: return "Attackspeed";
            }

            return "";
        }
        public int GetStatFromEnum(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return Attack;
                case CombatStatType.Avoid: return Avoid;
                case CombatStatType.Crit: return Crit;
                case CombatStatType.Critavoid: return CritAvoid;
                case CombatStatType.Hit: return Hit;
                case CombatStatType.MagicResistance: return MagicResistance;
                case CombatStatType.PhysicalResistance: return Armor;
                case CombatStatType.AttackSpeed: return AttackSpeed;
            }

            return 0;
        }

        public static BonusStats operator +(BonusStats a, BonusStats b)
        {
            var sum = new BonusStats(a);
            sum.Hit += b.Hit;
            sum.Avoid += b.Avoid;
            sum.CritAvoid += b.CritAvoid;
            sum. Crit += b.Crit;
            sum.AttackSpeed += b.AttackSpeed;
            sum.Armor += b.Armor;
            sum.MagicResistance += b.MagicResistance;
            sum.Attack += b.Attack;
            return sum;

        }
        public static BonusStats operator -(BonusStats a, BonusStats b)
        {
            var sum = new BonusStats(a);
            sum.Hit -= b.Hit;
            sum.Avoid -= b.Avoid;
            sum.CritAvoid -= b.CritAvoid;
            sum.Crit -= b.Crit;
            sum.AttackSpeed -= b.AttackSpeed;
            sum.Armor -= b.Armor;
            sum.MagicResistance -= b.MagicResistance;
            sum.Attack -= b.Attack;
            return sum;

        }
        public int[] AsArray()
        {
            return new []{Attack,Hit,Avoid,AttackSpeed,Crit,CritAvoid,Armor, MagicResistance,};
        }

        public static string GetScalingText(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return "STR/INT";
                case CombatStatType.Avoid: return "AGI * "+BattleStats.AVO_AGI_MULT;
                case CombatStatType.Crit: return "LCK DEX";
                case CombatStatType.Critavoid: return "LCK * "+BattleStats.CRIT_AVO_LCK_MULT;
                case CombatStatType.Hit: return "DEX * " +BattleStats.HIT_DEX_MULT;
                case CombatStatType.MagicResistance: return "FTH";
                case CombatStatType.PhysicalResistance: return "DEF";
                case CombatStatType.AttackSpeed: return "AGI";
            }

            return "";
        }
    }
}