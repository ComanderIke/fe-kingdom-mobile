using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/MetaUpgradeManager")]
public class MetaUpgradeManager : ScriptableObject
{
    [SerializeField] private List<MetaUpgrade> metaUpgrades;
    
    public  List<MetaUpgrade> GetUpgrades()
    {
        return metaUpgrades;
    }

    public MetaUpgradeManagerSaveData Save()
    {
        return new MetaUpgradeManagerSaveData(this);
    }

    public MetaUpgrade Find(MetaUpgrade upg)
    {
        return metaUpgrades.Find(u=> u.Equals(upg));
    }

    public void Load(MetaUpgradeManagerSaveData data)
    {
        metaUpgrades.Clear();
        foreach (var upg in data.metaUpgrades)
        {
            metaUpgrades.Add(upg.Load());
        }
        
    }

    public bool Contains(MetaUpgrade upg)
    {
        return metaUpgrades.Contains(upg);
    }

    public void Add(MetaUpgrade metaUpgrade)
    {
        metaUpgrades.Add(metaUpgrade);
    }
}