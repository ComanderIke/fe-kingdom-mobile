using System;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameInput.Interfaces;
using Game.GUI.Convoy;
using Game.States;
using Game.Systems;
using Game.Utility;
using UnityEngine;

namespace Game.GameInput.Buttons
{
    public class SelectionUI : ISelectionUI
    {
       
     
        [SerializeField] private GameObject waitButton;
        [SerializeField] private GameObject undoButton;

        [SerializeField] private UIConvoyController convoyController;
        public static Action OnBackClicked;

        private Unit selectedCharacter;
        private void Start()
        {
            undoButton.gameObject.SetActive(false);
            waitButton.gameObject.SetActive(false);
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
           // Debug.Log("CharacterGotSelected"+actor);
            if (!actor.Faction.IsPlayerControlled)
                return;
            if (actor is Unit unit)
            {
                CharacterSelectedState(unit);
               
            }
        }

      
        private void CharacterSelectedState(Unit unit)
        {
          
          //  Debug.Log("CharacterSelectedState "+unit);
            selectedCharacter = null;
            
            if (unit.TurnStateManager.IsWaiting)
            {
               // Debug.Log("Waiting "+unit);
                NoCharacterSelectedState(null);
                return;
            }
           // Debug.Log("NotWaiting "+unit);
            selectedCharacter = unit;
            Player.Instance.Party.SetActiveUnit(selectedCharacter);

            waitButton.SetActive(true);
            //Debug.Log("WaitButton true ");
            //undoButton.SetActive(true);
        }

        private void NoCharacterSelectedState(IGridActor actor)
        {
            selectedCharacter = null;
            waitButton.gameObject.SetActive(false);

        }
        
        public void WaitClicked()
        {
            new GameplayCommands.GameplayCommands().Wait(selectedCharacter);
            new GameplayCommands.GameplayCommands().ExecuteInputActions(null);
            NoCharacterSelectedState(selectedCharacter);
        }

        
        public void BackClicked()
        {

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

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}