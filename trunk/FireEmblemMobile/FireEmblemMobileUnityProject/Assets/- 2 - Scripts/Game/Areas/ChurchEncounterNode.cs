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
    public Party party;
    public ChurchEncounterNode(EncounterNode parent) : base(parent)
    {
        church = new Church();
        church.AddItem(GameData.Instance.GetRandomRelic());
        church.AddItem(GameData.Instance.GetRandomMagic());
        church.AddItem(GameData.Instance.GetRandomStaff());
        party = Player.Instance.Party;
    }

    public override void Activate()
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this);
        Debug.Log("Activate ChurchEncounterNode");
    }
}