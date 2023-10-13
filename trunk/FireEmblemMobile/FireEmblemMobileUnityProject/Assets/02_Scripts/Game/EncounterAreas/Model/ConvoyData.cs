using System;
using System.Collections.Generic;
using Game.GameResources;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [Serializable]
    public class ConvoyData
    {
        [SerializeField]private List<string> itemIds;
        [SerializeField]private List<int> stock;

        public ConvoyData(Convoy convoy)
        {
            itemIds = new List<string>();
            stock = new List<int>();
            foreach (var item in convoy.Items)
            {
                itemIds.Add(item.item.Name);
                stock.Add(item.stock);
            }
        }

        public Convoy LoadData()
        {
            Convoy retConvoy = new Convoy();
            int index = 0;
            foreach (var itemId in itemIds)
            {
                retConvoy.AddItem(new StockedItem(GameBPData.Instance.GetItemByName(itemId),stock[index]));
                index++;
            }

            return retConvoy;
        }
    }
}