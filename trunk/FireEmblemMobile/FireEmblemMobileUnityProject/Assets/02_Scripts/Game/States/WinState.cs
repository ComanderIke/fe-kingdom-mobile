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
            successRenderer.Hide();
            battleResultRenderer.Hide();
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }

        public void Finish()
        {
            GameSceneController.Instance.LoadWorldMapAfterBattle(true);
        }

        public static WinState Create()
        {
            var battleStatsSystem = GridGameManager.Instance.GetSystem<BattleStatsSystem>();
            var turnSystem = GridGameManager.Instance.GetSystem<TurnSystem>();
            var defeatedEnemies = battleStatsSystem.GetDefeatedEnemies();
            var eliteEnemies = battleStatsSystem.GetDefeatedEliteEnemies();
            var battleResult = new BattleResult(battleStatsSystem.GetDefeatedEnemyCount(),
                battleStatsSystem.GetDefeatedEliteEnemyCount(), turnSystem.TurnCount, battleStatsSystem.GetMvp(),
                defeatedEnemies, eliteEnemies);
            var winState = new WinState(battleResult)
            {
                successRenderer = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IBattleSuccessRenderer>().First(),
                battleResultRenderer=GameObject.FindObjectsOfType<BattleResultRenderer>().First()
            };
            return winState;
        }
    }
}