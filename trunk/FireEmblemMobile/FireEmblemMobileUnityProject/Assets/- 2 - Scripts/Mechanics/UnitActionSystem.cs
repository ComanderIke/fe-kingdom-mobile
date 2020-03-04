using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Monsters;
using Assets.GameInput;
using Assets.Grid;
using Assets.GUI;
using Assets.Mechanics.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics
{
    public class UnitActionSystem : MonoBehaviour, IEngineSystem
    {
        #region Events

        public delegate void OnCommandFinishedEvent();

        public static OnCommandFinishedEvent OnCommandFinished;

        public delegate void OnReactionFinishedEvent();

        public static OnReactionFinishedEvent OnReactionFinished;

        public delegate void OnAllCommandsFinishedEvent();

        public static OnAllCommandsFinishedEvent OnAllCommandsFinished;

        public delegate void OnUndoEvent();

        public static OnUndoEvent OnUndo;

        #region UnitActions

        public delegate void OnUnitMoveToEnemyEvent();

        public static OnUnitMoveToEnemyEvent OnUnitMoveToEnemy;

        public delegate void OnStartMovingUnitEvent();

        public static OnStartMovingUnitEvent OnStartMovingUnit;

        public delegate void OnStopMovingUnitEvent();

        public static OnStopMovingUnitEvent OnStopMovingUnit;

        public delegate void OnDeselectCharacterEvent();

        public static OnDeselectCharacterEvent OnDeselectCharacter;

        public delegate void OnSelectCharacterEvent();

        public static OnSelectCharacterEvent OnSelectedCharacter;

        #endregion

        #endregion

        private MainScript mainScript;
        public PreferredMovementPath PreferredPath;
        private Stack<Command> previewsActions;
        private Queue<Command> currentActions;

        private void Start()
        {
            mainScript = MainScript.Instance;
            InputSystem.OnEndDragOverGrid += UnitMoveOnTile;
            InputSystem.OnEndDragOverUnit += UnitMoveToOtherUnit;
            previewsActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            OnUndo += Undo;
            OnCommandFinished += ExecuteActions;
        }

        private void Undo()
        {
            Debug.Log(previewsActions.Count);
            previewsActions.Pop().Undo();
        }

        public void AddCommand(Command c)
        {
            currentActions.Enqueue(c);
        }

        public void MoveCharacter(Unit c, int x, int y, List<Vector2> path = null)
        {
            var mCc = new MoveCharacterCommand(c, x, y, path);
            Unit.UnitShowActiveEffect(c, false, false);
            UiSystem.OnShowCursor(x, y);
            currentActions.Enqueue(mCc);
        }

        public void PushUnit(Unit character, Vector2 direction)
        {
            var pCc = new PushCharacterCommand(character, direction);
            currentActions.Enqueue(pCc);
        }

        public void Fight(Unit attacker, Unit target)
        {
            var mCc = new AttackCommand(attacker, target);
            currentActions.Enqueue(mCc);
            // mCC.Execute();
        }

        public void ExecuteActions()
        {
            //do this for each finished Command
            if (currentActions.Count != 0)
            {
                var current = currentActions.Dequeue();
                if (currentActions.Count == 0)
                {
                    OnCommandFinished = null;
                    OnCommandFinished += ExecuteActions;
                    OnCommandFinished += AllCommandFinished;
                }

                current.Execute();

                previewsActions.Push(current);
            }
        }

        private void AllCommandFinished()
        {
            OnCommandFinished -= AllCommandFinished;
            OnAllCommandsFinished();
        }

        public void ActiveCharWait()
        {
            var unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            var selectedUnit = unitSelectionManager.SelectedCharacter;
            if (selectedUnit != null && !selectedUnit.UnitTurnState.IsWaiting)
            {
                mainScript.GetSystem<Map.MapSystem>().HideMovement();
                selectedUnit.UnitTurnState.IsWaiting = true;
                selectedUnit.UnitTurnState.Selected = false;
                selectedUnit.UnitTurnState.HasMoved = true;
            }

            unitSelectionManager.DeselectActiveCharacter();
        }

        public void GoToEnemy(Unit character, Unit enemy, bool drag)
        {
            OnUnitMoveToEnemy();
            if (PreferredPath.Path.Count == 0 &&
                character.GridPosition.CanAttack(character.Stats.AttackRanges, enemy.GridPosition))
            {
                mainScript.GetSystem<Map.MapSystem>().HideMovement();
                Debug.Log("Enemy is in Range:");

                if (!drag)
                {
                    Fight(character, enemy);
                    // mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));
                }
                else
                {
                    Fight(character, enemy);
                    //mainScript.SwitchState(new FightState(character, enemy, new GameplayState()));
                }

                Debug.Log("All Commands Setup");
                OnAllCommandsFinished += SwitchToGamePlayState;
                ExecuteActions();
                return;
            }
            else //go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.GetSystem<Map.MapSystem>().GridLogic
                    .IsFieldAttackable(enemy.GridPosition.X, enemy.GridPosition.Y))
                {
                    mainScript.GetSystem<Map.MapSystem>().HideMovement();

                    var movePath = new List<Vector2>();
                    for (int i = 0; i < PreferredPath.Path.Count; i++)
                    {
                        movePath.Add(new Vector2(PreferredPath.Path[i].x, PreferredPath.Path[i].y));
                    }

                    int xMov = 0;
                    int yMov = 0;
                    if (movePath.Count >= 1)
                    {
                        xMov = (int) movePath[movePath.Count - 1].x;
                        yMov = (int) movePath[movePath.Count - 1].y;
                    }

                    if (drag)
                    {
                        MoveCharacter(character, xMov, yMov, movePath);
                        Fight(character, enemy); // false, new FightState(character, enemy, new GameplayState()));
                        Debug.Log("All Commands Setup");
                        OnAllCommandsFinished += SwitchToGamePlayState;
                        ExecuteActions();
                    }
                    else
                    {
                        MoveCharacter(character, xMov, yMov,
                            movePath); //, false, new FightState(character, enemy, new GameplayState()));
                        Fight(character, enemy);
                        Debug.Log("All Commands Setup");
                        OnAllCommandsFinished += SwitchToGamePlayState;
                        ExecuteActions();
                    }

                    mainScript.GetSystem<InputSystem>().AttackRangeFromPath = 0;

                    return;
                }
                else
                {
                    Debug.Log("Enemy not in Range!");
                    return;
                }
            }
        }

        private void SwitchToGamePlayState()
        {
            OnAllCommandsFinished -= SwitchToGamePlayState;
            mainScript.GameStateManager.SwitchState(GameStateManager.GameplayState);
        }

        private void UnitMoveOnTile(int x, int y)
        {
            var unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            var selectedUnit = unitSelectionManager.SelectedCharacter;
            //Debug.Log("TEST: "+mainScript.GetSystem<Map.MapSystem>().Tiles[x, y]);
            if (mainScript.GetSystem<Map.MapSystem>().Tiles[x, y].IsActive &&
                !(x == selectedUnit.GridPosition.X && y == selectedUnit.GridPosition.Y))
            {
                if (!(selectedUnit is Monster) || !((BigTilePosition) selectedUnit.GridPosition).Position.Contains(
                    new Vector2(x, y)))
                {
                    selectedUnit.GridPosition.SetPosition(selectedUnit.GridPosition.X, selectedUnit.GridPosition.Y);
                    MoveCharacter(selectedUnit, x, y, PreferredPath.Path); //, true, new GameplayState());
                    OnAllCommandsFinished += SwitchToGamePlayState;
                    ExecuteActions();
                }
                else
                {
                    unitSelectionManager.DeselectActiveCharacter();
                }
            }
            else if (mainScript.GetSystem<Map.MapSystem>().Tiles[x, y].Unit != null &&
                     mainScript.GetSystem<Map.MapSystem>().Tiles[x, y].Unit.Player.Id != selectedUnit.Player.Id)
            {
                GoToEnemy(selectedUnit, mainScript.GetSystem<Map.MapSystem>().Tiles[x, y].Unit, true);
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }

        private void UnitMoveToOtherUnit(Unit draggedOverUnit)
        {
            Debug.Log("MoveToOtherUnit");
            var unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            if (draggedOverUnit.Player.Id != unitSelectionManager.SelectedCharacter.Player.Id)
            {
                if (mainScript.GetSystem<Map.MapSystem>().GridLogic.IsFieldAttackable(draggedOverUnit.GridPosition.X,
                    draggedOverUnit.GridPosition.Y))
                    GoToEnemy(unitSelectionManager.SelectedCharacter, draggedOverUnit, true);
                else
                {
                    Debug.Log("enemy not in Range");
                    unitSelectionManager.DeselectActiveCharacter();
                }
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }

        private void OnDestroy()
        {
            OnReactionFinished = null;
            OnCommandFinished = null;
            OnAllCommandsFinished = null;
            OnUndo = null;
            OnUnitMoveToEnemy = null;
            OnStartMovingUnit = null;
            OnStopMovingUnit = null;
            OnDeselectCharacter = null;
            OnSelectedCharacter = null;
        }
    }
}