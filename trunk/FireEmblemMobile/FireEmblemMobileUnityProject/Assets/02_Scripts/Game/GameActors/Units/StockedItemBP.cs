using System;
using Game.GameActors.Items;
using Game.GUI.EncounterUI.Merchant;
using UnityEngine;

namespace Game.GameActors.Units
{
    [Serializable]
    public class StockedItemBP
    {
        [SerializeField] public int stock;
        [SerializeField] public ItemBP item;

        public StockedItem Create()
        {
            if (item == null)
                return null;
            return new StockedItem(item.Create(), stock);
        }
    }
}