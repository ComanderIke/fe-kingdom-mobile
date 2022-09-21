using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class InnEncounterNode : EncounterNode
{
    public Inn inn;

    public InnEncounterNode(EncounterNode parent,int depth, int childIndex, string label, string description, Sprite sprite) : base(parent, depth, childIndex, label, description, sprite)
    {
        inn = new Inn();
        //inn = new Inn(new Quest("Demo Quest", new QuestReward(QuestRewardType.Gold,100, null)), GameData.Instance.GetHumanBlueprint("Elsa"));
        // inn.AddItem(new ShopItem("Drink",25,GameAssets.Instance.visuals.InnSprites.drink , "Heal for 10%"));
        // inn.AddItem(new ShopItem("Eat",50,GameAssets.Instance.visuals.InnSprites.meat , "Heal for 25%"));
        // inn.AddItem(new ShopItem("Rest",0,GameAssets.Instance.visuals.InnSprites.rest , "Heal for 100% but skip a day!"));

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIInnController>().Show(this, party);
        Debug.Log("Activate InnEncounterNode");
    }
}