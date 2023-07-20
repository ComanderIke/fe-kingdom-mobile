using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Curse", fileName = "Curse")]
    public class CurseBP : SkillBp
    {
        public override Skill Create()
        {
            var instantiatedPassiveMixins = new List<PassiveSkillMixin>();
            if(passiveMixins!=null)
                foreach (var passive in passiveMixins)
                {
                    instantiatedPassiveMixins.Add(Instantiate(passive));
                }
            var instantiatedActiveMixins = new List<ActiveSkillMixin>();
            if(activeMixins!=null)
                foreach (var active in activeMixins)
                {
                    instantiatedActiveMixins.Add(Instantiate(active));
                }
            return new Curse(Name, Description, Icon, Tier, maxLevel, instantiatedPassiveMixins, instantiatedActiveMixins, SkillTransferData);
        }

    }
}