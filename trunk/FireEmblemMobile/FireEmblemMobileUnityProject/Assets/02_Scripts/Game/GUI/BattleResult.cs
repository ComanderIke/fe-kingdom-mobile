using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostGrace
{
    [Serializable]
    public class BattleResult
    {
        private int defeatedEnemyCount;
        private int defeatedEliteEnemyCount;
        [SerializeField]private int turnCount;
        public BattleRewardConfig config;
        private List<Unit> enemy;
        private List<Unit> eliteEnemy;
        private Unit mvp;

        public BattleResult(int defeatedEnemyCount, int defeatedEliteEnemyCount, int turnCount, Unit mvp, List<Unit> enemy, List<Unit> eliteEnemy)
        {
            this.defeatedEnemyCount = defeatedEnemyCount;
            this.defeatedEliteEnemyCount = defeatedEliteEnemyCount;
            this.turnCount = turnCount;
            config = GameBPData.Instance.BattleRewardConfig;
            this.mvp = mvp;
            this.enemy = enemy;
            this.eliteEnemy = eliteEnemy;
        }
        public int GetExpFromEnemies()
        {
            return config.BexpPerEnemy * defeatedEnemyCount;
        }

        public int GetGoldFromEnemies()
        {
            return config.GoldPerEnemy * defeatedEnemyCount;
        }

        public int GetEnemyCount()
        {
            return defeatedEnemyCount;
        } 
        public int GetEliteEnemyCount()
        {
            return defeatedEliteEnemyCount;
        } 
        public int GetTurnCount()
        {
            return turnCount;
        } 

        public Unit GetFirstEnemy()
        {
            if(enemy!=null && enemy.Count>=1)
                return enemy[0];
            return null;
        }

        public int GetGoldFromEliteEnemies()
        {
            return config.GoldPerEliteEnemy * defeatedEliteEnemyCount;
        }

        public int GetExpFromEliteEnemies()
        {
            return config.BexpPerEnemy * defeatedEliteEnemyCount;
        }

        public Unit GetFirstEliteEnemy()
        {
            if(eliteEnemy!=null && eliteEnemy.Count>=1)
                return eliteEnemy[0];
            return null;
        }
        public Unit GetMVP()
        {
            return mvp;
        }

        public int GetGoldFromTurnCount()
        {
            return config.GoldPerTurnLeft * Math.Max(0,config.DefaultTurnCount-turnCount);
        }

      
        public int GetTotalGrace()
        {
            return GetVictoryGrace()+ GetGraceFromTurnCount();
        }

        public int GetTotalGold()
        {
            return GetGoldFromEnemies() + GetGoldFromEliteEnemies() + GetGoldFromTurnCount();
        }


        public int GetVictoryGold()
        {
            return 100;
        }

        public int GetVictoryGrace()
        {
            return 50;
        }

        public int GetGraceFromTurnCount()
        {
            return config.BexpPerTurnLeft * Math.Max(0,config.DefaultTurnCount-turnCount);
        }

        public IEnumerable<StockedItem> GetItemBonuses()
        {
            var list = new List<StockedItem>();
            int rng = Random.Range(3, 5);
            for(int i=0; i <rng; i++)
                list.Add(new StockedItem(GameBPData.Instance.GetRandomItem(), 1));
            return list;
        }
    }
}