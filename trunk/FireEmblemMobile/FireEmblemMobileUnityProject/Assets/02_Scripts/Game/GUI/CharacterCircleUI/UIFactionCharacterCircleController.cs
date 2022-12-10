using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

   
    private Dictionary<Unit, GameObject>characterUIgGameObjects;
    private Dictionary<Unit, CharacterUIController>characterUIs;

    private List<Unit> units;

    public UICharacterViewController characterView;
    public GameObject layout;
    // Start is called before the first frame update
    private void Start()
    {
        characterUIgGameObjects = new Dictionary<Unit, GameObject>();
        characterUIs = new Dictionary<Unit, CharacterUIController>();
    }

    private void OnDisable()
    {
        Unit.UnitDied -= DeleteUI;
  
    }
    private void OnEnable()
    {
        Unit.UnitDied -= DeleteUI;
        Unit.UnitDied += DeleteUI;

    }

    public void Show(List<Unit> units)
    {
        this.units = units;
        Unit.UnitDied -= DeleteUI;
        Unit.UnitDied += DeleteUI;

      
        if(characterUIgGameObjects==null||characterUIgGameObjects.Count!=   units.Count(u => u.IsAlive()))
            SpawnGOs();
        int cnt = 0;
        foreach (var unit in units)
        {
            if(!unit.IsAlive())
                continue;
            unit.visuals.UnitCharacterCircleUI = characterUIs[unit];
           
            characterUIs[unit].Show(unit);
            cnt++;
        }
    }

    private void DeleteUI(Unit died)
    {
        Debug.Log("Unit Died so Delete CircleUI!");
        
        if (characterUIgGameObjects != null)
        {
            foreach (var unit in units)
            {
                if (!unit.IsAlive())
                {
                    Debug.Log("Destroy Circle");
                    Destroy(characterUIgGameObjects[unit]);
                }
            }
        }
        layout.SetActive(false);
        layout.SetActive(true);
    }
    public void SelectUnit(Unit u)
    {

        foreach (var unit in units)
        {
            if (unit == u)
            {
                characterUIs[unit].ShowActive(unit);
            }
            else
                characterUIs[unit].Show(unit);
      
        }
       
        layout.SetActive(false);
        layout.SetActive(true);
    }

    private void SpawnGOs()
    {


        
        foreach (var unit in units)
        {
            if (!unit.IsAlive())
                
            {
                Debug.Log("Skip Circle for: "+unit);
               
                continue;
            }
            var go = Instantiate(CircleCharacterUIPrefab, transform);
            var uiController = go.GetComponent<CharacterUIController>();
            uiController.parentController = this;
            uiController.Show(unit);
            characterUIs.Add(unit, uiController);
            characterUIgGameObjects.Add(unit,go);
            
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

        foreach (var u in units)
        {
            if (unit == u)
            {
                return characterUIs[unit].GetUnitParticleAttractorTransform();
            }

        }

        return null;
    }

   

  
    
}
