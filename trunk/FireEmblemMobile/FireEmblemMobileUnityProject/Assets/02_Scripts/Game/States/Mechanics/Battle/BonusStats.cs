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

        public void Add(BonusStats bonusStats)
        {
            Hit += bonusStats.Hit;
            Avoid += bonusStats.Avoid;
            CritAvoid += bonusStats.CritAvoid;
            Crit += bonusStats.Crit;
            AttackSpeed += bonusStats.AttackSpeed;
            Armor += bonusStats.Armor;
            MagicResistance += bonusStats.MagicResistance;
            Attack += bonusStats.Attack;
            
        }
        public void Decrease(BonusStats bonusStats)
        {
            Hit -= bonusStats.Hit;
            Avoid -= bonusStats.Avoid;
            CritAvoid -= bonusStats.CritAvoid;
            Crit -= bonusStats.Crit;
            AttackSpeed -= bonusStats.AttackSpeed;
            Armor -= bonusStats.Armor;
            MagicResistance -= bonusStats.MagicResistance;
            Attack -= bonusStats.Attack;
            
        }
    }
}