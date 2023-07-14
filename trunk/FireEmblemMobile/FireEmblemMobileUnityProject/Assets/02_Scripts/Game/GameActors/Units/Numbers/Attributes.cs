using System;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    public class Attributes
    {
        public Action OnAttributesUpdated;
        [SerializeField]
        private int str= 0;

        [SerializeField]
        private int intel = 0;
        [SerializeField]
        private int dex= 0;
        [SerializeField]
        private int agi= 0;

        [SerializeField] 
        private int con = 0;

        public Action AttributesUpdated => OnAttributesUpdated;

        public int STR => str;

        public int INT => intel;

        public int DEX => dex;

        public int AGI => agi;

        public int CON => con;

        public int LCK => lck;

        public int DEF => def;

        public int FAITH => faith;

        [SerializeField]
        private int lck= 0;
        [SerializeField]
        private int def= 0;
        [SerializeField]
        private int faith= 0;

        public const int CON_HP_Mult = 1;
        public const int BASE_HP = 15;

        public Attributes()
        {
        }
        public Attributes(Attributes attributes)
        {
            intel = attributes.INT;
            con = attributes.CON;
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
            sum.con += b.con;
            sum.dex += b.dex;
            sum.intel += b.intel;
            sum.faith += b.def;
            return sum;
        }
        public static Attributes operator -(Attributes a, Attributes b)
        {
            var sum = new Attributes(a);
            sum.agi -= b.agi;
            sum.str -= b.str;
            sum.def -= b.def;
            sum.lck -= b.lck;
            sum.con -= b.con;
            sum.dex -= b.dex;
            sum.intel -= b.intel;
            sum.faith -= b.def;
            return sum;
        }
        public Attributes Clone()
        {
            //Debug.Log("CloneAttriubtes");
            return new Attributes(this);
            
        }

        public int[] AsArray()
        {
            return new []{STR,DEX,INT,AGI,CON,LCK, DEF,FAITH};
        }

        public void Update(int[] statIncreases)
        {
            str += statIncreases[0];
            dex += statIncreases[1];
            intel+= statIncreases[2];
            agi += statIncreases[3];
            con += statIncreases[4];
            lck += statIncreases[5];
            def += statIncreases[6];
            faith += statIncreases[7];
            OnAttributesUpdated?.Invoke();
        }

        public static object GetAsText(int textoptionStatIndex)
        {
            switch (textoptionStatIndex)
            {
                case 0: return "STR";
                case 1: return "DEX";
                case 2: return "INT";
                case 3: return "AGI";
                case 4: return "CON";
                case 5: return "LCK";
                case 6: return "DEF";
                case 7: return "FAITH";
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
            con = 0;
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
                    return con; 
                case AttributeType.LCK:
                    return lck; 
                case AttributeType.DEF:
                    return def; 
                case AttributeType.FTH:
                    return faith; 
            }

            return 0;
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
                    con += value; break;
                case AttributeType.LCK:
                    lck += value; break;
                case AttributeType.DEF:
                    def += value; break;
                case AttributeType.FTH:
                    faith += value; break;
            }
            OnAttributesUpdated?.Invoke();
        }

        public void Add(Attributes bonusAttributes)
        {
            str += bonusAttributes.STR;
            dex += bonusAttributes.DEX;
            intel += bonusAttributes.INT;
            agi += bonusAttributes.AGI;
            con += bonusAttributes.CON;
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
            con -= bonusAttributes.CON;
            lck -= bonusAttributes.LCK;
            def -= bonusAttributes.DEF;
            faith -= bonusAttributes.FAITH;
            OnAttributesUpdated?.Invoke();
        }
    }

    public enum AttributeType
    {
        NONE,
        STR,
        DEX,
        INT,
        AGI,
        CON,
        LCK,
        DEF,
        FTH,
        LVL
    }
}