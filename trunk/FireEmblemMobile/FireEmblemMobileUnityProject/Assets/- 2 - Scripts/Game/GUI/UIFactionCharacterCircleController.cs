using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI;
using Game.WorldMapStuff.Model;
using UnityEngine;

public interface IParticleAttractorTransformProvider
{
    RectTransform GetUnitParticleAttractorTransform(Unit unit);
}
public class UIFactionCharacterCircleController : MonoBehaviour,IClickedReceiver, IParticleAttractorTransformProvider
{
    // Start is called before the first frame update
    public GameObject CircleCharacterUIPrefab;

    private List<CharacterUIController>characterUIs;

    private List<GameObject> characterUIgGameObjects;

    private List<Unit> units;

    public UICharacterViewController characterView;
    public GameObject layout;
    // Start is called before the first frame update
    public void Show(List<Unit> units)
    {
        this.units = units;
        if(characterUIgGameObjects==null||characterUIgGameObjects.Count!= units.Count)
            SpawnGOs();
        int cnt = 0;
        foreach (var unit in units)
        {
            unit.visuals.UnitCharacterCircleUI = characterUIs[cnt];
            characterUIs[cnt].Show(unit);
            cnt++;
        }
    }

    public void SelectUnit(Unit u)
    {
        int cnt = 0;
        foreach (var unit in units)
        {
            if (unit == u)
            {
                characterUIs[cnt].ShowActive(unit);
            }
            else
                characterUIs[cnt].Show(unit);
            cnt++;
        }
        layout.SetActive(false);
        layout.SetActive(true);
    }

    private void SpawnGOs()
    {
        if (characterUIgGameObjects != null)
        {
            for (int i = characterUIgGameObjects.Count - 1; i >= 0; i--)
            {
                Destroy(characterUIgGameObjects[i]);
            }
        }
        int cnt = 0;

        characterUIgGameObjects = new List<GameObject>();
        characterUIs = new List<CharacterUIController>();
        foreach (var unit in units)
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
        // for (int i = 0; i < party.members.Count; i++)
        // {
        //     if (party.members[i] == unit)
        //     {
        //         party.ActiveUnitIndex = i;
        //         Show(party);
        //         FindObjectOfType<UICharacterViewController>().Show(party);
        //         FindObjectOfType<AreaGameManager>().UpdatePartyGameObjects();
        //         break;
        //     }
        // }
        // layout.SetActive(false);
        // layout.SetActive(true);
    }

    public void PlusClicked(Unit unit)
    {
        //
        characterView.Show(unit);
    }

    public RectTransform GetUnitParticleAttractorTransform(Unit unit)
    {
        int cnt = 0;
        foreach (var u in units)
        {
            if (unit == u)
            {
                return characterUIs[cnt].GetUnitParticleAttractorTransform();
            }
            cnt++;
        }

        return null;
    }

   

  
    
}
