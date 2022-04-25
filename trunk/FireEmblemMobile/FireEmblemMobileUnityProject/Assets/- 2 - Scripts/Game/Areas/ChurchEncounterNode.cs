using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Church
{
    public List<EquipableItem> shopItems = new List<EquipableItem>();
    public void AddItem(EquipableItem item)
    {
        shopItems.Add(item);
    }
}
public class ChurchEncounterNode : EncounterNode
{
    public Church church;

    public ChurchEncounterNode(EncounterNode parent,int depth, int childIndex) : base(parent, depth, childIndex)
    {
        church = new Church();
        church.AddItem(GameData.Instance.GetRandomRelic());
        church.AddItem(GameData.Instance.GetRandomMagic());
        church.AddItem(GameData.Instance.GetRandomStaff());
       
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        Debug.Log("Activate ChurchEncounterNode");
    }
}