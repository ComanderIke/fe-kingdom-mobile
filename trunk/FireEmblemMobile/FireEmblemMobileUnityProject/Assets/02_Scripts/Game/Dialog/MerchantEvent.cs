using System.Data;
using Game.GameActors.Items;
using Game.GameActors.Players;
using UnityEngine;

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