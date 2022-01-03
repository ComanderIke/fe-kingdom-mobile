using UnityEngine;

public abstract class EncounterNodeData: ScriptableObject
{

    public GameObject prefab;
    public float appearanceChance;

    public abstract EncounterNode CreateNode(EncounterNode parent);
    
}