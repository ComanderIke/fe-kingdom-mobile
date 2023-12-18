using Game.GameActors.Units.Skills;
using UnityEngine;

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
        [SerializeField] private SkillBp gemEffect;
        [SerializeField] private int soulCapacity = 20;
        [SerializeField] private int startSouls = 0;
        [SerializeField] private GemBP upgradeTo;
        public override Item Create()
        {
            Skill instSkill = null;
            if (gemEffect != null)
                instSkill = gemEffect.Create();
            if(upgradeTo!=null)
                return new Gem(name, description, cost, maxStack,sprite,rarity , instSkill,startSouls,soulCapacity,(Gem) upgradeTo.Create());
            return new Gem(name, description, cost, maxStack,sprite,rarity , instSkill,startSouls,soulCapacity, null);
        }

        
    }
}