using System;
using Game.GameActors.Items;
using UnityEngine;

namespace Game.GameActors.Units
{
    [Serializable]
    public class StockedItemBP
    {
        [SerializeField] private int stock;
        [SerializeField] private ItemBP item;

        public StockedItem Create()
        {
            return new StockedItem(item.Create(), stock);
        }
    }
}