using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameInput
{
    public class SelectionUI : ISelectionUI
    {
       
        [SerializeField] private GameObject itemButton;
        [SerializeField] private GameObject skill1Button;
        [SerializeField] private GameObject skill2Button;
        [SerializeField] private GameObject waitButton;
        [SerializeField] private GameObject undoButton;

        [SerializeField] private UIConvoyController convoyController;
        public static Action OnBackClicked;

        private Unit selectedCharacter;
        private void Start()
        {
            undoButton.gameObject.SetActive(false);
            waitButton.gameObject.SetActive(false);
            skill1Button.gameObject.SetActive(false);
            skill2Button.gameObject.SetActive(false);
            itemButton.gameObject.SetActive(false);
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
            // if (selectedCharacter != null && selectedCharacter.TurnStateManager.IsWaiting)
            // {
            //     CharacterSelectedState(selectedCharacter);
            // }
        }

        private void CharacterGotSelected(IGridActor actor)
        {
            Debug.Log("CharacterGotSelected"+actor);
            if (!actor.Faction.IsPlayerControlled)
                return;
            Debug.Log("no return"+actor);
            if (actor is Unit unit)
            {
                Debug.Log("Actor is unit"+actor);
                CharacterSelectedState(unit);
               
            }
        }

      
        private void CharacterSelectedState(Unit unit)
        {
          
            Debug.Log("CharacterSelectedState "+unit);
            selectedCharacter = null;
            
            if (unit.TurnStateManager.IsWaiting)
            {
                Debug.Log("Waiting "+unit);
                NoCharacterSelectedState(null);
                return;
            }
            Debug.Log("NotWaiting "+unit);
            selectedCharacter = unit;

            int skillCount = selectedCharacter.SkillManager.ActiveSkills.Count;
            skill1Button.gameObject.SetActive(skillCount>=1);
            skill2Button.gameObject.SetActive(skillCount>=2);
            if (skillCount >= 1)
            {
                skill1Button.GetComponentsInChildren<Image>()[1].sprite =
                    selectedCharacter.SkillManager.ActiveSkills[0].Icon;
            }

            if (skillCount >= 2)
            {
                skill2Button.GetComponentsInChildren<Image>()[1].sprite =
                    selectedCharacter.SkillManager.ActiveSkills[1].Icon;
            }

            if(Player.Instance.Party.Convoy.Items.Count > 0)
                itemButton.SetActive(true);
            else
            {
                itemButton.SetActive(false);
            }
            waitButton.SetActive(true);
            Debug.Log("WaitButton true ");
            //undoButton.SetActive(true);
        }

        private void NoCharacterSelectedState(IGridActor actor)
        {
            selectedCharacter = null;
            waitButton.gameObject.SetActive(false);
            skill1Button.gameObject.SetActive(false);
            skill2Button.gameObject.SetActive(false);
            itemButton.gameObject.SetActive(false);

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

       

      

    
        public void Skill1Clicked()
        {
            new GameplayCommands().SelectSkill(selectedCharacter.SkillManager.ActiveSkills[0]);
            new GameplayCommands().ExecuteInputActions(null);
        }
        public void Skill2Clicked()
        {
            new GameplayCommands().SelectSkill(selectedCharacter.SkillManager.ActiveSkills[1]);
            new GameplayCommands().ExecuteInputActions(null);
        }
        public void WaitClicked()
        {
            new GameplayCommands().Wait(selectedCharacter);
            new GameplayCommands().ExecuteInputActions(null);
            NoCharacterSelectedState(selectedCharacter);
        }
        public void ItemsClicked()
        {
            convoyController.Show();
            //this.CallWithDelay(ItemsSelectedState, 0.005f);
           
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