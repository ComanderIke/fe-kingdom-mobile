using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterNode
{
    public List<EncounterNode> parents;
    public List<EncounterNode> children;
    //public Column column;

    public EncounterNode(EncounterNode parent)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        parents.Add(parent);
    }
}

public class Column
{
    public List<EncounterNode> children;
}
[ExecuteInEditMode]
public class ColumnManager : MonoBehaviour
{
    public int columnCount=10;
    public int columnWidth= 5;
    public int columnHeight= 2;
    public float encounter2ChildPercentage=0.3f;
    public float encounter3ChildPercentage=0.2f;
    public float chanceToShareChild = 0.3f;
    public int columnMaxEncounter = 5;

    private EncounterNode startNode;

    private List<Column> columns = new List<Column>();
    // Start is called before the first frame update

    void OnEnable()
    {
        Column startColumn = new Column();
        startColumn.children.Add(startNode);
        columns.Add(startColumn);
        for (int i = 0; i < columnCount; i++)
        {
            Column column = new Column();
            foreach (EncounterNode node in columns.Last().children)
            {
                SpawnEncounters(node, column);
            }
            
            columns.Add(column);
        }
    }

    void SpawnEncounters(EncounterNode parent, Column current)
    {
        float rng = Random.value;
        if (rng <= encounter3ChildPercentage)
        {
            SpawnSingleEncounter(parent, current);
            SpawnSingleEncounter(parent, current);
            SpawnSingleEncounter(parent, current);
        }
        else if (rng <= encounter2ChildPercentage)
        {
            
            
           SpawnSingleEncounter(parent, current);
           SpawnSingleEncounter(parent, current);

        }
        else if(rng <= encounter2ChildPercentage)
        {
            SpawnSingleEncounter(parent, current);
        }
    }

    

    void SpawnSingleEncounter(EncounterNode parent, Column current)
    {
        if (current.children.Count > 0)
        {
            float rng2 = Random.value;
            bool bindChild = false;
            if (rng2 <= chanceToShareChild|| current.children.Count + 1 > columnMaxEncounter)
            {
                for (int i = current.children.Count-1; i >=0; i--)
                {
                    if (!current.children[i].parents.Contains(parent))
                    {
                        current.children[i].parents.Add(parent);
                        bindChild = true;
                        break;
                    }
                }

                
            }
            if (!bindChild)
            {
                EncounterNode node = new EncounterNode(parent);
                current.children.Add((node));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
