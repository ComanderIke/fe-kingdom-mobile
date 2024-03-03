using System.Collections.Generic;
using Game.MetaProgression;
using UnityEngine;

namespace Game.GUI.Controller
{
    [System.Serializable]
    public class MetaUpgradeManager
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

        public MetaUpgrade GetUpgrade(MetaUpgradeBP metaUpgradeBp)
        {
            foreach (var metaUpgrade in metaUpgrades)
            {
                if (metaUpgrade.blueprint == metaUpgradeBp)
                    return metaUpgrade;
            }

            return null;
        }
    }
}