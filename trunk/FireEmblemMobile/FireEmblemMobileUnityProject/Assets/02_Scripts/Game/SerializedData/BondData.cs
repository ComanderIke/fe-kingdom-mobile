using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameResources;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class BondData
    {
        [SerializeField] public List<string> godIds;
        [SerializeField] public List<int> bondExp;
        [SerializeField] public List<int> bondLevels;

        public BondData(Bonds unitBonds, List<God> gods)
        {
            godIds = new List<string>();
            bondLevels = new List<int>();
            bondExp = new List<int>();
            foreach (var god in gods)
            {
                godIds.Add(god.Name);
                bondLevels.Add(unitBonds.GetBondLevel(god));
                bondExp.Add(unitBonds.GetBondExperience(god));
            }
           
        }

        public Bonds Load()
        {
            Bonds bonds = new Bonds();
            for (int i =0; i<  godIds.Count; i++)
            {
                God god = GameBPData.Instance.GetGod(godIds[i]);
                BondExperience exp = new BondExperience();
                exp.level = bondLevels[i];
                exp.experience = bondExp[i];
                bonds.Experiences.Add(god, exp);
            }
                
            
            return bonds;
        }
    }
}