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
    public Party party;
    public SmithyEncounterNode(EncounterNode parent) : base(parent)
    { 
        smithy = new Smithy();
        smithy.AddItem(GameData.Instance.GetRandomArmor());
        smithy.AddItem(GameData.Instance.GetRandomArmor());
        smithy.AddItem(GameData.Instance.GetRandomWeapon());
        smithy.AddItem(GameData.Instance.GetRandomWeapon());
        party = Player.Instance.Party;

    }

    public override void Activate()
    {
        GameObject.FindObjectOfType<UISmithyController>().Show(this);
        Debug.Log("Activate SmithyEncounterNode");
    }
}