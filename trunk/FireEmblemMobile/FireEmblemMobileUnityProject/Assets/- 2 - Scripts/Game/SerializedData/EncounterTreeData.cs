using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    [System.Serializable]
    public class EncounterTreeData
    {
        [SerializeField]
        public List<ColumnData> columns;
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
    public class ColumnData
    {
        [SerializeField]
        public List<int> childrenNodeTypeIndexes;

        public ColumnData(Column column)
        {
            childrenNodeTypeIndexes = new List<int>();
            foreach (var child in column.children)
            {
                childrenNodeTypeIndexes.Add(child.prefabIdx);
            }
        }
    }
}