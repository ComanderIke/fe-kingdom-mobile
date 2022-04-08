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
        public GameObject SkillButtonPrefab;
        public Transform SkillParentTransform;
        public static Action OnBackClicked;

        private void Start()
        {
            UnitSelectionSystem.OnSelectedCharacter += CharacterGotSelected;
        }

        private void CharacterGotSelected(IGridActor actor)
        {
            if (actor is Unit unit)
            {
                if(unit.SkillManager.Skills.Count > 0)
                    ShowSkills();
                else
                {
                    HideSkills();
                }
                if(Player.Instance.Party.Convoy.Count > 0)
                    ShowItems();
                else
                {
                    HideItems();
                }
            }
        }

        public void CloseSkillsClicked()
        {
            SkillParentTransform.gameObject.SetActive(false);
            GUIUtility.ClearChildren(SkillParentTransform);
            SkillsButton.gameObject.SetActive(true);
            CloseSkillButton.gameObject.SetActive(false);
        }
        public void SkillsClicked()
        {
            SkillsButton.gameObject.SetActive(false);
            CloseSkillButton.gameObject.SetActive(true);
            SkillParentTransform.gameObject.SetActive(true);
            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            Unit activeUnit = (Unit)GridGameManager.Instance.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            GUIUtility.ClearChildren(SkillParentTransform);
            Debug.Log(activeUnit.name);
            Debug.Log("SkillCount: "+activeUnit.SkillManager.Skills.Count);
            foreach (var skill in activeUnit.SkillManager.Skills)
            {
                var go = Instantiate(SkillButtonPrefab, SkillParentTransform);
                go.GetComponent<SkillButtonController>().SetSkill(skill);
            }
        }
        public void ItemsClicked()
        {
        
        }
        public void BackClicked()
        {
            // Debug.Log("BACK Clicked!");
        
            OnBackClicked?.Invoke();
            Invoke(nameof(HideUndo),0.05f);//Invoke after small time so the raycast of the button click doesnt go to the grid....
        
       
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
        public override void ShowSkills()
        {
            // Debug.Log("ShowUndo");
            SkillsButton.SetActive(true);
            CloseSkillButton.SetActive(false);
        }

        public override void HideSkills()
        {
            // Debug.Log("HideUndo");
            SkillsButton.SetActive(false);
            CloseSkillButton.SetActive(false);
        }
        public override void ShowItems()
        {
            // Debug.Log("ShowUndo");
            ItemsButton.SetActive(true);
        }

        public override void HideItems()
        {
            // Debug.Log("HideUndo");
            ItemsButton.SetActive(false);
        }
    }
}