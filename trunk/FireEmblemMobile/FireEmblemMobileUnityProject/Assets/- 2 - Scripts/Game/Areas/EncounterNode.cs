using System.Collections.Generic;
using __2___Scripts.Game.Areas;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEngine;

public abstract class EncounterNode
{
    public string UniqueId { get; set; }
    public List<EncounterNode> parents;
    public List<EncounterNode> children;

    public List<Road> roads;
    public GameObject gameObject;

    public bool moveable;

    public int prefabIdx;
    //public Column column;

    protected EncounterNode(EncounterNode parent)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        roads = new List<Road>();
        parents.Add(parent);
    }

    public void Continue()
    {
        Player.Instance.Party.EncounterNode = this;
        Debug.Log("ContinueNode: " + Player.Instance.Party.EncounterNode);
        GameObject.FindObjectOfType<AreaGameManager>().Continue();
    }

    public abstract void Activate(Party party);

    public Road GetRoad(EncounterNode node)
    {
        foreach (var road in roads)
        {
            if (road.end == node)
                return road;
        }

        return null;
    }
}