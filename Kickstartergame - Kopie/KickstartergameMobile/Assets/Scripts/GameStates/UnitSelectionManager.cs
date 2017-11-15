using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class UnitSelectionManager
    {
        MainScript mainScript;
        public LivingObject SelectedCharacter { get; set; }


        public UnitSelectionManager()
        {
            mainScript = MainScript.GetInstance();
            EventContainer.unitClicked += CharacterClicked;
            EventContainer.endDragOverNothing += DeselectActiveCharacter;
        }
        void CharacterClicked(LivingObject c)
        {
            SetActiveCharacter(c, false);
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
            MouseManager.ResetMousePath();
            SelectedCharacter = null;
            mainScript.gridManager.HideMovement();

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


       

        public void SetActiveCharacter(LivingObject c, bool switchChar)//TODO will be called by CHaracterClicked
        {
            if (!switchChar && SelectedCharacter != null && SelectedCharacter.GameTransform.GameObject != null && c != SelectedCharacter)
            {

                if (c.Player.ID != SelectedCharacter.Player.ID)//Clicked On Enemy
                {
                    //Enemy already in Range

                    if (MouseManager.gridInput.confirmClick && MouseManager.gridInput.clickedField == new Vector2(MouseManager.currentX, MouseManager.currentY))
                        mainScript.GetSystem<UnitActionManager>().GoToEnemy(SelectedCharacter, c, false);
                    else
                    {
                        Debug.Log(c.Name + " " + MouseManager.currentX + " " + MouseManager.currentY);
                        MouseManager.gridInput.confirmClick = true;
                        MouseManager.gridInput.clickedField = new Vector2(MouseManager.currentX, MouseManager.currentY);
                        MouseManager.CalculateMousePathToEnemy(SelectedCharacter, new Vector2(MouseManager.currentX, MouseManager.currentY));
                        MouseManager.DrawMousePath();
                        if (c is Monster)
                        {
                            MouseManager.ShowAttackPreview(((BigTilePosition)c.GridPosition).Position.CenterPos());
                        }
                        else
                        {
                            MouseManager.ShowAttackPreview(new Vector2(c.GridPosition.x, c.GridPosition.y));
                        }
                    }

                    return;
                }
            }
            if (mainScript.GetSystem<TurnManager>().ActivePlayer.Units.Contains(c))
            {
                if (!c.UnitTurnState.IsWaiting)
                {
                    if (!switchChar && SelectedCharacter != null && SelectedCharacter == c)
                    {
                        SameCharacterSelected(c);
                    }
                    else
                    {
                        mainScript.gridManager.HideMovement();
                        MouseManager.gridInput.confirmClick = false;
                        MouseManager.gridInput.clickedField = new Vector2(-1, -1);
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
