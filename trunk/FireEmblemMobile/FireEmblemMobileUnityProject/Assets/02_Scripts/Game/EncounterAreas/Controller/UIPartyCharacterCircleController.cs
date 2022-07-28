using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIPartyCharacterCircleController : MonoBehaviour, IClickedReceiver
{
    public GameObject CircleCharacterUIPrefab;

    private List<CharacterUIController>characterUIs;

    private List<GameObject> characterUIgGameObjects;
    public UICharacterViewController characterView;

    private Party party;

    public GameObject layout;
    // Start is called before the first frame update
    public void Show(Party party)
    {
        this.party = party;
        if(characterUIgGameObjects==null||characterUIgGameObjects.Count!= party.members.Count)
            SpawnGOs();
        int cnt = 0;
        foreach (var unit in party.members)
        {
            if (cnt == party.ActiveUnitIndex)
            {
                characterUIs[cnt].ShowActive(unit);
            }
            else
            {
                characterUIs[cnt].Show(unit);
            }

            cnt++;
        }
    }
  

    private void SpawnGOs()
    {
        int cnt = 0;
        if (characterUIgGameObjects == null)
        {
            characterUIgGameObjects = new List<GameObject>();
            characterUIs = new List<CharacterUIController>();
        }
        foreach (var unit in party.members)
        {
            if (cnt >= characterUIs.Count)
            {
        
                var go = Instantiate(CircleCharacterUIPrefab, transform);
                var uiController = go.GetComponent<CharacterUIController>();
                uiController.parentController = this;
                uiController.Show(unit);
                characterUIs.Add(uiController);
                characterUIgGameObjects.Add(go);
            }

            cnt++;
        }
    }

    public void Clicked(Unit unit)
    {
        Debug.Log(("Clicked!"));
        for (int i = 0; i < party.members.Count; i++)
        {
            if (party.members[i] == unit)
            {
                party.ActiveUnitIndex = i;
                Show(party);
                characterView.UpdateUnit(unit);
                FindObjectOfType<AreaGameManager>().UpdatePartyGameObjects();
                break;
            }
        }
        FindObjectOfType<EncounterUIController>().UpdateUIScreens();
        layout.SetActive(false);
        layout.SetActive(true);
    }

    public void PlusClicked(Unit unit)
    {
        Debug.Log(("PlusClicked!"));
        characterView.Show(unit);
    }
}
