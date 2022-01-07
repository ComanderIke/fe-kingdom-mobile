using Game.WorldMapStuff.Model;
using UnityEngine;
public class RandomEvent
{
   // public List<EquipableItem> shopItems = new List<EquipableItem>();
    // public void AddItem(EquipableItem item)
    // {
    //     shopItems.Add(item);
    // }
}
public class EventEncounterNode : EncounterNode
{
    public RandomEvent randomEvent;
    public Party party;
    public EventEncounterNode(EncounterNode parent) : base(parent)
    {
        randomEvent = new RandomEvent();

    }

    public override void Activate()
    {
        GameObject.FindObjectOfType<UIEventController>().Show(this);
        Debug.Log("Activate EventEncounterNode");
        
    }
}