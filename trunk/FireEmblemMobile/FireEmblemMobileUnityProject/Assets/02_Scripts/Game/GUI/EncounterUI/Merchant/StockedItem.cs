using System;
using System.Runtime.InteropServices.ComTypes;
using Game.GameActors.Items;
using UnityEngine.Serialization;

[Serializable]
public class StockedItem 
{
    [FormerlySerializedAs("itemBp")] public Item item;
    public int stock = 1;

    public StockedItem(Item item,int stock)
    {
        this.stock = stock;
        this.item = item;
    }

    public override bool Equals(object obj)
    {
        // Debug.Log("Compare");
        if (obj is StockedItem item)
        {
            //Debug.Log("Compare Items"+item.Name +" "+Name);
            if (item.item.Name == this.item.Name&&stock==item.stock)
                return true;
            // Debug.Log("Not Equal");
        }
            
        return base.Equals(obj);
    }


   
}