using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameResources;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [Serializable]
    public class Convoy
    {
        [NonSerialized] private List<StockedItem> items;
        [SerializeField]private List<string> itemIds;
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
                if (stockedItem.item.Equals(item)&& stockedItem.stock< item.maxStack)
                {
                    instock = true;
                    stockedItem.stock++;
                    break;
                }
            }
            if(!instock)
                Items.Add(new StockedItem(item, 1));
            convoyUpdated?.Invoke();
            UpdateStockCounts();
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
            UpdateStockCounts();
            convoyUpdated?.Invoke();
           
        }

        private void UpdateStockCounts()
        {
            for (int i=items.Count-1; i >=0; i--)
            {
                var sameItems=items.FindAll(a=> a.item.Equals(items[i].item));
                if (sameItems.Count > 1)
                {
                    sameItems.Sort((x,y)=>
                    {
                        if (x.stock == y.stock)
                            return 0;
                        return x.stock < y.stock ? 1 : -1;
                    });
                    var sameNotFullItems = sameItems.FindAll(a => a.stock < a.item.maxStack);
                    if (sameNotFullItems.Count <= 1)
                        break;
                    StockedItem fullestStock=sameNotFullItems[0];
                    StockedItem lowestStock = sameNotFullItems[sameItems.Count-1];
                    while (fullestStock.stock < fullestStock.item.maxStack && lowestStock.stock > 0)
                    {
                        fullestStock.stock += 1;
                        lowestStock.stock -= 1;

                    }

                    if (lowestStock.stock == 0)
                    {
                        items.Remove(lowestStock);
                    }

                   
                }
            }
            //Search for stocked items that exists multiple times
            // check if more than 1 of those has not maxed out stock Count
            //if yes move from lower stock count to higher stock count.
            //repeat
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

        
        public IEnumerable<StockedItem> GetAllGems()
        {
            return items.FindAll(a => a.item is Gem);
        }

        public int GetGemCount(Gem searchGem)
        {
            int sum = 0;
            foreach (var stockedGem in GetAllGems())
            {
                if (stockedGem.item == searchGem)
                {
                    sum += stockedGem.stock;
                }
            }

            return sum;
        }

        public void RemoveSmithingStones(int getStoneUpgradeCost)
        {
            for (int i = 0; i < getStoneUpgradeCost; i++)
            {
                RemoveItem(items.Find(i => i.item.Name== GameBPData.Instance.GetSmithingStone().Name).item);
            }
        }

        public void RemoveDragonScales(int getDragonScaleUpgradeCost)
        {
            for (int i = 0; i < getDragonScaleUpgradeCost; i++)
            {
                RemoveItem(items.Find(i => i.item.Name == GameBPData.Instance.GetDragonScale().Name).item);
            }
        }

        public int GetItemCount(Item item)
        {
            var stones=items.FindAll(i => i.item.Name == item.Name);
            int sum = 0;
            for (int i = 0; i < stones.Count; i++)
            {
                sum += stones[i].stock;
            }

            return sum;
        }
    }
}