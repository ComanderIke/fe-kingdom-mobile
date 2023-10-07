using System.Collections.Generic;
using LostGrace;

namespace Game.GameActors.Units
{
    public struct BondExperience
    {
        public int level;
        public int experience;
    }
    public class Bonds
    {
        private Dictionary<God, BondExperience> Experiences;

        public int GetBondLevel(God god)
        {
            return Experiences[god].level;
        }

        public int GetBondExperience(God god)
        {
            return Experiences[god].experience;
        }
    }
    
}