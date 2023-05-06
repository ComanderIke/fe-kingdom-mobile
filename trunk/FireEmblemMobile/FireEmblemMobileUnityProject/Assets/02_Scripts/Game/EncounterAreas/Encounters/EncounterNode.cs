using System.Collections.Generic;
using __2___Scripts.Game.Areas;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEngine;


public abstract class EncounterNode
{
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

    public string label;
    //public Column column;
 

    protected EncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite icon)
    {
        children = new List<EncounterNode>();
        this.parents = new List<EncounterNode>();
        roads = new List<Road>();
        if(parents!=null)
            this.parents.AddRange(parents);
        this.depth = depth;
        this.childIndex = childIndex;
        this.sprite = icon;
        this.description = description;
        this.label = label;
    }

    public void AddParent(EncounterNode parent)
    {
        if (!parents.Contains(parent))
        {
            parents.Add(parent);
            parent.children.Add(this);
        }
    }
    public override string ToString()
    {
        return ""+this.GetType();
    }
    public void Continue()
    {
        Player.Instance.Party.EncounterComponent.activatedEncounter = true;
        Player.Instance.Party.EncounterComponent.EncounterNode = this;
     
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

    public void Grow()
    {
        renderer.GrowAnimation();
    }

    public string GetId()
    {
        return depth + "_" + childIndex;
    }
}