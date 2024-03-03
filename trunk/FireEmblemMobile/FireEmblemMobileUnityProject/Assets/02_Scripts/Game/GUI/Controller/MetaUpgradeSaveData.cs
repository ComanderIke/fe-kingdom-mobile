using System;
using Game.DataAndReferences.Data;
using Game.MetaProgression;
using UnityEngine;

namespace Game.GUI.Controller
{
    [Serializable]
    public class MetaUpgradeSaveData
    {
        [SerializeField] private string blueprintName;
        [SerializeField] private int level;

        public MetaUpgradeSaveData(MetaUpgrade upg)
        {
            SaveData(upg);
        }

        void SaveData(MetaUpgrade upg)
        {
            level = upg.level;
            blueprintName = upg.blueprint.name;
        }

        public MetaUpgrade Load()
        {
            MetaUpgrade ret = new MetaUpgrade(GameBPData.Instance.GetMetaUpgradeBlueprints(blueprintName));
            ret.level = level;
            return ret;
        }
    }
}