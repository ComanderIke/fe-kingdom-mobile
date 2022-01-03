using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;


public class AreaGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Area_ActionSystem actionSystem;
    public GameObject moveOptionPrefab;

    public Party playerStartParty;
    public ColumnManager ColumnManager;
    // Start is called before the first frame update
    private List<GameObject> moveOptions=new List<GameObject>();
    void Start()
    {
        actionSystem = new Area_ActionSystem();

        var go = Instantiate(playerPrefab, null, false);
        go.transform.position = ColumnManager.GetStartNode().gameObject.transform.position;
        Player.Instance.Party = Instantiate(playerStartParty);
        Player.Instance.Party.GameObject = go;
        Player.Instance.Party.EncounterNode = ColumnManager.GetStartNode();
        
        ResetMoveOptions();
  
        ShowMoveOptions();
    }

    private void ResetMoveOptions()
    {
        for (int i = moveOptions.Count - 1; i >= 0; i--)
        {
            Destroy(moveOptions[i]);

            
        }
        foreach (var child in Player.Instance.Party.EncounterNode.children)
        {
            child.moveable = false;
        }
    }
    public void ShowMoveOptions()
    {
        moveOptions = new List<GameObject>();
        foreach (var child in Player.Instance.Party.EncounterNode.children)
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
        FindObjectOfType<EncounterCursorController>().SetPosition(encounterNode.gameObject.transform.position);
        if (encounterNode.moveable)
        {
            
           
            actionSystem.Move(encounterNode);
            StartCoroutine( DelayAction(()=>encounterNode.Activate(), 1.0f));
            // ResetMoveOptions();
            // ShowMoveOptions();
        }
    }

    private IEnumerator DelayAction(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
