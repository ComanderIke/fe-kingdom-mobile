using System;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    public class Attributes
    {
        public Action OnAttributesUpdated;
        [SerializeField]
        public int STR= 0;

        [SerializeField]
        public int INT = 0;
        [SerializeField]
        public int DEX= 0;
        [SerializeField]
        public int AGI= 0;

        [SerializeField] 
        public int CON = 0;
        [SerializeField]
        public int LCK= 0;
        [SerializeField]
        public int DEF= 0;
        [SerializeField]
        public int FAITH= 0;

        public const int CON_HP_Mult = 3;
        public Attributes()
        {
        }
        public Attributes(Attributes attributes)
        {
            INT = attributes.INT;
            CON = attributes.CON;
            DEX = attributes.DEX;
            AGI = attributes.AGI;
            STR = attributes.STR;
            FAITH = attributes.FAITH;
            DEF = attributes.DEF;
            LCK = attributes.LCK;
           
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
            STR += statIncreases[0];
            DEX += statIncreases[1];
            INT+= statIncreases[2];
            AGI += statIncreases[3];
            CON += statIncreases[4];
            LCK += statIncreases[5];
            DEF += statIncreases[6];
            FAITH += statIncreases[7];
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
            STR = 0;
            DEX = 0;
            INT = 0;
            AGI = 0;
            LCK = 0;
            CON = 0;
            DEF = 0;
            FAITH = 0;
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
        FTH
    }
}