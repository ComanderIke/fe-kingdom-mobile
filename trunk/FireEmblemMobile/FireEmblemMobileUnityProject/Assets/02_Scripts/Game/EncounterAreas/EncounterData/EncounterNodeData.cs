using System.Collections.Generic;
using UnityEngine;

public abstract class EncounterNodeData: ScriptableObject
{
    public string label;
    public GameObject prefab;
    public float appearanceChance;
    public int maxAppearanceCountPerArea = 99;
    public string description;
    public Sprite sprite;

    public abstract EncounterNode CreateNode(List<EncounterNode> parents,int depth, int childIndex);
   
    
}