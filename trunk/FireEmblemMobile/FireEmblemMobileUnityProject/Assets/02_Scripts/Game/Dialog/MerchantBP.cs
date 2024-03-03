using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Merchant;
using Game.GameActors.Items;
using UnityEngine;

namespace Game.Dialog
{
    [System.Serializable]
    public class ShopItemBp
    {
        public ItemBP item;
        public int stock;
    }
    [CreateAssetMenu(menuName = "GameData/Merchant", fileName = "Merchant")]
    public class MerchantBP:ScriptableObject
    {
        public List<ShopItemBp> items;
        [SerializeField] private Sprite merchantFace;
        [SerializeField] private string merchantName;
        public Merchant Create()
        {
            var merchant = new Merchant(merchantFace, merchantName, items);
        
            
       

        

            return merchant;
        }
    }
}