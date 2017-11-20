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
            Debug.Log("Same Selected");
         }
        public void DeselectActiveCharacter()
        {
            if (SelectedCharacter != null)
                SelectedCharacter.UnitTurnState.Selected = false;
            EventContainer.deselectActiveCharacter();
            SelectedCharacter = null;
            mainScript.gridManager.HideMovement();

        }
        void SelectCharacter(LivingObject c)
        {
            Debug.Log("Select Character");
            if (SelectedCharacter != null)
            {
                SelectedCharacter.UnitTurnState.Selected = false;
            }
            SelectedCharacter = c;
            c.UnitTurnState.Selected = true;
            GridManager s = mainScript.gridManager;
            s.HideMovement();
            s.ShowMovement(c);
            s.ShowAttack(c, new List<int>(c.Stats.AttackRanges), false);

        }

        void EnemySelected(LivingObject c)
        {
            GridManager gridScript = mainScript.gridManager;

            gridScript.ShowMovement(c);
            gridScript.ShowAttack(c, new List<int>(c.Stats.AttackRanges), false);
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

                if (c.Player.ID != SelectedCharacter.Player.ID && confirm)//Clicked On Enemy
                {
                    if (confirm)
                    {
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
                        mainScript.gridManager.HideMovement();
                        SelectCharacter(c);
                    }
                }
                else if (c.UnitTurnState.HasMoved && !c.UnitTurnState.HasAttacked)
                {
                    mainScript.gridManager.HideMovement();
                    SelectedCharacter = c;
                    mainScript.gridManager.ShowAttackRange(c);
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
