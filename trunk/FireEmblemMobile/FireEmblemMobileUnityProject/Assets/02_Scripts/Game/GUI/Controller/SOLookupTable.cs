using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SOLookupTable")]
public class SOLookupTable : ScriptableObject
{
    private static SOLookupTable _instance;

    public static SOLookupTable Instance => _instance;

    [SerializeField]private List<MetaUpgradeDictionaryEntry> dictionaryList;
    private Dictionary<string, MetaUpgradeBP> metaUpgrades;
    private void OnEnable()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            if(_instance!=this)
                Destroy(this);
        }
        foreach (var kvp in dictionaryList) {
            metaUpgrades[kvp.key] = kvp.val;
        }
    }

    public MetaUpgradeBP GetMetaUpgrade(string identifier)
    {
        return metaUpgrades[identifier];
    }
}