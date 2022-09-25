using System;
using Game.GUI;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        public delegate void ExpGainedEvent(int expBefore, int expGained);
        public delegate void LevelupEvent();
        public ExpGainedEvent ExpGained;
        public LevelupEvent LevelUp;



        [SerializeField]
        private int level = 1;

        public int Level
        {
            get => level;
            set => level = value;
        }

        [SerializeField] private int exp;

        public int Exp
        {
            get => exp;
            set => exp = value;
        }

        public int MaxExp = 100;

        public const int MAX_EXP_TO_DRAIN = 100;
        [NonSerialized]
        public int ExpLeftToDrain = 100;

   

        public bool AddExp(int exp)
        {
            if (exp > MAX_EXP)
                exp = MAX_EXP;
            ExpGained?.Invoke(Exp, exp);
           return DoExp(exp);
            
        }

        private bool DoExp(int exp)
        {
            Debug.Log("Add Exp: " + exp);
            Exp += exp;
            Debug.Log("EXP: " + Exp);
            if (Exp >= MAX_EXP)
            {
                Exp -= MAX_EXP;
                PerformLevelUp();
                return true;
            }

            return false;
        }

        private void PerformLevelUp()
        {
            Debug.Log("Level Up");
            Level++;
            LevelUp?.Invoke();
        }

        public int GetMaxEXP(int exp)
        {
            if (exp > MAX_EXP)
                return MAX_EXP;
            return exp;
        }

        public bool HasLevelUp { get; set; }
        
    }
}