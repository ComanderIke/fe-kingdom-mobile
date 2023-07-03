using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class OnDebuff:PassiveSkill
    {
      
        public float procChance;
        public OnDebuff(string Name, string description, Sprite icon, GameObject animationObject,int tier, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.procChance = procChance;
        }
        public override void BindSkill(Unit unit)
        {
            // unit.OnDebuff += ReactToDebuff;
      
        }
        public override void UnbindSkill(Unit unit)
        {
            // unit.OnDebuff -= ReactToDebuff;
           
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Activation rate: ", (procChance*100f)+"%"));
            return list;
        }
        private void ReactToDebuff(Unit unit, CharStateEffects.Debuff debuff)
        {

            if (UnityEngine.Random.value <= procChance)
                Debug.Log("Remove Debuff once Debuff System exists");
        }
       
    }
}