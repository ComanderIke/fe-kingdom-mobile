using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class MoralityPotion:ConsumableItem
    {
        private int strength;
        private GameObject effect;
        private bool reset;
        public MoralityPotion(string name, string description, int cost, int rarity, int maxStack,Sprite sprite, ItemTarget target, int strength, GameObject effect, bool reset):base (name, description, cost, rarity, maxStack,sprite, target)
        {
            this.strength = strength;
            this.effect = effect;
            this.reset = reset;
        }

        public static int ExtraHealAmount { get; set; }

        public override void Use(Unit character, Party party)
        {
            Debug.Log("Use Healthpotion!");
            if (reset)
                party.Morality.Reset();
            else
                party.Morality.AddMorality(strength);
            if (strength < 0 && !reset)
            {
                character.Stats.BaseAttributes.IncreaseAllAttributes(1);
            }
            //Different Animation in GridBattle
            if(character.GameTransformManager.GameObject!=null)
                GameObject.Instantiate(effect, character.GameTransformManager.GetCenterPosition(), Quaternion.identity,null);
            
            base.Use(character, party);
        }
    }
}