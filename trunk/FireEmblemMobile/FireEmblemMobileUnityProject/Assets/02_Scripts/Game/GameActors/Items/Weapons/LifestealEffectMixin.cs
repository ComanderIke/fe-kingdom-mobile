using System;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [CreateAssetMenu(menuName = "GameData/ItemEffects/LifeSteal", fileName = "Lifesteal")]
    public class LifestealEffectMixin : EffectMixin
    {
        public int lifeSteal = 1;
        public override string GetDescription()
        {
            return "recover " + lifeSteal + " hp per attack.";
        }
    }
}