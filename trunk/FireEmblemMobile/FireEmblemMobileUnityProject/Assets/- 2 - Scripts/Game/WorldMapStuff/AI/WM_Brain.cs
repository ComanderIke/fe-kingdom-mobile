using System.Linq;
using Game.GameActors.Players;
using Game.Manager;
using Game.Mechanics;
using Game.Mechanics.Commands;
using GameEngine;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Game.AI
{
    public class WM_Brain:IBrain
    {

        private bool finished;
        private Faction player;
        // private GoalManager goalManager;
        // private DecisionMaker decisionMaker;
        private WM_UnitActionSystem unitActionSystem;

        public WM_Brain(Faction player)
        {
            this.player = player;
            // goalManager = new GoalManager(player);
            // decisionMaker = new DecisionMaker();
            // unitActionSystem = WorldMapGameManager.Instance.GetSystem<WM_UnitActionSystem>();
        }

        public void Think()
        {
            finished = true;
        }
        public void ExecuteAction( AIUnitAction action)
        {
   
        }
        private bool IsStartOfTurn()
        {

        }

       
        public bool IsFinished()
        {
            return finished;
        }
    }
}