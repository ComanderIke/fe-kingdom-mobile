using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    public class ExperienceManager
    {
        private const int MAX_EXP = 100;
        private const int EXP_PER_KILL = 40;
        private const int EXP_PER_BATTLE = 0;

        public int Level { get; set; }
        public int Exp { get; set; }

        public ExperienceManager()
        {
            Level = 1;
        }

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
