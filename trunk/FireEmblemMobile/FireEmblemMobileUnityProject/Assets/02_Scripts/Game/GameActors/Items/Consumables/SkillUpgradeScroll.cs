using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class SkillUpgradeScroll : ConsumableItem
    {
        private bool random;
        public SkillUpgradeScroll(bool random,string name, string description, int cost,int rarity, int maxStack,Sprite sprite, ItemTarget target) : base(name, description, cost, rarity,maxStack,sprite, target)
        {
            this.random = random;
        }

        public override void Use(Unit character, Party convoy)
        {
            character.SkillManager.UpgradeSkill(random);
            //ServiceProvider.Instance.GetSystem<UnitProgressSystem>().LearnNewSkill(character);
            base.Use(character, convoy);
        }
    }
}