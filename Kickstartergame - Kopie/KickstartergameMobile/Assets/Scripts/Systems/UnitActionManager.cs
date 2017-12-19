using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class UnitActionManager :  MonoBehaviour, EngineSystem
    {
        MainScript mainScript;
        public PreferedMovementPath preferedPath;
        Stack<Command> previewsActions;
        Queue<Command> currentActions;

        void Start()
        {
            Debug.Log("InitActionManager");
            mainScript = MainScript.GetInstance();
            EventContainer.endDragOverGrid += UnitMoveOnTile;
            EventContainer.endDragOverUnit += UnitMoveToOtherUnit;
            previewsActions = new Stack<Command>();
            currentActions = new Queue<Command>();
            EventContainer.undo += Undo;
            EventContainer.commandFinished += ExecuteActions;
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
        public void MoveCharacter(LivingObject c, int x, int y)
        {
            MoveCharacterCommand mCC = new MoveCharacterCommand(c, x, y);
            currentActions.Enqueue(mCC);
           // mCC.Execute();
            //mainScript.SwitchState(new MovementState( c, x, y, drag, targetState));
        }
        public void MoveCharacter(LivingObject c, int x, int y, List<Vector2> path)
        {
            MoveCharacterCommand mCC = new MoveCharacterCommand(c, x, y,path);
            currentActions.Enqueue(mCC);
           // mCC.Execute();
           // mainScript.SwitchState(new MovementState( c, x, y, path));
        }
        public void PushUnit(LivingObject character, Vector2 direction)
        {
            PushCharacterCommand pCC = new PushCharacterCommand(character, direction);
            currentActions.Enqueue(pCC);
        }
        public void Fight(LivingObject attacker, LivingObject target)
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
                Debug.Log(current);
                if (currentActions.Count == 0)
                {
                    Debug.Log("All Commands Finished!");
                    EventContainer.commandFinished += AllCommandFinished;
                }
                current.Execute();
               
                previewsActions.Push(current);
            }
        }
        void AllCommandFinished()
        {
            EventContainer.commandFinished -= AllCommandFinished;
            EventContainer.allCommandsFinished();
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
            EventContainer.unitMoveToEnemy();
            if (preferedPath.path.Count == 0 && character.GridPosition.CanAttack(character.Stats.AttackRanges,enemy.GridPosition))
            {
                
                mainScript.gridManager.HideMovement();
                Debug.Log("Enemy is in Range:");
                mainScript.oldPosition = new Vector2(character.GridPosition.x, character.GridPosition.y);
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
                EventContainer.allCommandsFinished += SwitchToGamePlayState;
                ExecuteActions();
                return;
            }
            else//go to enemy cause not in range
            {
                Debug.Log("Got to Enemy!");
                if (mainScript.gridManager.GridLogic.IsFieldAttackable(enemy.GridPosition.x, enemy.GridPosition.y))
                {
                    mainScript.gridManager.HideMovement();
                    int sx = (int)character.GameTransform.GameObject.transform.position.x;
                    int sy = (int)character.GameTransform.GameObject.transform.position.y;
                    int tx = (int)enemy.GameTransform.GameObject.transform.position.x;
                    int ty = (int)enemy.GameTransform.GameObject.transform.position.y;


                    List<Vector2> movePath = new List<Vector2>();
                    for (int i = 0; i < preferedPath.path.Count; i++)
                    {
                        movePath.Add(new Vector2(preferedPath.path[i].x, preferedPath.path[i].y));
                    }
                    if (drag)
                    {
                        MoveCharacter(character, 0, 0, movePath);
                        Fight(character, enemy);// false, new FightState(character, enemy, new GameplayState()));
                        Debug.Log("All Commands Setup");
                        EventContainer.allCommandsFinished += SwitchToGamePlayState;
                        ExecuteActions();
                    }
                    else
                    {
                        MoveCharacter(character, 0, 0, movePath);//, false, new FightState(character, enemy, new GameplayState()));
                        Fight(character, enemy);
                        Debug.Log("All Commands Setup");
                        EventContainer.allCommandsFinished += SwitchToGamePlayState;
                        ExecuteActions();

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
        private void SwitchToGamePlayState()
        {
            Debug.Log("Switch State Delete Command");
            EventContainer.allCommandsFinished -= SwitchToGamePlayState;
            mainScript.SwitchState(new GameplayState());
        }
        private void UnitMoveOnTile(int x, int y)
        {
            MainScript mainScript = MainScript.GetInstance();
            UnitSelectionManager unitSelectionManager = mainScript.GetSystem<UnitSelectionManager>();
            LivingObject selectedUnit = unitSelectionManager.SelectedCharacter;
            Debug.Log("UnitMoveontile");
            if (mainScript.gridManager.Tiles[x, y].isActive && !(x == selectedUnit.GridPosition.x && y == selectedUnit.GridPosition.y))
            {
                if (!(selectedUnit is Monster) || (selectedUnit is Monster && !((BigTilePosition)selectedUnit.GridPosition).Position.Contains(new Vector2(x, y))))
                {
                    selectedUnit.GridPosition.SetPosition(selectedUnit.GridPosition.x, selectedUnit.GridPosition.y);
                    MoveCharacter(selectedUnit, x, y,preferedPath.path);//, true, new GameplayState());
                    EventContainer.allCommandsFinished += SwitchToGamePlayState;
                    ExecuteActions();
                    
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
                Debug.Log("Deselect");
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
