﻿using System;
using System.Collections.Generic;

namespace Game.GUI.Controller
{
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
            if (manager.GetUpgrades() == null)
                return;
            foreach (var upg in manager.GetUpgrades())
            {
                metaUpgrades.Add(new MetaUpgradeSaveData(upg));
            }
        }
    
    }
}