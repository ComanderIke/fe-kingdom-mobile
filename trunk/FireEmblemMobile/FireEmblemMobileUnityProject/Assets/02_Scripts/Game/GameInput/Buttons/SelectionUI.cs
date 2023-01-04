using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class SelectionUI : ISelectionUI
    {
        [SerializeField] GameObject roundButtonPrefab;
        [SerializeField] Transform buttonContainer;
        [SerializeField] private GameObject itemButton;
        [SerializeField] private GameObject waitButton;
        [SerializeField] private GameObject undoButton;
        private List<GameObject> buttons;
        private List<GameObject> skillButtons;
        public static Action OnBackClicked;

        private Unit selectedCharacter;
        private void Start()
        {
            buttons = new List<GameObject>();
            skillButtons = new List<GameObject>();
            UnitSelectionSystem.OnSelectedCharacter += CharacterGotSelected;
            UnitSelectionSystem.OnDeselectCharacter += NoCharacterSelectedState;
            MovementState.OnMovementFinished += CharacterGotSelected;
        }
        
        private void OnDestroy()
        {
            UnitSelectionSystem.OnSelectedCharacter -= CharacterGotSelected;
            MovementState.OnMovementFinished -= CharacterGotSelected;
            UnitSelectionSystem.OnDeselectCharacter -= NoCharacterSelectedState;
        }
        private void Update()
        {
            if (selectedCharacter != null && selectedCharacter.TurnStateManager.IsWaiting)
            {
                CharacterSelectedState(selectedCharacter);
            }
        }

        private void CharacterGotSelected(IGridActor actor)
        {
            if (!actor.Faction.IsPlayerControlled)
                return;
            if (actor is Unit unit)
            {
                CharacterSelectedState(unit);
               
            }
        }

        private void CharacterSelectedState(Unit unit)
        {
            for (int i = skillButtons.Count - 1; i >= 0; i--)
            {
                Destroy(skillButtons[i]);
            }
            buttons.Clear();
            foreach (var skill in skillButtons)
            {
                var go = Instantiate(roundButtonPrefab, buttonContainer);
                buttons.Add(go);
                skillButtons.Add(go);
            }
            
            selectedCharacter = null;
            if (unit.TurnStateManager.IsWaiting)
            {
                NoCharacterSelectedState(null);
                return;
            }

            selectedCharacter = unit;
            

      
            if(Player.Instance.Party.Convoy.Items.Count > 0)
                itemButton.SetActive(true);
            else
            {
                itemButton.SetActive(false);
            }
            waitButton.SetActive(true);
            //undoButton.SetActive(true);
        }

        private void NoCharacterSelectedState(IGridActor actor)
        {
            foreach(var button in buttons)
                button.SetActive(false);
        }

        private void ItemsSelectedState()
        {
            
            // CharacterSelectedState(selectedCharacter);
            // ItemsButton.SetActive(false);
            // CloseItemButton.SetActive(true);
            // ItemParentTransform.gameObject.SetActive(true);
            //
            // GUIUtility.ClearChildren(ItemParentTransform);
            // foreach (var item in Player.Instance.Party.Convoy.Items)
            // {
            //     var go = Instantiate(ItemButtonPrefab, ItemParentTransform);
            //     go.GetComponent<ItemButtonController>().SetItem(item, this);
            // }
        }

        private void SkillSelectedState()
        {
            CharacterSelectedState(selectedCharacter);
            //
            // SkillsButton.SetActive(false);
            // CloseSkillButton.SetActive(true);
            // SkillParentTransform.gameObject.SetActive(true);
            // Unit activeUnit = (Unit)GridGameManager.Instance.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            // GUIUtility.ClearChildren(SkillParentTransform);
            // Debug.Log(activeUnit.name);
            // Debug.Log("SkillCount: "+activeUnit.SkillManager.Skills.Count);
            // foreach (var skill in activeUnit.SkillManager.Skills)
            // {
            //     var go = Instantiate(SkillButtonPrefab, SkillParentTransform);
            //     go.GetComponent<SkillButtonController>().SetSkill(skill, this);
            //
            // }
        }

      

    
        public void SkillsClicked()
        {
            this.CallWithDelay(SkillSelectedState, 0.005f);
        }
        
        public void ItemsClicked()
        {
            this.CallWithDelay(ItemsSelectedState, 0.005f);
           
        }
        
        public void BackClicked()
        {
            // Debug.Log("BACK Clicked!");
        
            OnBackClicked?.Invoke();
            this.CallWithDelay(HideUndo,0.05f);//Invoke after small time so the raycast of the button click doesnt go to the grid....
        
       
        }
        public override void ShowUndo()
        {
           // Debug.Log("ShowUndo");
            undoButton.SetActive(true);
            
        }
        public override void HideUndo()
        {
           // Debug.Log("HideUndo");
            undoButton.SetActive(false);
        }

    }
}