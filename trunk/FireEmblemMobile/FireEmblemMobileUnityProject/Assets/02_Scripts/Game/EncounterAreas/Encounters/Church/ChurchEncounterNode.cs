using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class ChurchEncounterNode : EncounterNode
{
    public Church church;
   
    public ChurchEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
    {
        church = new Church(GameBPData.Instance);
        
        church.AddItem(new ShopItem(GameBPData.Instance.GetRandomRelic(1)));
        church.AddItem(new ShopItem(GameBPData.Instance.GetRandomRelic(2)));
        church.AddItem(new ShopItem(GameBPData.Instance.GetRandomRelic(3)));
        //church.AddItem(new ShopItem(GameData.Instance.GetRandomStaff()));
       
    }
   
    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        Debug.Log("Activate ChurchEncounterNode");
    }
}