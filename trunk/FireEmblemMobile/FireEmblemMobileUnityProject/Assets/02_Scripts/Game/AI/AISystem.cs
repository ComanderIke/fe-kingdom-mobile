using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Commands;
using GameCamera;
using GameEngine;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Game.AI
{
    public class AISystem : IEngineSystem
    {
        private bool finished;
        private Faction player;
        private GoalManager goalManager;
        private DecisionMaker decisionMaker;
        private UnitActionSystem unitActionSystem;
        public AIRenderer AiRenderer;
     

        public Faction PlayerFaction
        {
            get { return player; }
        }

        public AISystem(Faction player, UnitActionSystem unitActionSystem, IGridInformation gridInfo,
            ICombatInformation combatInfo, IPathFinder pathFinder)
        {
            this.player = player;
            AiRenderer = GameObject.FindObjectOfType<AIRenderer>();
            goalManager = new GoalManager(player);
            decisionMaker = new DecisionMaker(gridInfo, combatInfo, pathFinder);
            this.unitActionSystem = unitActionSystem;
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
        }

        private bool ObstacleRemoved = false;

        public bool StopAIActions = false;
        private CameraSystem cameraSystem;
        public void Think()
        {
            if (IsStartOfTurn())
            {
                //Sort units based on melee=> range
                //if both same range distance to closest enemy as tie break
                //if tie smaller x value then smaller y value
                //StoreMovementDataOfAllUnit();
                // store for each unit in AIAGent if they threaten an enemy in their current range
                // optional store if they are threatend
                Debug.Log("Start of Turn");
                decisionMaker.InitTurnData(player.GetActiveUnits());
                //goalManager.PrepareGoals();
            }

            // Debug.Log("PrepareGoals");
            if (player.GetActiveUnits().Count == 0)
            {
                finished = true;
                Debug.Log("No Active Units");
            }
            else
            {
                
                if (StopAIActions)
                {
                    Debug.Log("Stop AIActions");
                    return;
                    
                }
                if (ObstacleRemoved)
                {
                    decisionMaker.InitTargets(player.GetActiveUnits());
                    decisionMaker.InitMoveOptions(player.GetActiveUnits());
                }

                ObstacleRemoved = false;
                var action = decisionMaker.ChooseBestAction(player.GetActiveUnits());
                decisionMaker.RemoveUnitFromListPool(action.Performer);
                ExecuteAction(action);
                Debug.Log("Execute Action: "+ action);
            }

        }

        public void ExecuteAction(AIUnitAction action)
        {
            //Debug.Log("Execute ACtion: "+action.Performer+" "+action.UnitAction+ " "+action.Location);
            if (action.Performer == null)
            {
                Debug.Log("action Performer should not be null!!!");
                return;
            }
            //TODO move Camera to action Performer
            //Debug.Log("Focus Camera on: "+action.Performer.GameTransformManager.GameObject.transform.position);
           // cameraSystem.GetMixin<FocusCameraMixin>().SetTargets(action.Performer.GameTransformManager.GameObject);
            //TODO wait for camera to arrive.
            unitActionSystem.AddCommand(new MoveCharacterCommand(action.Performer, action.Location));
            switch (action.UnitActionType)
            {
                case UnitActionType.Attack:
                    unitActionSystem.AddCommand(new AttackCommand((Unit)action.Performer, action.Target));
                    break;
            }

            unitActionSystem.AddCommand(new WaitCommand(action.Performer));
            //will also execute all previous commands like Movement
            UnitActionSystem.OnAllCommandsFinished += UnitActionsFinished;
            Debug.Log("Focus Camera on: "+action.Performer.GameTransformManager.GameObject.transform.position);
            cameraFocusedGO = action.Performer.GameTransformManager.GameObject;
            cameraSystem.GetMixin<FocusCameraMixin>().SetTargets(cameraFocusedGO, 0.34f);
            FocusCameraMixin.OnArrived += CameraOnUnit;
            
            
            
        }

        GameObject cameraFocusedGO;

        void CameraOnUnit()
        {
            
            FocusCameraMixin.OnArrived -= CameraOnUnit;
            cameraSystem.GetMixin<FocusCameraMixin>().SetTargets(cameraFocusedGO, 0.5f, true);
            unitActionSystem.ExecuteActions();
            
        }

        void UnitActionsFinished()
        {
            UnitActionSystem.OnAllCommandsFinished -= UnitActionsFinished;
            GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
        }
        private bool IsStartOfTurn()
        {
            return player.Units.All(u => !u.TurnStateManager.IsWaiting);
        }


        public bool IsFinished()
        {
            return finished;
        }

        public void Init()
        {
        }

        public void Deactivate()
        {
            Unit.UnitDied += UnitDiedOrMoved;
        }

        public void Activate()
        {
            Unit.UnitDied += UnitDiedOrMoved;
            Destroyable.OnDeath += UnitDiedOrMoved;
        }

        private void UnitDiedOrMoved(IGridObject unit)
        {
            ObstacleRemoved = true;
        }

        public void ShowInitTurnData()
        {
            AiRenderer.ShowInitTurnData(PlayerFaction, decisionMaker.moveOrderList);
        }

        public void NewTurn()
        {
            finished = false;
        }

        public IEnumerable<IAIAgent> GetAttackerList()
        {
            return decisionMaker.attackerList;
        }

        public IEnumerable<IAIAgent> GetMoveOrderList()
        {
            return decisionMaker.moveOrderList;
        }
    }
}