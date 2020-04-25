using System;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        public delegate void OnExpGainedEvent(int expBefore, int expGained);
        public delegate void OnLevelupEvent(int levelBefore, int levelAfter);
        public OnExpGainedEvent OnExpGained;
        public OnLevelupEvent OnLevelUp;
        public ExperienceManager()
        {
            Level = 1;
            NextLevelExp = MAX_EXP;
        }

        public int NextLevelExp { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int MaxEXPToDrain = 100;
        public int EXPLeftToDrain = 100;

        public void AddExp(int exp)
        {
            if (exp > MAX_EXP)
                exp = MAX_EXP;
            OnExpGained?.Invoke(Exp, exp);
            Exp += exp;
           
            if (Exp >= MAX_EXP)
            {
                Exp -= MAX_EXP;
                LevelUp();
            }
            
        }

        public void LevelUp()
        {
            Debug.Log("Level Up");
            OnLevelUp?.Invoke(Level, Level + 1);
            Level++;
            
        }
    }
}