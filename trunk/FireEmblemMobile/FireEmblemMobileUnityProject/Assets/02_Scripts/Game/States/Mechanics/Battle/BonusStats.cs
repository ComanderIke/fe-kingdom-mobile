using UnityEngine;

namespace Game.Mechanics.Battle
{
    [System.Serializable]
    public class BonusStats
    {
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
            return new []{Hit,Avoid,Crit,CritAvoid,AttackSpeed,Attack, MagicResistance,Armor};
        }
       
    }
}