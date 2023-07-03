using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;
using Random = System.Random;

namespace LostGrace
{
    public class Miracle : PassiveSkill
    {

        private Unit owner;
        [SerializeField] float procChance;

      public Miracle(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, tier, upgradeDescr)
        {
            this.procChance = procChance;
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Activation rate: ", (procChance*100f).ToString()+"%"));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
           // unit.OnLethalDamage += ReactToBeforeDeath;
        }
        public override void UnbindSkill(Unit unit)
        {
           // unit.OnLethalDamage -= ReactToBeforeDeath;
            this.owner = null;
        }
        private void ReactToBeforeDeath()
        {
            Debug.Log("TODO Event Before Death but after damage received");
            if(UnityEngine.Random.value<=procChance)
                owner.Hp = 1;
        }
      
    }
}