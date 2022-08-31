using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class ChurchEncounterNode : EncounterNode
{
    public Church church;

    public ChurchEncounterNode(EncounterNode parent,int depth, int childIndex, string description, Sprite sprite) : base(parent, depth, childIndex, description, sprite)
    {
        church = new Church(GameData.Instance.GetBlessingData());
        church.AddItem(new ShopItem(GameData.Instance.GetRandomRelic()));
        church.AddItem(new ShopItem(GameData.Instance.GetRandomMagic()));
        //church.AddItem(new ShopItem(GameData.Instance.GetRandomStaff()));
       
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        Debug.Log("Activate ChurchEncounterNode");
    }
}