using System;
using System.Collections.Generic;
using Game.GameResources;
using LostGrace;
using UnityEngine;

public enum MetaUpgradeCost
{
    Grace,
    CorruptedGrace,
    DeathStone
}
[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade", fileName = "MetaUpgrade1")]

public class MetaUpgradeBP: ScriptableObject
{
    public string label;
    public int maxLevel;
    public int[] costToLevel;
    public MetaUpgradeCost costType;
    
    public string Description;
    public Sprite icon;
    public int[] requiredFlameLevel;
    public bool toggle = false;
    public List<MetaUpgradeMixin> mixins;

    public void OnEnable()
    {
        //if (icon == null)
            icon=GameAssets.Instance.visuals.Icons.GetRandomMetaUpgradeIcon();
    }

    public List<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        foreach (var mixin in mixins)
        {
            list.AddRange(mixin.GetEffectDescriptions(level));
        }
        return list;
    }

    public int GetRequiredFlameLevel(int i)
    {
        if (i < 0 || i >= requiredFlameLevel.Length)
            return -1;
        return requiredFlameLevel[i];
        
    }

    public MetaUpgrade Create()
    {
        var metaUpgrade = new MetaUpgrade(this);
        return metaUpgrade;
    }

    public int GetCost(int metaUpgradeLevel)
    {
        if (metaUpgradeLevel >= costToLevel.Length)
            return 0;
        return costToLevel[metaUpgradeLevel];
        
    }
}