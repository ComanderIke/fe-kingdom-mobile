using System.Collections.Generic;
using __2___Scripts.Game.Areas;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public abstract class EncounterNode
{
    public string UniqueId { get; set; }
    public List<EncounterNode> parents;
    public List<EncounterNode> children;

    public List<Road> roads;
    public GameObject gameObject
    {
        get;
        private set;
    }

    public void SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        renderer = gameObject.GetComponentInChildren<NodeRenderer>();
    }
    public NodeRenderer renderer;

    public bool moveable
    {
        get;
        private set;
    }

    public int prefabIdx=-1;

    public int depth = 0;

    public int childIndex = 0;

    public Sprite sprite;

    public string description;
    //public Column column;
 

    protected EncounterNode(EncounterNode parent,int depth, int childIndex, string description, Sprite icon)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        roads = new List<Road>();
        parents.Add(parent);
        this.depth = depth;
        this.childIndex = childIndex;
        this.sprite = icon;
        this.description = description;
    }

    public override string ToString()
    {
        return ""+this.GetType();
    }
    public void Continue()
    {
        Player.Instance.Party.EncounterNode = this;
     
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

    public void SetMoveable(bool b)
    {
        moveable = b;
        if (moveable)
        {
            renderer.MovableAnimation();
        }
        else
        {
            renderer.Reset();
        }
    }
}