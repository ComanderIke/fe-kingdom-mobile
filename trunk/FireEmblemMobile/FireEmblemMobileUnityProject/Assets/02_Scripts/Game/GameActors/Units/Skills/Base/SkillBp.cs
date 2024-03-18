using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.EffectMixins;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
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
        public CombatSkillMixin combatSkillMixin;
        public List<ActiveSkillMixin> activeMixins;
        public int maxLevel = 5;

        public SkillTransferData SkillTransferData;

        protected List<PassiveSkillMixin> InitPassiveSkillMixins()
        {
            var instantiatedPassiveMixins = new List<PassiveSkillMixin>();
            if(passiveMixins!=null)
                foreach (var passive in passiveMixins)
                {
                    if (passive != null)
                    {
                        var instPassive = Instantiate(passive);
                        instPassive.Init();
                        instantiatedPassiveMixins.Add(instPassive);
                    }
                        
                }

            return instantiatedPassiveMixins;
        }

        protected List<ActiveSkillMixin> InitActiveSkillMixins()
        {
            var instantiatedActiveMixins = new List<ActiveSkillMixin>();
            if(activeMixins!=null)
                foreach (var active in activeMixins)
                {
                    if (active != null)
                    {
                        var instActive = Instantiate(active);
                        instActive.Init();
                        instantiatedActiveMixins.Add(instActive);
                    }
                       
                }

            return instantiatedActiveMixins;
        }

        protected CombatSkillMixin InitCombatSkillMixin()
        {
            
            CombatSkillMixin instCombatskill = null;
            if (combatSkillMixin != null)
            {
                instCombatskill = Instantiate(combatSkillMixin);
                instCombatskill.Init();
            }

            return instCombatskill;
        }
        public virtual Skill Create()
        {
            return new Skill(Name, Description, Icon, Tier,maxLevel, InitPassiveSkillMixins(),InitCombatSkillMixin(), InitActiveSkillMixins(),SkillTransferData);
        }
    }
}