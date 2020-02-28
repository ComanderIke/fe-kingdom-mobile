using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.Engine;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class UnitActionSystem :  MonoBehaviour, EngineSystem
    {

        #region Events
        public delegate void OnCommandFinished();
        public static OnCommandFinished onCommandFinished;

        public delegate void OnReactionFinished();
        public static OnReactionFinished onReactionFinished;

        public delegate void OnAllCommandsFinished();
        public static OnAllCommandsFinished onAllCommandsFinished;

        public delegate void OnUndo();
        public static OnUndo onUndo;

        #region UnitActions
        public delegate void OnUnitMoveToEnemy();
        public static OnUnitMoveToEnemy onUnitMoveToEnemy;

        public delegate void OnStartMovingUnit();
        public static OnStartMovingUnit onStartMovingUnit;

        public delegate void OnStopMovingUnit();
        public static OnStopMovingUnit onStopMovingUnit;

        public delegate void OnDeselectCharacter();
        public static OnDeselectCharacter onDeselectCharacter;

        public delegate void OnSelectCharacter();
        public static OnSelectCharacter onSelectedCharacter;
        #endregion

        #endregion

        MainScript mainScript;
        public PreferedMovementPath preferedPath;
        Stack<Command> previewsActions;
        Queue<Command> currentActions;

        void Start()
        {
            mainScript = MainScript.instance;
            InputSystem.onEndDragOverGrid += UnitMoveOnTile;
            InputSystem.onEndDragOverUnit += UnitMoveToOtherUnit;
            previewsActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            UnitActionSystem.onUndo += Undo;
            UnitActionSystem.onCommandFinished += ExecuteActions;
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

        public void MoveCharacter(Unit c, int x, int y, List<Vector2> path=null)
        {
            MoveCharacterCommand mCC = new MoveCharacterCommand(c, x, y,path);
            Unit.onUnitShowActiveEffect(c, false, false);
            UISystem.onShowCursor(x, y);
            currentActions.Enqueue(mCC);
        }
        public void PushUnit(Unit character, Vector2 direction)
        {
            PushCharacterCommand pCC = new PushCharacterCommand(character, direction);
            currentActions.Enqueue(pCC);
        }
        public void Fight(Unit attacker, Unit target)
        {
            AttackCommand mCC = new AttackCommand(attacker,target);
            currentActions.Enqueue(mCC);
           // mCC.Execute();
        }
        public void ExecuteActions()
        {
            //do this for each finished Command
            if (currentActions.Count != 0)
            {
                Command current = currentActions.Dequeue();
                if (currentActions.Count == 0)
                {
                    UnitActionSystem.onCommandFinished = null;
                    UnitActionSystem.onCommandFinished += ExecuteActions;
                    UnitActionSystem.onCommandFinished += AllCommandFinished;
                }
                current.Execute();
               
                previewsActions.Push(current);
            }
        }
        void AllCommandFinished()
        {
            UnitActionSystem.onCommandFinished -= AllCommandFinished;
            UnitActionSystem.onAllCommandsFinished();
        } 

        public void ActiveCharWait()
        {
           
            UnitSelectionSystem unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            Unit selectedUnit = unitSelectionManager.SelectedCharacter;
            if (selectedUnit != null && !selectedUnit.UnitTurnState.IsWaiting)
            {
                mainScript.GetSystem<global::MapSystem>().HideMovement();
                selectedUnit.UnitTurnState.IsWaiting = true;
                selectedUnit.UnitTurnState.Selected = false;
                selectedUnit = null;

            }
            unitSelectionManager.DeselectActiveCharacter();
        }

        public void GoToEnemy(Unit character, Unit enemy, bool drag)
        {
            onUnitMoveToEnemy();
            if (preferedPath.path.Count == 0 && character.GridPosition.CanAttack(character.Stats.AttackRanges,enemy.GridPosition))
            {

                mainScript.GetSystem<global::MapSystem>().HideMovement();
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
                UnitActionSystem.onAllCommandsFinished += SwitchToGamePlayState;
                ExecuteActions();
                return;
            }
            else//go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.GetSystem<global::MapSystem>().GridLogic.IsFieldAttackable(enemy.GridPosition.x, enemy.GridPosition.y))
                {
                    mainScript.GetSystem<global::MapSystem>().HideMovement();

                    List<Vector2> movePath = new List<Vector2>();
                    for (int i = 0; i < preferedPath.path.Count; i++)
                    {
                        movePath.Add(new Vector2(preferedPath.path[i].x, preferedPath.path[i].y));
                    }
                    int xMov = 0;
                    int yMov = 0;
                    if (movePath != null && movePath.Count >= 1)
                    {
                        xMov = (int)movePath[movePath.Count - 1].x;
                        yMov = (int)movePath[movePath.Count - 1].y;
                    }
                    if (drag)
                    {
                        MoveCharacter(character, xMov, yMov, movePath);
                        Fight(character, enemy);// false, new FightState(character, enemy, new GameplayState()));
                        Debug.Log("All Commands Setup");
                        UnitActionSystem.onAllCommandsFinished += SwitchToGamePlayState;
                        ExecuteActions();
                    }
                    else
                    {
                        MoveCharacter(character, xMov, yMov, movePath);//, false, new FightState(character, enemy, new GameplayState()));
                        Fight(character, enemy);
                        Debug.Log("All Commands Setup");
                        UnitActionSystem.onAllCommandsFinished += SwitchToGamePlayState;
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
            UnitActionSystem.onAllCommandsFinished -= SwitchToGamePlayState;
            mainScript.GameStateManager.SwitchState(GameStateManager.GameplayState);
        }
        private void UnitMoveOnTile(int x, int y)
        {
  
            UnitSelectionSystem unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            Unit selectedUnit = unitSelectionManager.SelectedCharacter;
            if (mainScript.GetSystem<global::MapSystem>().Tiles[x, y].isActive && !(x == selectedUnit.GridPosition.x && y == selectedUnit.GridPosition.y))
            {
                if (!(selectedUnit is Monster) || (selectedUnit is Monster && !((BigTilePosition)selectedUnit.GridPosition).Position.Contains(new Vector2(x, y))))
                {
                    selectedUnit.GridPosition.SetPosition(selectedUnit.GridPosition.x, selectedUnit.GridPosition.y);
                    MoveCharacter(selectedUnit, x, y, preferedPath.path);//, true, new GameplayState());
                    UnitActionSystem.onAllCommandsFinished += SwitchToGamePlayState;
                    ExecuteActions();
                    
                }
                else
                {
                    unitSelectionManager.DeselectActiveCharacter();
                }

            }
            else if (mainScript.GetSystem<global::MapSystem>().Tiles[x, y].character != null && mainScript.GetSystem<global::MapSystem>().Tiles[x, y].character.Player.ID != selectedUnit.Player.ID)
            {
                GoToEnemy(selectedUnit, mainScript.GetSystem<global::MapSystem>().Tiles[x, y].character, true);
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }

        private void UnitMoveToOtherUnit(Unit draggedOverUnit)
        {
            Debug.Log("MoveToOtherUnit");
            UnitSelectionSystem unitSelectionManager = mainScript.GetSystem<UnitSelectionSystem>();
            if (draggedOverUnit.Player.ID != unitSelectionManager.SelectedCharacter.Player.ID)
            {
                if (mainScript.GetSystem<global::MapSystem>().GridLogic.IsFieldAttackable(draggedOverUnit.GridPosition.x, draggedOverUnit.GridPosition.y))
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
        void OnDestroy()
        {
            onReactionFinished = null;
            onCommandFinished = null;
            onAllCommandsFinished = null;
            onUndo = null;

            onUnitMoveToEnemy = null;
            onStartMovingUnit = null;
            onStopMovingUnit = null;
            onDeselectCharacter = null;
            onSelectedCharacter = null;
        }

    }
}
