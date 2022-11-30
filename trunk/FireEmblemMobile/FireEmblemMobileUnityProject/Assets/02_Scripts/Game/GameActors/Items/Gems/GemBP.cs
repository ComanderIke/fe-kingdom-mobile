﻿using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    public enum GemType
    {
        HealthRegen,    //tanky
        ExtraExp,       //broken?
        Avoid,         //doguetank
        ScoutChanceAndCritAvoid,    //tanky but bad     crit immune on highest tier?
        CritChanceAndCritDamage,         //DPS
        ReflectDamage, //Good for Tanks
        SkillActivationRate,          //kinda dps
    }
    [CreateAssetMenu(menuName = "GameData/Items/Gem", fileName = "Gem")]
    public class GemBP: ItemBP
    {
        [SerializeField] private GemType gemType;
        public override Item Create()
        {
            return new Gem(name, description, cost, sprite,rarity , gemType);
        }

        
    }
}