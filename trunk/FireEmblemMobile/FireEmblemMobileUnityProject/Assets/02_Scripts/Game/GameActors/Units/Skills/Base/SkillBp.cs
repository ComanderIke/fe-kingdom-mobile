﻿using System;
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
        public List<ActiveSkillMixin> activeMixins;
        public int maxLevel = 5;

        public SkillTransferData SkillTransferData;
        public virtual Skill Create()
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
          
           
            return new Skill(Name, Description, Icon, Tier,maxLevel, instantiatedPassiveMixins, instantiatedActiveMixins,SkillTransferData);
        }
    }
}