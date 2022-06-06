using UnityEngine;

public abstract class EncounterNodeData: ScriptableObject
{

    public GameObject prefab;
    public float appearanceChance;
    public int maxAppearanceCountPerArea = 99;
    public string description;
    public Sprite sprite;

    public abstract EncounterNode CreateNode(EncounterNode parent,int depth, int childIndex);
    
}