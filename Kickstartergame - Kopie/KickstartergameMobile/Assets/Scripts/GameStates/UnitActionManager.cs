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
    public class UnitActionManager : EngineSystem
    {
        MainScript mainScript;
        public UnitActionManager()
        {
            mainScript = MainScript.GetInstance();
            EventContainer.endDragOverGrid += UnitMoveOnTile;
            EventContainer.endDragOverUnit += UnitMoveToOtherUnit;

        }
        public void MoveCharacter(LivingObject c, int x, int y, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState( c, x, y, drag, targetState));
        }
        public void MoveCharacter(LivingObject c, int x, int y, List<Vector2> path, bool drag, GameState targetState)
        {
            mainScript.SwitchState(new MovementState( c, x, y, drag, targetState, path));
        }

        public void ActiveCharWait()
        {
            MainScript mainScript = MainScript.GetInstance();
            UnitSelectionManager unitSelectionManager = mainScript.GetSystem<UnitSelectionManager>();
            LivingObject selectedUnit = unitSelectionManager.SelectedCharacter;
            if (selectedUnit != null && !selectedUnit.UnitTurnState.IsWaiting)
            {
                mainScript.gridManager.HideMovement();
                selectedUnit.UnitTurnState.IsWaiting = true;
                selectedUnit.UnitTurnState.Selected = false;
                selectedUnit = null;

            }
        }

        public void GoToEnemy(LivingObject character, LivingObject enemy, bool drag)
        {
            MouseManager.ResetMousePath();

            if (MouseManager.oldMousePath.Count == 0 && character.Stats.AttackRanges.Contains<int>((int)(Mathf.Abs(enemy.GridPosition.x - character.GridPosition.x) + Mathf.Abs(enemy.GridPosition.y - character.GridPosition.y))))
            {
                mainScript.gridManager.HideMovement();
                Debug.Log("Enemy is in Range:");
                mainScript.oldPosition = new Vector2(character.GridPosition.x, character.GridPosition.y);
                if (!drag)
                {
                    mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));

                }
                else
                {
                    mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));
                }
                return;
            }
            else//go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.gridManager.GridLogic.IsFieldAttackable(enemy.GridPosition.x, enemy.GridPosition.y))
                {
                    Debug.Log("Field Attackable");
                    mainScript.gridManager.HideMovement();
                    int sx = (int)character.GameTransform.GameObject.transform.position.x;
                    int sy = (int)character.GameTransform.GameObject.transform.position.y;
                    int tx = (int)enemy.GameTransform.GameObject.transform.position.x;
                    int ty = (int)enemy.GameTransform.GameObject.transform.position.y;


                    List<Vector2> movePath = new List<Vector2>();
                    for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                    {
                        movePath.Add(new Vector2(MouseManager.oldMousePath[i].x, MouseManager.oldMousePath[i].y));
                        Debug.Log(movePath[i]);
                    }
                    if (drag)
                    {
                        MoveCharacter(character, 0, 0, movePath, false, new FightState(character, enemy, new GameplayState()));
                    }
                    else
                    {
                        MoveCharacter(character, 0, 0, movePath, false, new FightState(character, enemy, new GameplayState()));

                    }
                    mainScript.AttackRangeFromPath = 0;

                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void UnitMoveOnTile(int x, int y)
        {
            MainScript mainScript = MainScript.GetInstance();
            UnitSelectionManager unitSelectionManager = mainScript.GetSystem<UnitSelectionManager>();
            LivingObject selectedUnit = unitSelectionManager.SelectedCharacter;
            if (mainScript.gridManager.Tiles[x, y].isActive && !(x == selectedUnit.GridPosition.x && y == selectedUnit.GridPosition.y))
            {
                if (!(selectedUnit is Monster) || (selectedUnit is Monster && !((BigTilePosition)selectedUnit.GridPosition).Position.Contains(new Vector2(x, y))))
                {
                    selectedUnit.GridPosition.SetPosition(selectedUnit.GridPosition.x, selectedUnit.GridPosition.y);
                    MoveCharacter(selectedUnit, x, y, MouseManager.oldMousePath, true, new GameplayState());
                }
                else
                {
                    unitSelectionManager.DeselectActiveCharacter();
                }

            }
            else if (mainScript.gridManager.Tiles[x, y].character != null && mainScript.gridManager.Tiles[x, y].character.Player.ID != selectedUnit.Player.ID)
            {
                GoToEnemy(selectedUnit, mainScript.gridManager.Tiles[x, y].character, true);
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }

        private void UnitMoveToOtherUnit(LivingObject draggedOverUnit)
        {
            MainScript mainScript = MainScript.GetInstance();
            UnitSelectionManager unitSelectionManager = mainScript.GetSystem<UnitSelectionManager>();
            if (draggedOverUnit.Player.ID != unitSelectionManager.SelectedCharacter.Player.ID)
            {
                if (mainScript.gridManager.GridLogic.IsFieldAttackable(draggedOverUnit.GridPosition.x, draggedOverUnit.GridPosition.y))
                    GoToEnemy(unitSelectionManager.SelectedCharacter, draggedOverUnit, true);
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }


    }
}
