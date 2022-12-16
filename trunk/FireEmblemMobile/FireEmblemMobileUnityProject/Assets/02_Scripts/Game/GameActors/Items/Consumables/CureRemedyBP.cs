﻿using System;
using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/CureRemedy", fileName = "CureRemedy")]
    public class CureRemedyBP : ConsumableItemBp
    {
        public override Item Create()
        {
            return new CureRemedy(name, description, cost, rarity,sprite, target);

        }
    }
}