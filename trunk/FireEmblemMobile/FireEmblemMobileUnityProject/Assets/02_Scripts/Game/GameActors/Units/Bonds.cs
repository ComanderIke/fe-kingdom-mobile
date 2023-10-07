using System.Collections.Generic;
using LostGrace;

namespace Game.GameActors.Units
{
    public class BondExperience
    {
        public int level;
        public int experience;

        public BondExperience()
        {
            level = 1;
            experience = 0;
        }
    }
    public class Bonds
    {
        private Dictionary<God, BondExperience> Experiences;

        public Bonds()
        {
            Experiences = new Dictionary<God, BondExperience>();
        }
        public int GetBondLevel(God god)
        {
            if(!Experiences.ContainsKey(god))
                Experiences.Add(god, new BondExperience());
            return Experiences[god].level;
        }

        public int GetBondExperience(God god)
        {
            if(!Experiences.ContainsKey(god))
                Experiences.Add(god, new BondExperience());
            return Experiences[god].experience;
        }

        public object Clone()
        {
            Bonds bonds = new Bonds();
            foreach (KeyValuePair<God, BondExperience> kv in Experiences)
            {
                bonds.Experiences.Add(kv.Key,kv.Value);
            }

            return bonds;
        }
    }
    
}