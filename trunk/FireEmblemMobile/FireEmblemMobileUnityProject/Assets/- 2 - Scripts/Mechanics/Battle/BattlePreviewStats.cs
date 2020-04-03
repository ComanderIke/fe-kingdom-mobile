using System.Collections.Generic;

namespace Assets.Mechanics.Battle
{
    public class BattlePreviewStats
    {
        public int Attack { get; set; }
        public int Speed { get; set; }
        public bool IsPhysical { get; set; }
        public int Defense { get; set; }
        public int Skill { get; set; }
        public int Damage { get; set; }
        public int AttackCount { get; set; }
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int AfterBattleHp { get; set; }
        public List<int> IncomingDamage { get; set; }
        public int CurrentSp { get; set; }
        public int MaxSp { get; set; }
        public int AfterBattleSp { get; set; }
        public List<int> IncomingSpDamage { get; set; }

        public BattlePreviewStats(int attack, int speed, bool isPhysical, int defense, int skill, int damage, int attackCount, int currentHp, int maxHp, int afterBattleHp, List<int> incomingDamage, int currentSp, int maxSp, int afterBattleSp, List<int> incomingSpDamage)
        {
            Attack = attack;
            Speed = speed;
            IsPhysical = isPhysical;
            Defense = defense;
            Skill = skill;
            Damage = damage;
            AttackCount = attackCount;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            AfterBattleHp = afterBattleHp;
            IncomingDamage = incomingDamage;
            CurrentSp = currentSp;
            MaxSp = maxSp;
            AfterBattleSp = afterBattleSp;
            IncomingSpDamage = incomingSpDamage;
        }
    }
}
