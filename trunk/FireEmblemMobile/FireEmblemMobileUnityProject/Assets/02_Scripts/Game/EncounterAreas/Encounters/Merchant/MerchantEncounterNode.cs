using System.Collections.Generic;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Model;
using Game.GUI.EncounterUI.Merchant;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Merchant
{
    public class MerchantEncounterNode : EncounterNode
    {
        public Merchant merchant;
  
   
        public MerchantEncounterNode(List<EncounterNode> parents,int depth, int childIndex,string label, string description, Sprite sprite) : base(parents, depth, childIndex, label,description, sprite)
        {
            merchant = new Merchant(GameBPData.Instance.DefaultMerchantSprite, GameBPData.Instance.DefaultMerchantName, null);
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetItemByName("Health Potion"),Random.Range(2,4)));
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetRandomCommonConsumeables(),Random.Range(1,3)));
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetRandomCommonConsumeables(),Random.Range(1,2)));
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetRandomRareConsumeable()));
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetRandomGem()));
            // merchant.AddItem(new ShopItem(GameBPData.Instance.GetRandomGem()));
            //merchant.AddItem(new ShopItem(GameData.Instance.GetRandomPotion(),Random.Range(1,4)));


        }

        public override void Activate(Party party)
        {
            MyDebug.LogLogic("Visiting Merchant");
            base.Activate(party);
            GameObject.FindObjectOfType<UIMerchantController>().Show(merchant,party);
            UIMerchantController.OnFinished += MerchantFinished;
        
            //GameObject.FindObjectOfType<UIInnController>().Show(Player.Instance.Party);
        
        }

        void MerchantFinished()
        {
            UIMerchantController.OnFinished -= MerchantFinished;
            this.Continue();
       
        }
    }
}