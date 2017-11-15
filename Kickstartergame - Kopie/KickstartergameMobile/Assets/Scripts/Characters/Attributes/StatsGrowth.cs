using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Attributes
{
    public class StatsGrowth
    {
        public int HPGrowth { get; set; }
        public int SpeedGrowth { get; set; }
        public int DefenseGrowth { get; set; }
        public int AttackGrowth { get; set; }
        public int AccuracyGrowth { get; set; }
        public int SpiritGrowth { get; set; }
        public int SPGrowth { get; set; }

        public StatsGrowth(int hpGrowth, int spGrowth, int attackGrowth, int speedGrowth, int defenseGrowth, int accuracyGrowth, int spiritGrowth)
        {
            HPGrowth = hpGrowth;
            AttackGrowth = attackGrowth;
            SpeedGrowth = speedGrowth;
            DefenseGrowth = defenseGrowth;
            AccuracyGrowth = accuracyGrowth;
            SpiritGrowth = spiritGrowth;
            SPGrowth = spGrowth;
        }
    }
}
