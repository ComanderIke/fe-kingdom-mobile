using System.Linq;
using Game.GameActors.Players;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Commands;
using GameEngine;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Game.AI
{
    public class Brain
    {
        private bool finished;
        private Faction player;
        private GoalManager goalManager;
        private DecisionMaker decisionMaker;
        private UnitActionSystem unitActionSystem;

        public Brain(Faction player)
        {
            this.player = player;
            goalManager = new GoalManager(player);
            decisionMaker = new DecisionMaker(GridGameManager.Instance.GetSystem<GridSystem>().GridLogic);
            unitActionSystem = GridGameManager.Instance.GetSystem<UnitActionSystem>();
        }

        public void Think()
        {
             if (IsStartOfTurn()) goalManager.PrepareGoals();
            // Debug.Log("PrepareGoals");
             var units = player.GetActiveUnits();
            if (units.Count > 0)
            {
            //     Debug.Log("ChooseBestMove");
                 var action = decisionMaker.ChooseBestAction(units);
            //     Debug.Log("Execute Action");
                 ExecuteAction(action);

            }
            else
            {
                //Debug.Log("Finished");
                finished = true;
            }
        }
        public void ExecuteAction( AIUnitAction action)
        {
            //Debug.Log("Execute ACtion: "+action.Performer+" "+action.UnitAction+ " "+action.Location);
            Debug.Log(unitActionSystem);
            Debug.Log(action.Performer);
            Debug.Log(action.Location);
            unitActionSystem.AddCommand(new MoveCharacterCommand(action.Performer, action.Location));
            switch (action.UnitActionType)
            {
                case UnitActionType.Attack: unitActionSystem.AddCommand(new AttackCommand(action.Performer, action.Target));
                    break;
            }
            unitActionSystem.AddCommand(new WaitCommand(action.Performer));
            //will also execute all previous commands like Movement
            unitActionSystem.ExecuteActions();
        }
        private bool IsStartOfTurn()
        {
            return player.Units.All(u => !u.TurnStateManager.IsWaiting);
        }

       
        public bool IsFinished()
        {
            return finished;
        }
    }
}