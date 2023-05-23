using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    [System.Serializable]
    public class EncounterTreeData
    {
        [SerializeField] public List<ColumnData> columns;

        public EncounterTreeData(EncounterTree encounterTree)
        {
            columns = new List<ColumnData>();
            foreach (var column in encounterTree.columns)
            {
                columns.Add(new ColumnData(column));
            }
        }
    }
    [System.Serializable]
    public class NodeData
    {
        [SerializeField]
        public int nodeTypeIndex;
        [SerializeField]
        public List<int> parentIndexes;

        [SerializeField] public string userDataId;

        public NodeData(EncounterNode node)
        {
            nodeTypeIndex=node.prefabIdx;
            parentIndexes = new List<int>();
            foreach (var parent in node.parents)
            {
                if (parent != null)
                {
                    parentIndexes.Add(parent.childIndex);
                }
                else
                {
                    Debug.Log("Parent is null!?"+node);
                }
            }

            if (node is EventEncounterNode eventNode)
            {
                userDataId = eventNode.randomEvent.name;
            }
            
        }
    }

    [System.Serializable]
    public class ColumnData
    {
        [SerializeField] public List<NodeData> nodeDatas;
      
        public ColumnData(Column column)
        {
            nodeDatas = new List<NodeData>();
            foreach (var child in column.children)
            {
                nodeDatas.Add(new NodeData(child));
                
            }
        }
    }
}