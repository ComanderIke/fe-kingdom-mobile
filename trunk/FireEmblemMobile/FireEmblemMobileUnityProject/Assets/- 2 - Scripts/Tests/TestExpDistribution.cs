using System.Collections;
using System.Collections.Generic;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using Assets.Mechanics;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestExpDistribution
    {
        [Test]
        public void ChipExpSameLeveledBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit= ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(20, result);
        }
        [Test]
        public void KillExpSameLeveledBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(100, result);
        }
        [Test]
        public void ChipExp5LevelsHigherBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 17;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(30, result);
        }
        [Test]
        public void KillExp5LevelsHigherBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 17;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(150, result);
        }
        [Test]
        public void ChipExp5LevelsLowerBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(10, result);
        }
        [Test]
        public void KillExp5LevelsLowerBoss()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(50, result);
        }

        [Test]
        public void ChipExpSameLeveledEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(4, result);
        }
        [Test]
        public void KillExpSameLeveledEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(20, result);
        }
        [Test]
        public void ChipExp5LevelsHigherEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.Level = 17;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(6, result);
        }
        [Test]
        public void KillExp5LevelsHigherEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.Level = 17;
            playerUnit.ExperienceManager.Level = 12;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(30, result);
        }
        [Test]
        public void ChipExp5LevelsLowerEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(2, result);
        }
        [Test]
        public void KillExp5LevelsLowerEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Hp = 0;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(10, result);
        }
        [Test]
        public void ChipThenKillExp5LevelsLowerEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.EXPLeftToDrain = 20;
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            enemy.Hp = 0;
            int result2 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            Assert.AreEqual(2, result);
            Assert.AreEqual(8, result2);
        }
        [Test]
        public void Chip6TimesExp5LevelsLowerEnemy()
        {
            Unit enemy = ScriptableObject.CreateInstance<Human>();
            enemy.Initialize();
            enemy.Stats.MaxHp = 50;
            enemy.Hp = 50;
            Unit playerUnit = ScriptableObject.CreateInstance<Human>();
            playerUnit.Initialize();
            enemy.ExperienceManager.MaxEXPToDrain = 20;
            enemy.ExperienceManager.EXPLeftToDrain = 20;
            enemy.ExperienceManager.Level = 12;
            playerUnit.ExperienceManager.Level = 17;
            BattleSystem battleSystem = new BattleSystem();
            int result = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            
            int result2 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            int result3 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            int result4 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            int result5 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            int result6 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);
            enemy.Hp = 0;
            int result7 = battleSystem.CalculateExperiencePoints(playerUnit, enemy);


            Assert.AreEqual(2, result);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(2, result3);
            Assert.AreEqual(2, result4);
            Assert.AreEqual(0, result5);
            Assert.AreEqual(0, result6);
            Assert.AreEqual(2, result7);
        }

    }
}
