using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Progression
{
    [System.Serializable]
    public class ExperienceManager
    {
        public const int MAX_EXP = 100;
        public delegate void ExpGainedEvent(int expBefore, int expGained);
        public delegate void LevelupEvent();
        public ExpGainedEvent ExpGained;
        public LevelupEvent LevelUp;


        public ExperienceManager(ExperienceManager otherExpManager)
        {
            this.exp = otherExpManager.exp;
            this.level = otherExpManager.level;
            this.expLeftToDrain = otherExpManager.expLeftToDrain;
            this.drainableExp = otherExpManager.drainableExp;
            ExpMultipliers = otherExpManager.ExpMultipliers;
        }

        public ExperienceManager()
        {
            expLeftToDrain = drainableExp;
            ExpMultipliers = new List<float>();
        }

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

    

        public int drainableExp = 25;
[HideInInspector]
        public int expLeftToDrain;



        public bool AddExp(int exp)
        {
            if (exp == 0)
                return false;
            if (exp > MAX_EXP)
                exp = MAX_EXP;
            int sumExp = (int)(exp * Player.Player.Instance.Modifiers.ExperienceGain);
            foreach (var multiplier in ExpMultipliers)
            {
                sumExp += (int)(exp*multiplier);
            }
            ExpGained?.Invoke(Exp, exp);
           return DoExp(exp);
            
        }

        private bool DoExp(int exp)
        {
      
            Exp += exp;
          
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
  
        public List<float> ExpMultipliers { get; set; }
    }
}