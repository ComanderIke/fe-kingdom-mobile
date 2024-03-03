using System.Collections.Generic;
using Game.EncounterAreas.AreaConstruction;
using Game.EncounterAreas.Encounters.NodeVisuals;
using Game.EncounterAreas.Management;
using Game.EncounterAreas.Model;
using Game.GameActors.Player;
using UnityEngine;

namespace Game.EncounterAreas.Encounters
{
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
            Init();
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
        public void AddChild(EncounterNode min)
        {
            if (!children.Contains(min))
            {
                children.Add(min);
                min.parents.Add(this);
            }
        }
        public override string ToString()
        {
            return ""+this.GetType();
        }
        public void Continue()
        {
            Player.Instance.Party.EncounterComponent.activatedEncounter = true;
            Player.Instance.CurrentEventDialogID = "";
     
            GameObject.FindObjectOfType<AreaGameManager>().Continue();
        }

        public virtual void Activate(Party party)
        {
            Player.Instance.Party.EncounterComponent.activatedEncounter = false;
        }
    

        public Road GetRoad(EncounterNode node)
        {
            foreach (var road in roads)
            {
                if (road.end == node)
                    return road;
            }

            return null;
        }

        public void SetActive(bool b)
        {
            if(b)
                renderer.SetActive();
            else
            {
                renderer.SetInactive();
            }
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
                for (int i = 0; i < roads.Count; i++)
                    roads[i].SetMoveable(b);
                renderer.Reset();
            }
        }

        public virtual void Init()
        {
            Debug.Log("Init Node: "+label);
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
}