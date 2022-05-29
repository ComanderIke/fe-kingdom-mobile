using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;
public class RandomEvent
{
    public List<TextOption> textOptions;
    public string description;

    public RandomEvent(string description, List<TextOption> textOptions)
    {
        this.textOptions = textOptions;
        this.description = description;
    }
  
}

public class TextOption
{
    public string Text;
    public int StatRequirement;
    public int StatIndex;

    public TextOption(string text,int statRequirement, int statIndex)
    {
        this.Text = text;
        this.StatIndex = statIndex;
        this.StatRequirement = statRequirement;
    }
}
public class EventEncounterNode : EncounterNode
{
    public RandomEvent randomEvent;
   
    public EventEncounterNode(EncounterNode parent,int depth, int childIndex) : base(parent, depth, childIndex)
    {
        List<TextOption> options = new List<TextOption>();
        options.Add(new TextOption("Option 1:", 10, 2));
        options.Add(new TextOption("Option 2:", 15, 0));
        options.Add(new TextOption("Option 3:", 5, 5));
        randomEvent = new RandomEvent("testDescription", options);

    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIEventController>().Show(this,party);
        Debug.Log("Activate EventEncounterNode");
        
    }
}