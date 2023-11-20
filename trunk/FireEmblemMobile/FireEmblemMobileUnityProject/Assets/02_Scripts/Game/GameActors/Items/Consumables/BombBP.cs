using System;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Bomb", fileName = "Bomb")]
    public class BombBP : ConsumableItemBp
    {
        //public Debuff appliedDebuff;
       // public int power;
      //  public int range;
      //  public int size;
       // public EffectType effectType;
        public SkillBp skill;
        //public SkillTargetArea targetArea;

        public override Item Create()
        {
            return new Bomb(skill.Create(),name, description, cost, rarity,maxStack,sprite);
        }
    }
}