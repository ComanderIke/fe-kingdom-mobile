using System;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.AI
{
    public class AIRenderer : MonoBehaviour
    {
        [SerializeField] private RedlineVisualizer redlineVisualizer;
        [SerializeField] private GridTextVisualizer textVisualizer;
        public Brain brain;
        private Faction aiPlayerFaction;


        public void Init(Brain brain)
        {
            this.brain = brain;
        }
        public void ShowInitTurnData()
        {
            if (brain == null)
                return;
            aiPlayerFaction = brain.PlayerFaction;
            ShowMoveOrderList();
            ShowAllTargets();
        }

        void ShowMoveOrderList()
        {
            //decisionMaker.moveOrderList;
        }
        void ShowAllTargets()
        {
            foreach (var unit in aiPlayerFaction.Units)
            {
                ShowTargets(unit);
            }
        }
        void ShowTargets(IAIAgent u)
        {
            //redlineVisualizer.HideAll();
           
            Debug.Log("Show Targets for : "+u);

            if (u == null)
                return;
            if (u.AIComponent == null)
                return;
            if ( u.AIComponent.ClosestTarget == null)
            {
                Debug.Log("HUH No Closest Target");
                return;
            }

            var target = u.AIComponent.ClosestTarget;
                var gridPos = u.GridComponent.GridPosition;
                var targetGridPos = target.Actor.GridComponent.GridPosition;
                redlineVisualizer.ShowRed(new Vector3(gridPos.X+0.5f, gridPos.Y+0.5f,0.5f),new Vector3(targetGridPos.X+0.5f, targetGridPos.Y+0.5f,0.5f));
                textVisualizer.ShowRed(new Vector3(gridPos.X+0.5f, gridPos.Y+0.5f,0.5f), target.Distance);
        }

        public void Hide()
        {
            redlineVisualizer.HideAll();
            textVisualizer.Clear();
        }

      
    }
}