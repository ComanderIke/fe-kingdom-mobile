using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using Game.Mechanics;
using Game.States;
using UnityEngine;

namespace LostGrace
{
    public class BottomUI : MonoBehaviour
    {
        [SerializeField] private BottomUICharacterSelected bottomUICharacterSelected;
        [SerializeField] private BottomUILeftEmpty bottomUILeftEmpty;
        [SerializeField] private BottomUICharacterSelected bottomUIRight;
        [SerializeField] private BottomUIRightEmpty bottomUIRightEmpty;
        [SerializeField] private GameObject wholeBottomUI;
        [SerializeField] private GameObject wholeBottomUIRed;
        private static BottomUI instance;
        private void Start()
        {
            GameplayCommands.OnSelectUnit += UnitSelected;
            GameplayCommands.OnDeselectUnit += UnitDeselected;
            UnitSelectionSystem.OnEnemyDeselected+= EnemyDeselected;
            UnitSelectionSystem.OnEnemySelected+= EnemySelected;
            GridActorComponent.AnyUnitChangedPositionAfter += AnyUnitChangedPosition;
            WinState.OnEnter += HideAll;

            instance = this;
            // UnitSelectionSystem.OnEnemyDeselected+= UnitDeselected;
        }

        void AnyUnitChangedPosition(IGridActor unit)
        {
            bottomUIRight.UpdateUI();
            bottomUICharacterSelected.UpdateUI();
        }
        public void HideAll()
        {
            bottomUICharacterSelected.Hide();
            bottomUILeftEmpty.Hide();
            bottomUIRight.Hide();
            bottomUIRightEmpty.Hide();
            wholeBottomUI.SetActive(false);
            wholeBottomUIRed.SetActive(false);
        }

        void EnemyDeselected(IGridActor unit)
        {
            Debug.Log("Enemy Deselected: "+unit);
            bottomUIRight.Hide();
            bottomUIRightEmpty.Show();
            LeanTween.cancel(wholeBottomUIRed);
            LeanTween.scaleY(wholeBottomUIRed, 0, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }

        void EnemySelected(IGridActor unit)
        {
            Debug.Log("Enemy Selected "+unit);
            if (!wholeBottomUIRed.activeSelf)
            {
                
                wholeBottomUIRed.SetActive(true);
            }

            bottomUICharacterSelected.Hide();
            bottomUILeftEmpty.Hide();
            bottomUIRight.Show((Unit)unit, false);
            bottomUIRightEmpty.Hide();
            
            if (wholeBottomUI.activeSelf)
            {
                LeanTween.cancel(wholeBottomUI);
                LeanTween.scaleY(wholeBottomUI, 0, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
            }
            LeanTween.cancel(wholeBottomUIRed);
            LeanTween.scaleY(wholeBottomUIRed, 1, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }
        void UnitSelected(IGridActor unit)
        {
            if (!wholeBottomUI.activeSelf)
            {
                
                wholeBottomUI.SetActive(true);
            }
          //  Debug.Log("Unit Selected: "+unit);
            if (unit.Faction.IsPlayerControlled)
            {
                bottomUICharacterSelected.Show((Unit)unit);
                bottomUILeftEmpty.Hide();
                bottomUIRight.Hide();
                bottomUIRightEmpty.Hide();
               
            }

            if (wholeBottomUIRed.activeSelf)
            {
                LeanTween.cancel(wholeBottomUIRed);
                LeanTween.scaleY(wholeBottomUIRed, 0, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
            }
LeanTween.cancel(wholeBottomUI);
            LeanTween.scaleY(wholeBottomUI, 1, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }
        void UnitDeselected(IGridActor unit)
        {
           // Debug.Log("Unit Deselected: "+unit);
            if (unit.Faction.IsPlayerControlled)
            {
                bottomUICharacterSelected.Hide();
                bottomUILeftEmpty.Show();
            }
         
            LeanTween.cancel(wholeBottomUI);
            LeanTween.scaleY(wholeBottomUI, 0, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }
        
        private void OnDisable()
        {
            GameplayCommands.OnSelectUnit -= UnitSelected;
            GameplayCommands.OnDeselectUnit -= UnitDeselected;
            UnitSelectionSystem.OnEnemyDeselected-= EnemyDeselected;
            UnitSelectionSystem.OnEnemySelected-= EnemySelected;
            GridActorComponent.AnyUnitChangedPositionAfter -= AnyUnitChangedPosition;
            WinState.OnEnter -= HideAll;
        }
        
        public static void Hide()
        {
            if(instance!=null&& instance.gameObject!=null)
                instance.HideAll();
        }
    }
}
