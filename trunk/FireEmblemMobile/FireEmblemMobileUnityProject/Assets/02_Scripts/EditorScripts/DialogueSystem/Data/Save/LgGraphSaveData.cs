using System.Collections.Generic;
using __2___Scripts.External.Editor.Utility;
using Game.GUI.Utility;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace __2___Scripts.External.Editor.Data.Save
{
    public class LgGraphSaveData:ScriptableObject
    {
        [field:SerializeField] public string FileName { get; set; }
        [field:SerializeField] public List<LGGroupSaveData> Groups { get; set; }
        [field:SerializeField] public List<LGNodeSaveData> Nodes { get; set; }
        [field:SerializeField] public List<LGEventNodeSaveData> EventNodes { get; set; }
        [field:SerializeField] public List<string> OldGroupNames { get; set; }
        [field:SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field:SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }
        [field:SerializeField] public List<LGFightNodeSaveData> FightNodes { get; set; }
        [field:SerializeField] public List<LGBattleNodeSaveData> BattleNodes { get; set; }
        [field:SerializeField] public List<LGEventNodeSaveData> RandomNodes { get; set; }
        public void Initialize(string fileName)
        {
            FileName = fileName;
            Groups = new List<LGGroupSaveData>();
            Nodes = new List<LGNodeSaveData>();
            EventNodes = new List<LGEventNodeSaveData>();
            FightNodes = new List<LGFightNodeSaveData>();
            BattleNodes = new List<LGBattleNodeSaveData>();
            RandomNodes = new List<LGEventNodeSaveData>();
        }
    }
}