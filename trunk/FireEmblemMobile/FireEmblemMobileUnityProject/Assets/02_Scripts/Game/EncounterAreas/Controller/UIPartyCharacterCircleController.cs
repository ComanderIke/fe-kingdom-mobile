using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI;
using Game.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIPartyCharacterCircleController : MonoBehaviour, IClickedReceiver, IParticleAttractorTransformProvider
{
    public GameObject CircleCharacterUIPrefab;

    private Dictionary<Unit, CharacterUIController>characterUIs;

    private List<GameObject> characterUIgGameObjects;
    public UICharacterViewController characterView;

    private Party party;

    public GameObject layout;
    // Start is called before the first frame update
    public void Show(Party party)
    {
        this.party = party;
        DeleteGOs();
        SpawnGOs();
        foreach (var unit in party.members)
        {
            if (unit == party.ActiveUnit)
            {
                characterUIs[unit].ShowActive(unit);
                unit.visuals.UnitCharacterCircleUI = characterUIs[unit];
            }
            else
            {
                unit.visuals.UnitCharacterCircleUI = characterUIs[unit];
                characterUIs[unit].Show(unit);
            }

           
        }
    }

    void DeleteGOs()
    {
        transform.DeleteAllChildren();
        characterUIgGameObjects = new List<GameObject>();
        characterUIs = new Dictionary<Unit, CharacterUIController>();
    }
    private void SpawnGOs()
    {
        
        foreach (var unit in party.members)
        {
            if (!characterUIs.ContainsKey(unit))
            {
        
                var go = Instantiate(CircleCharacterUIPrefab, transform);
                var uiController = go.GetComponent<CharacterUIController>();
                uiController.parentController = this;
                uiController.Show(unit);
                characterUIs.Add(unit,uiController);
                characterUIgGameObjects.Add(go);
            }

        }
    }

    public void Clicked(Unit unit)
    {
        characterView.Show(unit);
        Debug.Log(("Clicked!"));
        for (int i = 0; i < party.members.Count; i++)
        {
            if (party.members[i] == unit)
            {
                party.ActiveUnitIndex = i;
                // MonoUtility.InvokeNextFrame(()=>Show(party));//Otherwise mouse click will go through UI
                
                
                break;
            }
        }
        FindObjectOfType<EncounterUIController>()?.UpdateUIScreens();
        layout.SetActive(false);
        layout.SetActive(true);
    }

    // public void PlusClicked(Unit unit)
    // {
    //     characterView.Show(unit);
    // }

    public RectTransform GetUnitParticleAttractorTransform(Unit unit)
    {
        foreach (var u in party.members)
        {
            if (unit == u)
            {
                return characterUIs[u].GetUnitParticleAttractorTransform();
            }

        }

        return null;
    }
}
