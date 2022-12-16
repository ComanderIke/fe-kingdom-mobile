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

    public SmithyEncounterNode(List<EncounterNode> parents,int depth, int childIndex,string label, string description, Sprite sprite) : base(parents, depth, childIndex,label, description, sprite)
    { 
        smithy = new Smithy();
        smithy.AddItem(GameBPData.Instance.GetRandomBow());
        smithy.AddItem(GameBPData.Instance.GetRandomSpear());
        smithy.AddItem(GameBPData.Instance.GetRandomSword());
        smithy.AddItem(GameBPData.Instance.GetRandomSword());
    

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UISmithyController>().Show(this,party);
        Debug.Log("Activate SmithyEncounterNode");
    }
}