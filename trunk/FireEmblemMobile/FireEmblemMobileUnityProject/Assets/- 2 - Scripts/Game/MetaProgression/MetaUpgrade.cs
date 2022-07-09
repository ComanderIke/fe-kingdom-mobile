using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade", fileName = "MetaUpgrade1")]
public class MetaUpgrade: ScriptableObject
{
    public int level;
    public int maxLevel;
    public int[] costToLevel;
    public string Description;
    public Sprite icon;
    public UpgradeState state;
}