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
    public class UnitSelectionManager : EngineSystem
    {
        MainScript mainScript;
        public LivingObject SelectedCharacter { get; set; }


        public UnitSelectionManager()
        {
            mainScript = MainScript.GetInstance();
            EventContainer.unitClickedConfirmed += UnitClicked;
            EventContainer.endDragOverNothing += DeselectActiveCharacter;
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
            
            SelectedCharacter = null;
            
            mainScript.gridManager.HideMovement();
            Debug.Log("Hello");
            mainScript.GetController<UIController>().ShowAllActiveUnitEffects();

        }
        void SelectCharacter(LivingObject c)
        {
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            GridManager s = mainScript.gridManager;
            mainScript.GetController<UIController>().ShowTopUI(c);
            s.HideMovement();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
            {
                s.ShowMovement(c);
                s.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            } 
            else
            {
                if(!SelectedCharacter.UnitTurnState.HasAttacked)
                    mainScript.gridManager.ShowAttackRange(c);
            }
            mainScript.GetController<UIController>().HideAllActiveUnitEffects();
            if (!SelectedCharacter.UnitTurnState.HasMoved)
                EventContainer.unitShowActiveEffect(SelectedCharacter, true, false);
        }

        void EnemySelected(LivingObject c)
        {
            Debug.Log("enemy selected " + c.Name);
            GridManager gridScript = mainScript.gridManager;
            Debug.Log(mainScript.name);
            Debug.Log(gridScript.name);
            gridScript.HideMovement();
            gridScript.ShowMovement(c);

            gridScript.ShowAttack(c, new List<int>(c.Stats.AttackRanges));
            mainScript.GetController<UIController>().ShowTopUI(c);
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
                    Debug.Log("AttackTargetsCount: "+mainScript.gridManager.GridLogic.GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Count);
                    if (confirm|| mainScript.gridManager.GridLogic.GetAttackTargetsAtGameObjectPosition(SelectedCharacter).Contains(c))
                    {
                        SelectedCharacter.ResetPosition();
                        mainScript.GetSystem<UnitActionManager>().GoToEnemy(SelectedCharacter, c, false);
                    }
                    else
                    {
                        EventContainer.enemyClicked(c);
                        
                    }
                    return;
                 }

            }
            if (mainScript.GetSystem<TurnManager>().ActivePlayer.Units.Contains(c))
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
                        mainScript.GetSystem<MouseManager>().ResetAll();
                        mainScript.gridManager.HideMovement();
                        SelectCharacter(c);
                    }
                }
                else //if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked && c.)
                {

                    mainScript.GetController<UIController>().ShowTopUI(c);
                    if (SelectedCharacter == null)
                    {
                        mainScript.GetSystem<MouseManager>().ResetAll();
                        mainScript.gridManager.HideMovement();
                        GridManager s = mainScript.gridManager;
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
