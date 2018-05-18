using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(menuName = "GameData/Items/Mixins/Healing", fileName = "Healing")]
    public class HealingItemMixin : ItemMixin
    {
        public override void Use(Unit character)
        {
            throw new NotImplementedException();
        }
    }
}
