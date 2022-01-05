﻿using Game.GameActors.Players;
using Game.GameResources;
using UnityEngine;

public class InnEncounterNode : EncounterNode
{
    public Inn inn;
    public InnEncounterNode(EncounterNode parent) : base(parent)
    {
        inn = new Inn(new Quest("Demo Quest", new QuestReward(QuestRewardType.Gold,100, null)), GameData.Instance.GetHuman("Elsa"));
        inn.AddItem(new ShopItem(25,GameAssets.Instance.visuals.InnSprites.drink , "Heal for 10%"));
        inn.AddItem(new ShopItem(50,GameAssets.Instance.visuals.InnSprites.meat , "Heal for 25%"));
        inn.AddItem(new ShopItem(0,GameAssets.Instance.visuals.InnSprites.rest , "Heal for 100% but skip a day!"));
    }

    public override void Activate()
    {
        GameObject.FindObjectOfType<UIInnController>().Show(Player.Instance.Party, inn);
        Debug.Log("Activate InnEncounterNode");
    }
}