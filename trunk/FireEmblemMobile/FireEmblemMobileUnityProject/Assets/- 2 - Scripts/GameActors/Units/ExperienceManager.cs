namespace Assets.GameActors.Units
{
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        private const int EXP_PER_KILL = 40;
        private const int EXP_PER_BATTLE = 0;

        public ExperienceManager()
        {
            Level = 1;
            NextLevelExp = MAX_EXP;
        }

        public int NextLevelExp { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }

        public void AddExp(int exp)
        {
            Exp += exp;
            if (Exp >= MAX_EXP)
            {
                Exp -= MAX_EXP;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Level++;
        }

        public void GetExpForKill()
        {
            AddExp(EXP_PER_KILL);
        }
    }
}