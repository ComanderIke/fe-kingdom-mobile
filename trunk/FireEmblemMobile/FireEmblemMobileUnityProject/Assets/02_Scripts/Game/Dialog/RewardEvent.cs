using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEngine;

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
}