using System.Collections.Generic;
using Game.GameActors.Players;
using UnityEngine;

public abstract class EncounterNode
{
    public string UniqueId { get; set; }
    public List<EncounterNode> parents;
    public List<EncounterNode> children;

    public GameObject gameObject;

    public bool moveable;

    public int prefabIdx;
    //public Column column;

    protected EncounterNode(EncounterNode parent)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        parents.Add(parent);
    }

    public void Continue()
    {
        Player.Instance.Party.EncounterNode = this;
        Debug.Log("ContinueNode: " + Player.Instance.Party.EncounterNode);
        GameObject.FindObjectOfType<AreaGameManager>().Continue();
    }

    public abstract void Activate();
}