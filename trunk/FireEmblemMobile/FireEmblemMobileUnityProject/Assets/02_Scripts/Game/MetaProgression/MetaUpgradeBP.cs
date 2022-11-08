using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade", fileName = "MetaUpgrade1")]
public class MetaUpgradeBP: ScriptableObject
{
    public string label;
    public int maxLevel;
    public int[] costToLevel;
    public string Description;
    public Sprite icon;
    public int xPosInTree=0;
    public int yPosInTree=0;
    public bool availableAtStart = false;
    
}