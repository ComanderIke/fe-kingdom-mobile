using System;
using System.Collections.Generic;
using Game.Systems;

[Serializable]
public class MetaUpgradeManagerSaveData
{
    public List<MetaUpgradeSaveData> metaUpgrades;

    public MetaUpgradeManagerSaveData(MetaUpgradeManager manager)
    {
        metaUpgrades = new List<MetaUpgradeSaveData>();
        SaveData(manager);
    }

    void SaveData(MetaUpgradeManager manager)
    {
        if (manager == null)
            return;
        foreach (var upg in manager.GetUpgrades())
        {
            metaUpgrades.Add(new MetaUpgradeSaveData(upg));
        }
    }
    
}