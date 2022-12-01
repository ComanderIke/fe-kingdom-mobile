﻿using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class ChurchEncounterNode : EncounterNode
{
    public Church church;
   
    public ChurchEncounterNode(EncounterNode parent,int depth, int childIndex, string label, string description, Sprite sprite) : base(parent, depth, childIndex, label, description, sprite)
    {
        church = new Church(GameData.Instance.GetBlessingData());
        var relics = GameData.Instance.GetRandomRelics(2);
        church.AddItem(new ShopItem(relics[0]));
        church.AddItem(new ShopItem(relics[1]));
        //church.AddItem(new ShopItem(GameData.Instance.GetRandomStaff()));
       
    }
   
    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        Debug.Log("Activate ChurchEncounterNode");
    }
}