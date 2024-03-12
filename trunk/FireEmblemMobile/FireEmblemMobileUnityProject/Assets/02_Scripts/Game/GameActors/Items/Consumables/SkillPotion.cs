using Game.EncounterAreas.Model;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public class SkillPotion:ConsumableItem, IEquipableCombatItem
    {
        public int strength;
        public GameObject effect;

        public SkillPotion(string name, string description, int cost, int rarity, int maxStack,Sprite sprite, ItemTarget target, int strength, GameObject effect):base (name, description, cost, rarity, maxStack,sprite, target)
        {
            this.strength = strength;
            this.effect = effect;
        }

        public static int ExtraHealAmount { get; set; }

        public override void Use(Unit character, Party convoy)
        {
            Debug.Log("Use Healthpotion!");
            character.SkillManager.AddSkillPoints(strength+ ExtraHealAmount);
            //Different Animation in GridBattle
            if(character.GameTransformManager.GameObject!=null)
                GameObject.Instantiate(effect, character.GameTransformManager.GetCenterPosition(), Quaternion.identity,null);
            
            base.Use(character, convoy);
        }
    }
}