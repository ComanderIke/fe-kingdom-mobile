using System;
using System.Linq;
using Game.GameActors.Players;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using GameEngine;
using GameEngine.GameStates;
using LostGrace;
using UnityEngine;

namespace Game.States
{
    public class WinState : GameState<NextStateTrigger>
    {
        
        private float time = 0;
        public IBattleSuccessRenderer successRenderer;
        public BattleResultRenderer battleResultRenderer;
        private BattleResult result;
        public override void Enter()
        {
            Debug.Log("Player Won");
            time = 0;
            successRenderer.Show();
            successRenderer.OnFinished -= ShowBattleResult;
            successRenderer.OnFinished += ShowBattleResult;
            OnEnter?.Invoke();
        }

        public WinState(BattleResult result)
        {
            this.result = result;
        }
        void ShowBattleResult()
        {
            battleResultRenderer.Show(result);
            battleResultRenderer.OnFinished -= Finish;
            battleResultRenderer.OnFinished += Finish;
        }
        public override void Exit()
        {
            // successRenderer.Hide();
            // battleResultRenderer.Hide();
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }

        public void Finish()
        {
            Debug.Log("FINISH");
            Player.Instance.Party.AddGold(result.GetTotalGold());
            Player.Instance.Party.AddGrace(result.GetTotalGrace());
            foreach (var item in result.GetItemBonuses())
            {
                Player.Instance.Party.AddStockedItem(item);
            }
            Player.Instance.LastBattleOutcome = BattleOutcome.Victory;
            GameSceneController.Instance.LoadEncounterAreaAfterBattle(true);
        }

        public static WinState Create()
        {
            var battleStatsSystem = GridGameManager.Instance.GetSystem<BattleStatsSystem>();
            var turnSystem = GridGameManager.Instance.GetSystem<TurnSystem>();
            var defeatedEnemies = battleStatsSystem.GetDefeatedEnemies();
            var eliteEnemies = battleStatsSystem.GetDefeatedEliteEnemies();
            BattleMap battleMap=null;
            if (SceneTransferData.Instance != null &&SceneTransferData.Instance.BattleMap!=null )
                battleMap = SceneTransferData.Instance.BattleMap;
            else
                battleMap = GameObject.FindObjectOfType<DemoUnits>().battleMap;
            var battleResult = new BattleResult(battleStatsSystem.GetDefeatedEnemyCount(),
                battleStatsSystem.GetDefeatedEliteEnemyCount(), turnSystem.TurnCount, battleStatsSystem.GetMvp(),
                defeatedEnemies, eliteEnemies, battleMap);
            var winState = new WinState(battleResult)
            {
                successRenderer = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IBattleSuccessRenderer>().First(),
                battleResultRenderer=GameObject.FindObjectsOfType<BattleResultRenderer>().First()
            };
            return winState;
        }

        public static event Action OnEnter;
    }
}