using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class UnitSelectionSystem : EngineSystem
    {
        MainScript mainScript;
        public LivingObject SelectedCharacter { get; set; }


        public UnitSelectionSystem()
        {
            mainScript = MainScript.GetInstance();
            EventContainer.unitClickedConfirmed += UnitClicked;
            EventContainer.endDragOverNothing += DeselectActiveCharacter;
            mainScript.GetSystem<UISystem>().onDeselectClicked.AddListener(DeselectActiveCharacter);
            //EventContainer.deselectButtonClicked += DeselectActiveCharacter;
        }
        void SameCharacterSelected(LivingObject c)
        {
            mainScript.oldPosition = new Vector2(SelectedCharacter.GameTransform.GameObject.transform.localPosition.x, SelectedCharacter.GameTransform.GameObject.transform.localPosition.y);
            DeselectActiveCharacter();
         }
        public void DeselectActiveCharacter()
        {
            if (SelectedCharacter != null)
                SelectedCharacter.ResetPosition();
            if (SelectedCharacter != null)
                SelectedCharacter.UnitTurnState.Selected = false;
            EventContainer.deselectActiveCharacter();
            mainScript.GetSystem<UISystem>().HideAttackableField();
            SelectedCharacter = null;
            
            mainScript.GetSystem<GridSystem>().HideMovement();
            Debug.Log("Hello");
            mainScript.GetSystem<UISystem>().ShowAllActiveUnitEffects();

        }
        void SelectCharacter(LivingObject c)
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            GridSystem s = mainScript.GetSystem<GridSystem>();
            mainScript.GetSystem<UISystem>().ShowTopUI(c);
            s.HideMovement();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
            {
                s.ShowMovement(c);
                s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            } 
            else
            {
                if(!SelectedCharacter.UnitTurnState.HasAttacked)
                    mainScript.GetSystem<GridSystem>().ShowAttackRange(c);
            }
            mainScript.GetSystem<UISystem>().HideAllActiveUnitEffects();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
                EventContainer.unitShowActiveEffect(SelectedCharacter, true, false);
            EventContainer.selectedActiveCharacter();
        }

        void EnemySelected(LivingObject c)
        {
            Debug.Log("enemy selected " + c.Name);
            GridSystem gridScript = mainScript.GetSystem<GridSystem>();
            Debug.Log(mainScript.name);
            Debug.Log(gridScript.name);
            gridScript.HideMovement();
            gridScript.ShowMovement(c);

            gridScript.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            mainScript.GetSystem<UISystem>().ShowTopUI(c);
            gridScript.GridLogic.ResetActiveFields();
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
                if (SelectedCharacter.GameTransform.GameObject != null)
                    SelectedCharacter = null;
            }
        }

        public void UnitClicked(LivingObject c, bool confirm)//TODO will be called by CHaracterClicked
        {
            if (SelectedCharacter != null && SelectedCharacter.GameTransform.GameObject != null && c != SelectedCharacter)
            {

                if (c.Player.ID != SelectedCharacter.Player.ID)//Clicked On Enemy
                {
                    Debug.Log("AttackTargetsCount: "+mainScript.GetSystem<GridSystem>().GridLogic.GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Count);
                    if (confirm|| mainScript.GetSystem<GridSystem>().GridLogic.GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Contains(c))
                    {
                        SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<UnitActionSystem>().GoToEnemy(SelectedCharacter, c, false);
                    }
                    else
                    {
                        EventContainer.enemyClicked(c);
                        
                    }
                    return;
                 }

            }
            if (mainScript.GetSystem<TurnSystem>().ActivePlayer.Units.Contains(c))
            {
                if (!c.UnitTurnState.IsWaiting)
                {
                    if ( SelectedCharacter != null && SelectedCharacter == c)
                    {
                        SameCharacterSelected(c);
                    }
                    else
                    {
                        if (SelectedCharacter!=null)
                            SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<GridSystem>().HideMovement();
                        SelectCharacter(c);
                    }
                }
                else //if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked && c.)
                {

                    mainScript.GetSystem<UISystem>().ShowTopUI(c);
                    if (SelectedCharacter == null)
                    {
                        mainScript.GetSystem<InputSystem>().ResetAll();
                        mainScript.GetSystem<GridSystem>().HideMovement();
                        GridSystem s = mainScript.GetSystem<GridSystem>();
                        s.ShowMovement(c);
                        s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
                    }
                    //if (SelectedCharacter != null)
                    //    SelectedCharacter.ResetPosition();
                    //mainScript.GetSystem<MouseManager>().ResetAll();
                    //mainScript.gridManager.HideMovement();
                    //DeselectActiveCharacter();
                    ////SelectedCharacter = c;
                    //SelectCharacter(c);
                    //mainScript.gridManager.ShowAttackRange(c);
                }
            }
            else
            {
                SelectedCharacter = null;
                EnemySelected(c);
            }
        }
    }
}
