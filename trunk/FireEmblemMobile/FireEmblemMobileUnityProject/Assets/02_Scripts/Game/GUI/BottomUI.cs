using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;

namespace LostGrace
{
    public class BottomUI : MonoBehaviour
    {
        [SerializeField] private BottomUILeft bottomUILeft;
        [SerializeField] private BottomUILeftEmpty bottomUILeftEmpty;
        [SerializeField] private BottomUIRight bottomUIRight;
        [SerializeField] private BottomUIRightEmpty bottomUIRightEmpty;
        [SerializeField] private GameObject wholeBottomUI;

        private void Start()
        {
            GameplayCommands.OnSelectUnit += UnitSelected;
            GameplayCommands.OnDeselectUnit += UnitDeselected;
            UnitSelectionSystem.OnEnemyDeselected+= UnitDeselected;

            // UnitSelectionSystem.OnEnemyDeselected+= UnitDeselected;
        }

        void UnitSelected(IGridActor unit)
        {
            if (!wholeBottomUI.activeSelf)
            {
                
                wholeBottomUI.SetActive(true);
            }

            if (unit.Faction.IsPlayerControlled)
            {
                bottomUILeft.Show((Unit)unit);
                bottomUILeftEmpty.Hide();
                bottomUIRight.Hide();
                bottomUIRightEmpty.Hide();
                Debug.Log("Hide Right");
            }
            else
            {
                bottomUILeft.Hide();
                bottomUILeftEmpty.Hide();
                bottomUIRight.Show(unit);
                bottomUIRightEmpty.Hide();
                Debug.Log("Show Right");
            }
            LeanTween.scaleY(wholeBottomUI, 1, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }
        void UnitDeselected(IGridActor unit)
        {
            
            if (unit.Faction.IsPlayerControlled)
            {
                bottomUILeft.Hide();
                bottomUILeftEmpty.Show();
            }
            else
            {
                bottomUIRight.Hide();
                bottomUIRightEmpty.Show();
            }

            LeanTween.scaleY(wholeBottomUI, 0, TweenUtility.fadeOutDuration).setEase(TweenUtility.easeFadeOut);
        }
        
        private void OnDisable()
        {
            GameplayCommands.OnSelectUnit -= UnitSelected;
            GameplayCommands.OnDeselectUnit -= UnitDeselected;
            UnitSelectionSystem.OnEnemyDeselected-= UnitDeselected;
        }
    }
}
