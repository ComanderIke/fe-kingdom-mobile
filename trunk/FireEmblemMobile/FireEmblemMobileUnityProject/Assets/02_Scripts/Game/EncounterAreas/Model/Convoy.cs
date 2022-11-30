using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [Serializable]
    public class Convoy
    {
        [SerializeField] private List<ItemBP> startItems;
        [SerializeField] private List<StockedItem> items;
        [SerializeField] public event Action convoyUpdated;

        public List<StockedItem> Items
        {
            get
            {
                if (items == null)
                    items = new List<StockedItem>();
                return items;
            }
        } 
        

        public void Init()
        {
            if (startItems != null)
            {
                foreach (var itemBp in startItems)
                {
                    AddItem(itemBp.Create());
                }
            }
        }

        public override string ToString()
        {
            string convoy = "";
            convoy += "StockedItemCount: " + Items.Count;
            foreach (var stock in Items)
            {
                convoy += stock.item.ToString() + "x" + stock.stock;
            }
            return convoy;
        }
        public void AddItem(Item item)
        {
            bool instock = false;
            foreach (var stockedItem in Items)
            {
                if (stockedItem.item.Equals(item))
                {
                    instock = true;
                    stockedItem.stock++;
                    break;
                }
            }
            if(!instock)
                Items.Add(new StockedItem(item, 1));
            convoyUpdated?.Invoke();
        }
        public void RemoveItem(Item item)
        {
            
            StockedItem removeItem=null;
            foreach (var stockedItem in Items)
            {
                if (stockedItem.item.Equals(item))
                {
                    stockedItem.stock--;
                    if (stockedItem.stock <= 0)
                        removeItem = stockedItem;
                    break;

                }
            }

            if (removeItem != null)
                Items.Remove(removeItem);
            convoyUpdated?.Invoke();
           
        }

        public void AddStockedItem(StockedItem stockedItem)
        {
            items.Add(stockedItem);
        }

        public bool ContainsItem(Item item)
        {
            foreach (var stockedItem in Items)
            {
                if (stockedItem.item.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}