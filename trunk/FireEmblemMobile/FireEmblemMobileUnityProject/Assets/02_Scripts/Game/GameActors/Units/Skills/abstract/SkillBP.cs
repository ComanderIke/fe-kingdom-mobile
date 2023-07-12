using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(fileName = "Skill", menuName = "GameData/Skills/Skill")]
    public class SkillBp:  ScriptableObject
    {
        public Sprite Icon;
        public string Description;
        public string Name;
        public int Tier;
        public List<PassiveSkillMixin> passiveMixins;
        public ActiveSkillMixin activeMixin;
        public int maxLevel = 5;

        public virtual Skill Create()
        {
            var instantiatedPassiveMixins = new List<PassiveSkillMixin>();
            if(passiveMixins!=null)
                foreach (var passive in passiveMixins)
                {
                    instantiatedPassiveMixins.Add(Instantiate(passive));
                }

            var instantiatedActiveMixin = activeMixin == null ? null : Instantiate(activeMixin);
           
            return new Skill(Name, Description, Icon, Tier,maxLevel, instantiatedPassiveMixins, instantiatedActiveMixin);
        }
    }
}