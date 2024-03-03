using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Event;
using Game.GameActors.Items;
using Game.GameActors.Player;
using UnityEngine;

namespace Game.Dialog
{
    [CreateAssetMenu(menuName = "GameData/RewardsDialogEvent", fileName = "DialogEvent")]
    public class RewardEvent : DialogEvent
    {
        [SerializeField] int expReward;
        [SerializeField] int goldReward;
        [SerializeField] int graceReward;
        [SerializeField] List<ItemBP> itemRewards;

        public override void Action()
        {
            Player.Instance.Party.Money += goldReward;
            Player.Instance.Party.CollectedGrace += graceReward;
            Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(expReward);
            foreach (var itemBP in itemRewards)
            {
                Player.Instance.Party.Convoy.AddItem(itemBP.Create());
            }
       
        }

        public override Reward GetReward()
        {
            return null;
        }
    }
}