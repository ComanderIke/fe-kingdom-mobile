using System;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class SelectionUI : ISelectionUI
    {
        public GameObject UndoButton;
        public GameObject SkillsButton;
        public GameObject ItemsButton;
        public GameObject CloseSkillButton;
        public GameObject CloseItemButton;
        public GameObject SkillButtonPrefab;
        public GameObject ItemButtonPrefab;
        public Transform SkillParentTransform;
        public Transform ItemParentTransform;
        private GameObject favButton;
        public static Action OnBackClicked;

        private Unit selectedCharacter;
        private void Start()
        {
            UnitSelectionSystem.OnSelectedCharacter += CharacterGotSelected;
            UnitSelectionSystem.OnDeselectCharacter += NoCharacterSelectedState;
        }

        private void CharacterGotSelected(IGridActor actor)
        {
            if (actor is Unit unit)
            {
                CharacterSelectedState(unit);
            }
        }

        private void CharacterSelectedState(Unit unit)
        {
            selectedCharacter = unit;
            if (selectedCharacter.SkillManager.Favourite != null)
            {
                if(favButton!=null)
                    Destroy(favButton);
                favButton = GameObject.Instantiate(SkillButtonPrefab, this.transform);
                favButton.GetComponent<SkillButtonController>().SetSkill(selectedCharacter.SkillManager.Favourite, this);
                
            }

            if(unit.SkillManager.Skills.Count > 0)
                SkillsButton.SetActive(true);
            else
            {
                SkillsButton.SetActive(false);
               
            }
            if(Player.Instance.Party.Convoy.Items.Count > 0)
                ItemsButton.SetActive(true);
            else
            {
                ItemsButton.SetActive(false);
            }
            CloseSkillButton.SetActive(false);
            CloseItemButton.SetActive(false);
        }

        private void NoCharacterSelectedState(IGridActor actor)
        {
            ItemsButton.SetActive(false);
            CloseItemButton.SetActive(false);
            SkillsButton.SetActive(false);
            CloseSkillButton.SetActive(false);
            if(favButton!=null)
                favButton.SetActive(false);
        }

        private void ItemsSelectedState()
        {
            
            CharacterSelectedState(selectedCharacter);
            ItemsButton.SetActive(false);
            CloseItemButton.SetActive(true);
            ItemParentTransform.gameObject.SetActive(true);
          
            GUIUtility.ClearChildren(ItemParentTransform);
            foreach (var item in Player.Instance.Party.Convoy.Items)
            {
                var go = Instantiate(ItemButtonPrefab, ItemParentTransform);
                go.GetComponent<ItemButtonController>().SetItem(item, this);
            }
        }

        private void SkillSelectedState()
        {
            CharacterSelectedState(selectedCharacter);
            
            SkillsButton.SetActive(false);
            CloseSkillButton.SetActive(true);
            SkillParentTransform.gameObject.SetActive(true);
            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            Unit activeUnit = (Unit)GridGameManager.Instance.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            GUIUtility.ClearChildren(SkillParentTransform);
            Debug.Log(activeUnit.name);
            Debug.Log("SkillCount: "+activeUnit.SkillManager.Skills.Count);
            foreach (var skill in activeUnit.SkillManager.Skills)
            {
                var go = Instantiate(SkillButtonPrefab, SkillParentTransform);
                go.GetComponent<SkillButtonController>().SetSkill(skill, this);
     
            }
        }

      
        public void CloseItemsClicked()
        {
            this.CallWithDelay(CloseSubMenuClickedDelayed, 0.005f);
        }
      
        public void CloseSkillsClicked()
        {
            this.CallWithDelay(CloseSubMenuClickedDelayed, 0.005f);
        }
        private void CloseSubMenuClickedDelayed()
        {
            CharacterSelectedState(selectedCharacter);
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
            UndoButton.SetActive(true);
            
        }
        public override void HideUndo()
        {
           // Debug.Log("HideUndo");
            UndoButton.SetActive(false);
        }

    }
}