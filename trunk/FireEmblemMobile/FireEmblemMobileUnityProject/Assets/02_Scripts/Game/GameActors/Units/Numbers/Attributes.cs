using System;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    public class Attributes
    {
        [SerializeField]
        private int str;

       
        [SerializeField]
        private int dex;
        [SerializeField]
        private int intel;
        [SerializeField]
        private int agi;

        [SerializeField]
        private int lck;
       
        [SerializeField]
        private int def;
        [SerializeField]
        private int faith;
        [SerializeField] 
        private int maxHp;

        public event Action OnAttributesUpdated;

        public int STR => str;

        public int INT => intel;

        public int DEX => dex;

        public int AGI => agi;

        public int MaxHp => maxHp;

        public int LCK => lck;

        public int DEF => def;

        public int FAITH => faith;

       
        [SerializeField] public int sum;


        
        public Attributes()
        {
        }
        public Attributes(Attributes attributes)
        {
            intel = attributes.INT;
            maxHp = attributes.MaxHp;
            dex = attributes.DEX;
            agi = attributes.AGI;
            str = attributes.STR;
            faith = attributes.FAITH;
            def = attributes.DEF;
            lck = attributes.LCK;
           
        }
        
        public static Attributes operator +(Attributes a, Attributes b)
        {
            var sum = new Attributes(a);
            sum.agi += b.agi;
            sum.str += b.str;
            sum.def += b.def;
            sum.lck += b.lck;
            sum.maxHp += b.maxHp;
            sum.dex += b.dex;
            sum.intel += b.intel;
            sum.faith += b.faith;
            return sum;
        }
        public static Attributes operator -(Attributes a, Attributes b)
        {
            var sum = new Attributes(a);
            sum.agi -= b.agi;
            sum.str -= b.str;
            sum.def -= b.def;
            sum.lck -= b.lck;
            sum.maxHp -= b.maxHp;
            sum.dex -= b.dex;
            sum.intel -= b.intel;
            sum.faith -= b.faith;
            return sum;
        }
        public Attributes Clone()
        {
            //Debug.Log("CloneAttriubtes");
            return new Attributes(this);
            
        }

        public const int MaxHPIndex = 4;
        public int[] AsArray()
        {
            return new []{STR,DEX,INT,AGI,MaxHp,LCK, DEF,FAITH};
        }

        public void Update(int[] statIncreases)
        {
            str += statIncreases[0];
            dex += statIncreases[1];
            intel+= statIncreases[2];
            agi += statIncreases[3];
            maxHp += statIncreases[4];
            lck += statIncreases[5];
            def += statIncreases[6];
            faith += statIncreases[7];
            OnAttributesUpdated?.Invoke();
        }

        public static string GetAsText(int textoptionStatIndex)
        {
            switch (textoptionStatIndex)
            {
                case 0: return "STR";
                case 1: return "DEX";
                case 2: return "INT";
                case 3: return "AGI";
                case 4: return "MaxHP";
                case 5: return "LCK";
                case 6: return "DEF";
                case 7: return "FTH";
                default: return "?";
            }
        }
        public static string GetAsLongText(int attribute)
        {
            switch (attribute)
            {
                case 0: return "Strength";
                case 1: return "Dexterity";
                case 2: return "Intelligence";
                case 3: return "Agility";
                case 4: return "Maximum HP";
                case 5: return "Luck";
                case 6: return "Defense";
                case 7: return "Faith";
                default: return "?";
            }
        }

        public int GetFromIndex(int textoptionStatIndex)
        {
            return AsArray()[textoptionStatIndex];
        }

        public void Reset()
        {
            str = 0;
            dex = 0;
            intel = 0;
            agi = 0;
            lck = 0;
            maxHp = 0;
            def = 0;
            faith = 0;
        }

        public int GetAttributeStat(AttributeType type)
        {
            switch (type)
            {
                case AttributeType.STR:
                    return str; 
                case AttributeType.DEX:
                    return dex; 
                case AttributeType.INT:
                    return intel; 
                case AttributeType.AGI:
                    return agi; 
                case AttributeType.CON:
                    return maxHp; 
                case AttributeType.LCK:
                    return lck; 
                case AttributeType.DEF:
                    return def; 
                case AttributeType.FTH:
                    return faith; 
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Attributes attributes)
            {
                return attributes.MaxHp == MaxHp && attributes.str == str && attributes.intel == intel &&
                       attributes.dex == dex && attributes.lck == lck && attributes.faith == faith &&
                       attributes.agi == agi && attributes.def == def;
            }
            return base.Equals(obj);
        }

        public void IncreaseAttribute(int value, AttributeType attributeType)
        {
            switch (attributeType)
            {
                case AttributeType.STR:
                    str += value; break;
                case AttributeType.DEX:
                    dex += value; break;
                case AttributeType.INT:
                    intel += value; break;
                case AttributeType.AGI:
                    agi += value; break;
                case AttributeType.CON:
                    maxHp += value; break;
                case AttributeType.LCK:
                    lck += value; break;
                case AttributeType.DEF:
                    def += value; break;
                case AttributeType.FTH:
                    faith += value; break;
                case AttributeType.ALL:
                    str += value; 
                    dex += value;
                    intel += value;
                    agi += value;
                    maxHp += value;
                    lck += value;
                    def += value;
                    faith += value;
                    break;
            }
            OnAttributesUpdated?.Invoke();
        }

        public void Add(Attributes bonusAttributes)
        {
            str += bonusAttributes.STR;
            dex += bonusAttributes.DEX;
            intel += bonusAttributes.INT;
            agi += bonusAttributes.AGI;
            maxHp += bonusAttributes.MaxHp;
            lck += bonusAttributes.LCK;
            def += bonusAttributes.DEF;
            faith += bonusAttributes.FAITH;
            OnAttributesUpdated?.Invoke();
            
        }

        public void Decrease(Attributes bonusAttributes)
        {
            str -= bonusAttributes.STR;
            dex -= bonusAttributes.DEX;
            intel -= bonusAttributes.INT;
            agi -= bonusAttributes.AGI;
            maxHp -= bonusAttributes.MaxHp;
            lck -= bonusAttributes.LCK;
            def -= bonusAttributes.DEF;
            faith -= bonusAttributes.FAITH;
            OnAttributesUpdated?.Invoke();
        }


        public string GetTooltipText()
        {
            string attributeslabel= "" + (STR != 0
                          ? GetAsText(0) + "/"
                          : "") //  either grant STR/SPD/SKL   5/4/3 -> 5/5/
                      + (DEX != 0 ? GetAsText(1) + "/" : "")
                      + (INT != 0 ? GetAsText(2) + "/" : "")
                      + (AGI != 0 ? GetAsText(3) + "/" : "")
                      + (MaxHp != 0 ? GetAsText(4) + "/" : "")
                      + (LCK != 0 ? GetAsText(5) + "/" : "")
                      + (DEF != 0 ? GetAsText(6) + "/" : "")
                      + (FAITH != 0 ? GetAsText(7) + "/" : "");
            if (attributeslabel.Length > 0)
                return attributeslabel.Remove(attributeslabel.Length - 1, 1);
            return attributeslabel;
        }

        public string GetTooltipValue()
        {
           string valueLabel= "" + (STR != 0
                   ? STR + "/"
                   : "") //  either grant STR/SPD/SKL   5/4/3 -> 5/5/
               + (DEX != 0 ? DEX + "/" : "")
               + (INT != 0 ? INT + "/" : "")
               + (AGI != 0 ? AGI + "/" : "")
               + (MaxHp != 0 ? MaxHp + "/" : "")
               + (LCK != 0 ? LCK + "/" : "")
               + (DEF != 0 ? DEF + "/" : "")
                + (FAITH != 0 ? FAITH + "/" : "");
            if (valueLabel.Length > 0)
                return valueLabel.Remove(valueLabel.Length - 1, 1);
            return valueLabel;
        }

        public void Clear()
        {
            str = 0;
            def = 0;
            dex = 0;
            intel = 0;
            faith = 0;
            agi = 0;
            maxHp = 0;
            lck = 0;
        }

        public int GetSum(bool hpHalf=false)
        {
            return STR + DEX + INT + LCK + AGI + DEF + FAITH + (hpHalf?MaxHp/2:MaxHp);
        }
    }

    public enum AttributeType
    {
        STR,
        DEX,
        INT,
        AGI,
        CON,
        LCK,
        DEF,
        FTH,
        LVL,
        NONE,
        ATK,
        ALL
    }
}