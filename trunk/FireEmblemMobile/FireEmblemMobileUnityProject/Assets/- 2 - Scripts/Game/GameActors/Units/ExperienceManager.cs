using System;
using Game.GUI;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        public delegate void ExpGainedEvent(Vector3 drainedPos, int expBefore, int expGained);
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

        public void AddExp(Vector3 drainPos, int exp)
        {
            if (exp > MAX_EXP)
                exp = MAX_EXP;
            ExpGained?.Invoke(drainPos, Exp, exp);
            Debug.Log("Add Exp: " + exp);
            Exp += exp;
            Debug.Log("EXP: " + Exp);
            if (Exp >= MAX_EXP)
            {
                Exp -= MAX_EXP;
                PerformLevelUp();
            }
            
        }

        private void PerformLevelUp()
        {
            Debug.Log("Level Up");
            Level++;
            LevelUp?.Invoke();
            
           
            
            
        }
    }
}