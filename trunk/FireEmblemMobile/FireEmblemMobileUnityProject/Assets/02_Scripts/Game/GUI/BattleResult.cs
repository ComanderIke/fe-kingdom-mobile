using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.WorldMapStuff.Model;
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
        private BattleMap battleMap;

        public BattleResult(int defeatedEnemyCount, int defeatedEliteEnemyCount, int turnCount, Unit mvp, List<Unit> enemy, List<Unit> eliteEnemy, BattleMap battleMap)
        {
            this.battleMap = battleMap;
            this.defeatedEnemyCount = defeatedEnemyCount;
            this.defeatedEliteEnemyCount = defeatedEliteEnemyCount;
            this.turnCount = turnCount;
            config = GameBPData.Instance.BattleRewardConfig;
            this.mvp = mvp;
            this.enemy = enemy;
            this.eliteEnemy = eliteEnemy;
            itemBonuses = null;
        }
        public int GetExpFromEnemies()
        {
            return config.GracePerEnemy * defeatedEnemyCount;
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
            return config.GracePerEnemy * defeatedEliteEnemyCount;
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

        private int GetMapTurnCount()
        {
            return battleMap.turnCount == 0 ? config.DefaultTurnCount : battleMap.turnCount;
        }
        public int GetGoldFromTurnCount()
        {
            return config.GoldPerTurnLeft * Math.Max(0,GetMapTurnCount()-turnCount);
        }

      
        public int GetTotalGrace()
        {
            return GetVictoryGrace()+ GetGraceFromTurnCount();
        }

        public int GetTotalGold()
        {
            return GetGoldFromEnemies() + GetGoldFromEliteEnemies() + GetGoldFromTurnCount()+ GetVictoryGold();
        }


        public int GetVictoryGold()
        {
            return (battleMap.victoryGold==0?config.VictoryGold:battleMap.victoryGold)+Player.Instance.Modifiers.BattleGoldReward;
        }

        public int GetVictoryGrace()
        {
            return battleMap.victoryGrace==0?config.VictoryGrace:battleMap.victoryGrace;
        }

        public int GetGraceFromTurnCount()
        {
            return config.GracePerTurnLeft * Math.Max(0,GetMapTurnCount()-turnCount);
        }

        private List<StockedItem> itemBonuses;

        public   IEnumerable<StockedItem> GetItemBonuses()
        {
            if (itemBonuses == null)
                GenerateItemBonuses();
            return itemBonuses;
        }
        public IEnumerable<StockedItem> GenerateItemBonuses()
        {
            itemBonuses = new List<StockedItem>();
            if(battleMap.victoryItems!=null)
                foreach (var rewardItem in battleMap.victoryItems)
                {
                    foreach (var item in rewardItem.items)
                    {
                        if(Random.value< rewardItem.chance)
                            itemBonuses.Add(new StockedItem(item.Create(), rewardItem.count));
                    }
                   
                }

            if (SceneTransferData.Instance.BattleType == BattleType.Elite)
            {
                foreach (var rewardItem in GameBPData.Instance.eliteBattleItemDropProfile.GetRelicDrops())
                {
                    RngItemDrops(rewardItem, (Player.Instance.Modifiers.RelicDropRate-1));
                }
                foreach (var rewardItem in GameBPData.Instance.eliteBattleItemDropProfile.GetGemDrops())
                {
                    RngItemDrops(rewardItem, (Player.Instance.Modifiers.GemstoneDropRate-1));
                }
                foreach (var rewardItem in GameBPData.Instance.eliteBattleItemDropProfile.GetSmithingMaterialDrops())
                {
                    RngItemDrops(rewardItem, 0);
                }
                foreach (var rewardItem in GameBPData.Instance.eliteBattleItemDropProfile.GetOtherDrops())
                {
                    RngItemDrops(rewardItem, 0);
                }
            }
            else if (SceneTransferData.Instance.BattleType == BattleType.Normal|| SceneTransferData.Instance.BattleType == BattleType.Final)
            {
               
                foreach (var rewardItem in GameBPData.Instance.normalBattleItemDropProfile.GetRelicDrops())
                {
                    RngItemDrops(rewardItem, (Player.Instance.Modifiers.RelicDropRate-1));
                }
                foreach (var rewardItem in GameBPData.Instance.normalBattleItemDropProfile.GetGemDrops())
                {
                    RngItemDrops(rewardItem, (Player.Instance.Modifiers.GemstoneDropRate-1));
                }
                foreach (var rewardItem in GameBPData.Instance.normalBattleItemDropProfile.GetSmithingMaterialDrops())
                {
                    RngItemDrops(rewardItem, 0);
                }
                foreach (var rewardItem in GameBPData.Instance.normalBattleItemDropProfile.GetOtherDrops())
                {
                    RngItemDrops(rewardItem, 0);
                }
            }
            
                
            return itemBonuses;
        }

        private void RngItemDrops(RewardItem rewardItem, float bonusChance=0)
        {
            if (Random.value > rewardItem.chance + bonusChance)
                return;
            itemBonuses.Add(new StockedItem(rewardItem.items[Random.Range(0, rewardItem.items.Count)].Create(), rewardItem.count));
            
        }

        public int GetMaxTurnCount()
        {
            return GetMapTurnCount();
        }
    }
}