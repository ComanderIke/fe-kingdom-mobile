using UnityEngine;

namespace Game.GameActors.Units
{
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        public delegate void ExpGainedEvent(int expBefore, int expGained);
        public delegate void LevelupEvent(int levelBefore, int levelAfter);
        public ExpGainedEvent ExpGained;
        public LevelupEvent LevelUp;
        public ExperienceManager()
        {
            Level = 1;
            NextLevelExp = MAX_EXP;
        }

        public int NextLevelExp { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public const int MAX_EXP_TO_DRAIN = 100;
        public int ExpLeftToDrain = 100;

        public void AddExp(int exp)
        {
            if (exp > MAX_EXP)
                exp = MAX_EXP;
            ExpGained?.Invoke(Exp, exp);
            Exp += exp;
           
            if (Exp >= MAX_EXP)
            {
                Exp -= MAX_EXP;
                PerformLevelUp();
            }
            
        }

        private void PerformLevelUp()
        {
            Debug.Log("Level Up");
            LevelUp?.Invoke(Level, Level + 1);
            Level++;
            
        }
    }
}