using System;
using Game.GameActors.Units;
using Game.GameResources;

namespace LostGrace
{
    public class BattleResult
    {
        private int defeatedEnemyCount;
        private int defeatedEliteEnemyCount;
        private int turnCount;
        public BattleRewardConfig config;
        private Unit enemy;
        private Unit eliteEnemy;
        private Unit mvp;

        public BattleResult(int defeatedEnemyCount, int defeatedEliteEnemyCount, int turnCount, Unit mvp, Unit enemy, Unit eliteEnemy)
        {
            this.defeatedEnemyCount = defeatedEnemyCount;
            this.defeatedEliteEnemyCount = defeatedEliteEnemyCount;
            this.turnCount = turnCount;
            config = GameData.Instance.BattleRewardConfig;
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
            return enemy;
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
            return eliteEnemy;
        }
        public Unit GetMVP()
        {
            return mvp;
        }

        public int GetGoldFromTurnCount()
        {
            return config.GoldPerTurnLeft * Math.Max(0,config.DefaultTurnCount-turnCount);
        }

        public int GetExpFromTurnCount()
        {
            return config.BexpPerTurnLeft * Math.Max(0,config.DefaultTurnCount-turnCount);
        }

        public int GetTotalBexp()
        {
            return GetExpFromEnemies() + GetExpFromEliteEnemies() + GetExpFromTurnCount();
        }

        public int GetTotalGold()
        {
            return GetGoldFromEnemies() + GetGoldFromEliteEnemies() + GetGoldFromTurnCount();
        }

        public GachaReward GetGachaReward()
        {
            return new GachaReward();
        }
    }
}