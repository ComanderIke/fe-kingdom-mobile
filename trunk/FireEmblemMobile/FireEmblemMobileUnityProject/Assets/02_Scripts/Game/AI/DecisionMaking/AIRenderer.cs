using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        [SerializeField] private TileVisualizer tileVisualizer;
        private Faction aiPlayerFaction;


     
        public void ShowInitTurnData(Faction aiPlayerFaction, List<IAIAgent> moveOrderList)
        {
           
            this.aiPlayerFaction = aiPlayerFaction;
            ShowMoveOrder(moveOrderList);
            ShowAllTargets();
          
        }

        void ShowMoveOrder(List<IAIAgent> moveOrderList)
        {
            if (moveOrderList == null)
                return;
            int cnt = 1;
            foreach (var u in moveOrderList)
            {
                var gridPos = u.GridComponent.GridPosition;
                textVisualizer.ShowWhite(new Vector3(gridPos.X+0.1f, gridPos.Y+0.1f,0.5f), ""+cnt);
                cnt++;
            }
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
                redlineVisualizer.ShowRedPath(u.AIComponent.ClosestTarget.Path);
                //redlineVisualizer.ShowRed(new Vector3(gridPos.X+0.5f, gridPos.Y+0.5f,0.5f),new Vector3(targetGridPos.X+0.5f, targetGridPos.Y+0.5f,0.5f));
                textVisualizer.ShowRed(new Vector3(gridPos.X+0.5f, gridPos.Y+0.5f,0.5f), ""+target.Distance);
        }

        public void Hide()
        {
            redlineVisualizer.HideAll();
            textVisualizer.Clear();
        }


        public void ShowAgentData(IAIAgent selectedAgent)
        {
            if (tileVisualizer == null)
                return;
            
            tileVisualizer.Hide();
            if (selectedAgent == null || selectedAgent.AIComponent == null||selectedAgent.AIComponent.MovementOptions==null)
                return;
            foreach (var moveOption in selectedAgent.AIComponent.MovementOptions)
            {
                tileVisualizer.ShowBlue(moveOption);
            }
            Debug.Log("AttackableTargetsCount: "+selectedAgent.AIComponent.AttackableTargets.Count());
            int a = 1;
            foreach (var attackOption in selectedAgent.AIComponent.AttackableTargets)
            {
                var gridObject = (IGridObject)attackOption.Target;
                textVisualizer.ShowText(new Vector3(gridObject.GridComponent.GridPosition.X+0.1f, gridObject.GridComponent.GridPosition.Y+0.1f,0), "Prio: "+a, Color.red);
                a++;
                int i = 1;
                Debug.Log("AttackAbleTileCount: "+attackOption.AttackableTiles.Count());
                foreach (var attackTile in attackOption.AttackableTiles)
                {
                    Debug.Log("attacktile: "+attackTile);
                    textVisualizer.ShowText(new Vector3(attackTile.x+0.1f, attackTile.y+0.1f,0), "Prio: "+i, Color.white);
                    i++;
                }
                
                tileVisualizer.ShowRed(new Vector2Int(gridObject.GridComponent.GridPosition.X,gridObject.GridComponent.GridPosition.Y));
            }
        }
    }
}