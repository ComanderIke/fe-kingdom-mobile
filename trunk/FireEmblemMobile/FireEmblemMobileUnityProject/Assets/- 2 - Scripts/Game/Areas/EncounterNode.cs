using System.Collections.Generic;
using UnityEngine;

public abstract class EncounterNode
{
    public string UniqueId { get; set; }
    public List<EncounterNode> parents;
    public List<EncounterNode> children;

    public GameObject gameObject;

    public bool moveable;
    //public Column column;

    protected EncounterNode(EncounterNode parent)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        parents.Add(parent);
    }

    public abstract void Activate();
}