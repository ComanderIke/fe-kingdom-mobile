using Game.GameActors.Items;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace _02_Scripts.Game.GameActors.Items.Consumables
{
    public class Bomb : ConsumableItem
    {
        public Debuff appliedDebuff;
        public int power;
        public int range;
        public int size;
        
        public SkillTargetArea targetArea;
    public Bomb(int power, int range, int size, SkillTargetArea targetArea, Debuff appliedDebuff, string name, string description, int cost, int rarity,int maxStack,Sprite sprite) : base(name, description, cost,rarity, maxStack,sprite, ItemTarget.Position)
    {
        this.power = power;
        this.range = range;
        this.size = size;
        this.targetArea = targetArea;
        this.appliedDebuff = appliedDebuff;
    }
}
}