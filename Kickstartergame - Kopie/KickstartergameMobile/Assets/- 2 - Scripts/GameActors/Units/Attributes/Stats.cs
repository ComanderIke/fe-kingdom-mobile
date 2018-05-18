using System.Collections.Generic;


namespace Assets.Scripts.Characters.Attributes
{
    [System.Serializable]
    public class Stats
    {
        public int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value > MaxHP)
                    hp = MaxHP;
                else
                {
                    hp = value;
                }
                if (hp <= 0)
                {
                    hp = 0;
                }
                if(Unit.onHpValueChanged!=null)
                    Unit.onHpValueChanged();
            }
        }
        public int sp;
        public int SP
        {
            get
            {
                return sp;
            }
            set
            {
                if (value > MaxSP)
                    sp = MaxSP;
                else
                {
                    sp = value;
                }
                if (sp <= 0)
                {
                    sp = 0;
                }
            }
        }
        public int MaxHP { get; set; }
        public int Speed { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }
        public int Accuracy { get; set; }
        public int Spirit { get; set; }
        public int MaxSP { get; set; }
        public int MoveRange { get; set; }
        public List<int> AttackRanges { get; set; }

        public Stats(int hp, int sp, int attack, int speed, int defense, int accuracy, int spirit, int moveRange, List<int>attackRanges)
        {
            MaxHP = hp;
            Attack = attack;
            Speed = speed;
            Defense = defense;
            Accuracy = accuracy;
            Spirit = spirit;
            MaxSP = sp;
            HP = MaxHP;
            SP = MaxSP;
            MoveRange = moveRange;
            AttackRanges = attackRanges;
        }

        public int GetMaxAttackRange()
        {
            int max = 0;
            foreach (int attack in AttackRanges)
            {
                if (attack > max)
                    max = attack;
            }
            return max;
        }
    }
}
