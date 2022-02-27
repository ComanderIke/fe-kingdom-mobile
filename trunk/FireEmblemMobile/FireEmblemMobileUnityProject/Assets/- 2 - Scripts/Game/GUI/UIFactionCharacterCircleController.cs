using System;
using System.Collections;
using System.Collections.Generic;
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

    private Faction faction;

    public GameObject layout;
    // Start is called before the first frame update
    public void Show(Faction faction)
    {
        this.faction = faction;
        if(characterUIgGameObjects==null||characterUIgGameObjects.Count!= faction.Units.Count)
            SpawnGOs();
        int cnt = 0;
        foreach (var unit in faction.Units)
        {
            unit.visuals.UnitCharacterCircleUI = characterUIs[cnt];
            characterUIs[cnt].Show(unit);
            cnt++;
        }
    }

    public void SelectUnit(Unit u)
    {
        int cnt = 0;
        foreach (var unit in faction.Units)
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
        int cnt = 0;
        if (characterUIgGameObjects == null)
        {
            characterUIgGameObjects = new List<GameObject>();
            characterUIs = new List<CharacterUIController>();
        }
        foreach (var unit in faction.Units)
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

    public RectTransform GetUnitParticleAttractorTransform(Unit unit)
    {
        int cnt = 0;
        foreach (var u in faction.Units)
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
