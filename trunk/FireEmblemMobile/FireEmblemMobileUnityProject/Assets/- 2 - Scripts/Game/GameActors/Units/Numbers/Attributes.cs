using System;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    public class Attributes
    {
        [SerializeField]
        public int STR= 30;

        [SerializeField]
        public int INT = 30;
        [SerializeField]
        public int DEX= 30;
        [SerializeField]
        public int AGI= 30;
        [SerializeField]
        public int CON= 30;
        [SerializeField]
        public int FAITH= 30;

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
        }

        public Attributes Clone()
        {
            //Debug.Log("CloneAttriubtes");
            return new Attributes(this);
            
        }

        public int[] AsArray()
        {
            return new []{STR,INT,DEX,AGI,CON,FAITH};
        }
    }
}