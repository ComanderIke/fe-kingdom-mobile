using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GUI.CharacterScreen;
using Game.GUI.Controller;
using Game.Systems;
using UnityEngine;

namespace Game.GUI.CharacterCircleUI
{
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
            // Debug.Log("START UI FACTRIONCIRCLECONTROLLER");
            characterUIgGameObjects = new Dictionary<Unit, GameObject>();
            characterUIs = new Dictionary<Unit, CharacterUIController>();
        }

        private void OnDisable()
        {
            Unit.UnitDied -= DeleteUI;
            UnitSelectionSystem.OnSelectedCharacter -= SelectUnit;
            UnitSelectionSystem.OnSelectedInActiveCharacter -= SelectUnit;
            UnitSelectionSystem.OnDeselectCharacter -= DeselectUnit;

        }
        private void OnEnable()
        {
            Unit.UnitDied -= DeleteUI;
            Unit.UnitDied += DeleteUI;
            UnitSelectionSystem.OnSelectedCharacter -= SelectUnit;
            UnitSelectionSystem.OnSelectedCharacter += SelectUnit;
            UnitSelectionSystem.OnSelectedInActiveCharacter -= SelectUnit;
            UnitSelectionSystem.OnSelectedInActiveCharacter += SelectUnit;
            UnitSelectionSystem.OnDeselectCharacter -= DeselectUnit;
            UnitSelectionSystem.OnDeselectCharacter += DeselectUnit;


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

        private void DeselectUnit(IGridActor u)
        {
            foreach (var unit in units)
            {
                characterUIs[unit].Show(unit);
            }
            layout.SetActive(false);
            layout.SetActive(true);
        }

        private void SelectUnit(IGridActor u)
        {

            foreach (var unit in units)
            {
                if (Equals(unit, u))
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
            // Debug.Log("SPAWN CIRCLE GOS ");
            foreach (var unit in units)
            {
                if (!unit.IsAlive())
                
                {
                    // Debug.Log("Skip Circle for: "+unit);
               
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
            characterView.Show(unit);
        }

        public void PlusClicked(Unit unit)
        {
            //
       
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
}