using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Blessing", fileName = "Blessing")]
    public class BlessingBP : SkillBp
    {
        [SerializeField]private God god;
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
            return new Blessing(Name, Description, Icon, Tier, maxLevel, instantiatedPassiveMixins, combatSkillMixin==null?null:Instantiate(combatSkillMixin),instantiatedActiveMixins, SkillTransferData, god);
        }

    }
}