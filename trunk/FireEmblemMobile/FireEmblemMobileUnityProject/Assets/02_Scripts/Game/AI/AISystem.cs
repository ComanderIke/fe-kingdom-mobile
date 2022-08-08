using System.Linq;
using System.Runtime.CompilerServices;
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
    public class AISystem:IEngineSystem
    {
        private bool finished;
        private Faction player;
        private GoalManager goalManager;
        private DecisionMaker decisionMaker;
        private UnitActionSystem unitActionSystem;
        public AIRenderer AiRenderer;

        public Faction PlayerFaction
        {
            get
            {
                return player;
            }
        }

        public AISystem(Faction player, UnitActionSystem unitActionSystem,IGridInformation gridInfo,ICombatInformation combatInfo, IPathFinder pathFinder)
        {
            this.player = player;
            AiRenderer = GameObject.FindObjectOfType<AIRenderer>();
            goalManager = new GoalManager(player);
            decisionMaker = new DecisionMaker(gridInfo,combatInfo, pathFinder );
            this.unitActionSystem = unitActionSystem;
        }

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
                var action = decisionMaker.ChooseBestAction(player.GetActiveUnits());
                ExecuteAction(action);
            }

            //var units = player.GetActiveUnits();
            // if (units.Count > 0)
            //{
            //     Debug.Log("ChooseBestMove");
            //create an attacker list for each unit that has the threaten enemy flag
            //foreach attacker generate a target list for all possible targets
            //target list can be empty(blocked by previous move etc..)
            // For each target in each list, determine the optimal tile from which to attack using these tiebreaks:
            //
            // 1. Defense tile: Y > N
            // 2. Enemy threat: lowest #
            // 3. Requires teleportation: Y > N
            // 4. Special terrain: Flier Terrain > Forest > Trench >= Regular (Trench > Regular only if cavalry)
            // 5. Movement required: lowest #
            // 6. Tile priority value: highest #

            //D. Calculate combat result for each target for each attacker, based on tile selected in previous step
            // var action = decisionMaker.ChooseBestAction(units);
            //     Debug.Log("Execute Action");
            // ExecuteAction(action);

            // }
            //  else
            // {
            //Debug.Log("Finished");
            //     finished = true;
            // }
        }
        public void ExecuteAction( AIUnitAction action)
        {
            //Debug.Log("Execute ACtion: "+action.Performer+" "+action.UnitAction+ " "+action.Location);
            if (action.Performer == null)
            {
                Debug.Log("action Performer should not be null!!!");
                return;
            }
                
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

        public void Init()
        {
            
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            
        }

        public void ShowInitTurnData()
        {
            AiRenderer.ShowInitTurnData(PlayerFaction, decisionMaker.moveOrderList);
        }

        public void NewTurn()
        {
            finished = false;
        }
    }
}