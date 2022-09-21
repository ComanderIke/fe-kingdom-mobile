using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Smithy
{
    public List<EquipableItem> shopItems = new List<EquipableItem>();
    public void AddItem(EquipableItem item)
    {
        shopItems.Add(item);
    }
}
public class SmithyEncounterNode : EncounterNode
{
    public Smithy smithy;

    public SmithyEncounterNode(EncounterNode parent,int depth, int childIndex,string label, string description, Sprite sprite) : base(parent, depth, childIndex,label, description, sprite)
    { 
        smithy = new Smithy();
        smithy.AddItem(GameData.Instance.GetRandomBow());
        smithy.AddItem(GameData.Instance.GetRandomSpear());
        smithy.AddItem(GameData.Instance.GetRandomSword());
        smithy.AddItem(GameData.Instance.GetRandomSword());
    

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UISmithyController>().Show(this,party);
        Debug.Log("Activate SmithyEncounterNode");
    }
}