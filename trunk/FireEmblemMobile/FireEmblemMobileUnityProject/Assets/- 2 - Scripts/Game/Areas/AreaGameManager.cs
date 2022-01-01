using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area_Player
{
    public EncounterNode EncounterNode;
    public GameObject GameObject;
}
public class AreaGameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public GameObject moveOptionPrefab;
    public Area_Player player;
    public ColumnManager ColumnManager;
    // Start is called before the first frame update
    private List<GameObject> moveOptions=new List<GameObject>();
    void Start()
    {
        player = new Area_Player();
        var go = Instantiate(playerPrefab, null, false);
        go.transform.position = ColumnManager.GetStartNode().gameObject.transform.position;
        player.GameObject = go;
        player.EncounterNode = ColumnManager.GetStartNode();
        
        ResetMoveOptions();
  
        ShowMoveOptions();
    }

    private void ResetMoveOptions()
    {
        for (int i = moveOptions.Count - 1; i >= 0; i--)
        {
            Destroy(moveOptions[i]);

            
        }
        foreach (var child in player.EncounterNode.children)
        {
            child.moveable = false;
        }
    }
    public void ShowMoveOptions()
    {
        moveOptions = new List<GameObject>();
        foreach (var child in player.EncounterNode.children)
        {
            child.moveable = true;
            var go = Instantiate(moveOptionPrefab, null, false);
            go.transform.position = child.gameObject.transform.position;
            moveOptions.Add(go);
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    public void NodeClicked(EncounterNode encounterNode)
    {
        player.GameObject.transform.position = encounterNode.gameObject.transform.position;
        player.EncounterNode = encounterNode;
        FindObjectOfType<EncounterCursorController>().SetPosition(encounterNode.gameObject.transform.position);
        ResetMoveOptions();
        ShowMoveOptions();
    }
}
