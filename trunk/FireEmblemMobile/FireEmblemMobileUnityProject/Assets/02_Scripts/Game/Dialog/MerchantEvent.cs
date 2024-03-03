using Game.EncounterAreas.Encounters.Event;
using Game.GameActors.Player;
using Game.GUI.EncounterUI.Merchant;
using UnityEngine;

namespace Game.Dialog
{
    [CreateAssetMenu(menuName = "GameData/MerchantEvent", fileName = "MerchantEvent")]
    public class MerchantEvent : DialogEvent
    {
  
        [SerializeField] public MerchantBP merchant;

        public override void Action()
        {
            FindObjectOfType<UIMerchantController>().Show(merchant.Create(), Player.Instance.Party);

        }

        public override Reward GetReward()
        {
            return null;
        }
    }
}