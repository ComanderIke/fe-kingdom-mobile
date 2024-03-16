using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.States.Mechanics.Battle
{
    [Serializable]
    public class CombatStats
    {
        public enum CombatStatType
        {
            Attack,
            Hit,
            Avoid,
            AttackSpeed,
            Crit,
            CritAvoid,
            Protection,
            Resistance,
            CurseResistance
        }
        [field:SerializeField]
        public int Hit { get; set; }
        [field:SerializeField]
        public int Avoid { get; set; }
        [field:SerializeField]
        public int CritAvoid { get; set; }
        [field:SerializeField]
        public int CurseResistance { get; set; }
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

        public CombatStats()
        {
            
        }
        public CombatStats(CombatStats bonusStats)
        {
            Hit = bonusStats.Hit;
            Avoid = bonusStats.Avoid;
            CritAvoid = bonusStats.CritAvoid;
            Crit = bonusStats.Crit;
            AttackSpeed = bonusStats.AttackSpeed;
            Armor = bonusStats.Armor;
            MagicResistance = bonusStats.MagicResistance;
            Attack = bonusStats.Attack;
            CurseResistance = bonusStats.CurseResistance;

        }

        public override bool Equals(object obj)
        {
            if (obj is CombatStats combatStats)
            {
             
                return combatStats.AttackSpeed == AttackSpeed && combatStats.MagicResistance == MagicResistance && combatStats.Crit == Crit &&
                       combatStats.Armor == Armor && combatStats.CritAvoid == CritAvoid && combatStats.Hit == Hit &&
                       combatStats.Attack == Attack && combatStats.Avoid == Avoid&& combatStats.CurseResistance == CurseResistance;
                
            }
            return base.Equals(obj);
        }

        public static string GetAsText(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return "Attack";
                case CombatStatType.Avoid: return "Avoid";
                case CombatStatType.Crit: return "Critical";
                case CombatStatType.CritAvoid: return "Critavoid";
                case CombatStatType.Hit: return "Hitrate";
                case CombatStatType.Resistance: return "M. Resistance";
                case CombatStatType.Protection: return "Defense";
                case CombatStatType.AttackSpeed: return "Speed";
                case CombatStatType.CurseResistance: return "C. Resistance";
            }

            return "";
        }
        public static string GetAsShortText(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return "Atk";
                case CombatStatType.Avoid: return "Avo";
                case CombatStatType.Crit: return "Crit";
                case CombatStatType.CritAvoid: return "C. Avo";
                case CombatStatType.Hit: return "Hit";
                case CombatStatType.Resistance: return "M. Res";
                case CombatStatType.Protection: return "Prot";
                case CombatStatType.AttackSpeed: return "A. Spd";
                case CombatStatType.CurseResistance: return "C. Res.";
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
                case CombatStatType.CritAvoid: return CritAvoid;
                case CombatStatType.Hit: return Hit;
                case CombatStatType.Resistance: return MagicResistance;
                case CombatStatType.Protection: return Armor;
                case CombatStatType.AttackSpeed: return AttackSpeed;
                case CombatStatType.CurseResistance: return CurseResistance;
            }

            return 0;
        }

        public static CombatStats operator +(CombatStats a, CombatStats b)
        {
            var sum = new CombatStats(a);
            sum.Hit += b.Hit;
            sum.Avoid += b.Avoid;
            sum.CritAvoid += b.CritAvoid;
            sum. Crit += b.Crit;
            sum.AttackSpeed += b.AttackSpeed;
            sum.Armor += b.Armor;
            sum.MagicResistance += b.MagicResistance;
            sum.Attack += b.Attack;
            sum.CurseResistance += b.CurseResistance;
            return sum;

        }
        public static CombatStats operator -(CombatStats a, CombatStats b)
        {
            var sum = new CombatStats(a);
            sum.Hit -= b.Hit;
            sum.Avoid -= b.Avoid;
            sum.CritAvoid -= b.CritAvoid;
            sum.Crit -= b.Crit;
            sum.AttackSpeed -= b.AttackSpeed;
            sum.Armor -= b.Armor;
            sum.MagicResistance -= b.MagicResistance;
            sum.Attack -= b.Attack;
            sum.CurseResistance -= b.CurseResistance;
            return sum;

        }
        public static CombatStats operator *(CombatStats a, float b)
        {
            var sum = new CombatStats(a);
            sum.Hit = (int)(sum.Hit * b);
            sum.Avoid =(int)(sum.Avoid * b);
            sum.CritAvoid =(int)(sum.CritAvoid * b);
            sum.Crit =(int)(sum.Crit * b);
            sum.AttackSpeed= (int)(sum.AttackSpeed * b);
            sum.Armor =(int)(sum.Armor * b);
            sum.MagicResistance =(int)(sum.MagicResistance * b);
            sum.Attack =(int)(sum.Attack * b);
            sum.CurseResistance =(int)(sum.CurseResistance * b);
            return sum;

        }
        public int[] AsArray()
        {
            return new []{Attack,Hit,Avoid,AttackSpeed,Crit,CritAvoid,Armor, MagicResistance,CurseResistance};
        }

        public static string GetScalingText(CombatStatType type)
        {
            switch (type)
            {
                case CombatStatType.Attack: return "STR/INT";
                case CombatStatType.Avoid: return "AGI * "+BattleStats.AVO_AGI_MULT;
                case CombatStatType.Crit: return "LCK DEX";
                case CombatStatType.CritAvoid: return "LCK * "+BattleStats.CRIT_AVO_LCK_MULT;
                case CombatStatType.Hit: return "DEX * " +BattleStats.HIT_DEX_MULT;
                case CombatStatType.Resistance: return "FTH";
                case CombatStatType.Protection: return "DEF";
                case CombatStatType.AttackSpeed: return "AGI";
                case CombatStatType.CurseResistance: return "FTH *" + BattleStats.CURSE_RES_FTH_MULT;
            }

            return "";
        }

   
        private string SetCount(List<string> list, string add)
        {

            return add;
        }
        public List<string> GetToolTipLabels()
        {
            List<string> list = new List<string>();
            string firstlabel = "";
            int cnt = 0;
            int[] asArray = AsArray();
            int bonusCount = asArray.Count(a => a != 0);
            for (int i = 0; i < asArray.Length; i++)
            {
                if (asArray[i] != 0)
                {
                    firstlabel += "" + GetAsShortText((CombatStatType)i) + "/";
                    cnt++;
                }
                if (cnt >= 2||(i==asArray.Length-1&&cnt!=0)||(bonusCount<=2&&cnt!=0))
                {
                    cnt = 0;
                    if (firstlabel.Length > 0)
                        firstlabel= firstlabel.Remove(firstlabel.Length - 1, 1);
                    list.Add(firstlabel);
                    firstlabel = "";
                }
            }

            return list;
        }

        public List<string> GetToolTipValues()
        {
            List<string> list = new List<string>();
            string firstlabel = "";
            int cnt = 0;
            int[] asArray = AsArray(); 
            int bonusCount = asArray.Count(a => a != 0);
            for (int i = 0; i < asArray.Length; i++)
            {
                if (asArray[i] != 0)
                {
                    firstlabel += ( asArray[i]>0?"+":"")+asArray[i] + "/";
                    cnt++;
                }
                if (cnt >= 2||(i==asArray.Length-1&&cnt!=0)||(bonusCount<=2&&cnt!=0))
                {
                    cnt = 0;
                    if (firstlabel.Length > 0)
                        firstlabel= firstlabel.Remove(firstlabel.Length - 1, 1);
                    list.Add(firstlabel);
                    firstlabel = "";
                }
            }
            return list;
        }

       

        public int GetSum()
        {
            return Attack + AttackSpeed + Hit + Avoid + CritAvoid + Crit + Armor + CurseResistance + MagicResistance;
        }
    }
}