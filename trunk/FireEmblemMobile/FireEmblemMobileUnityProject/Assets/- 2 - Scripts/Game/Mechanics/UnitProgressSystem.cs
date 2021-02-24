﻿using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GUI;
using Game.Manager;
using GameEngine;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Mechanics
{
    public class UnitProgressSystem : IEngineSystem, IDependecyInjection
    {
        public ILevelUpRenderer levelUpRenderer;//injected
        private List<Faction> Factions;

        public void Init()
        {
            Factions = GridGameManager.Instance.FactionManager.Factions;
            foreach (var faction in Factions)
            {
                Debug.Log(faction);
                Debug.Log(faction.Units);
                faction.OnAddUnit += UpdateUnit;
                foreach (var unit in faction.Units)
                {
                    unit.OnLevelUp += LevelUp;
                }
            }
        }

        private void UpdateUnit(Unit unit)
        {
            unit.OnLevelUp += LevelUp;
        }
        private void LevelUp(Unit unit)
        {
            int[] statIncreases = CalculateStatIncreases(unit.Growths.GetGrowthsArray());
            Debug.Log(" Level Up Progress!");
            levelUpRenderer.Show(unit.name, unit.ExperienceManager.Level-1, unit.ExperienceManager.Level, unit.Stats.GetStatArray(), statIncreases);
            unit.Stats.MaxHp += statIncreases[0];
            unit.Stats.MaxSp += statIncreases[1];
            unit.Stats.Str += statIncreases[2];
            unit.Stats.Mag += statIncreases[3];
            unit.Stats.Spd += statIncreases[4];
            unit.Stats.Skl += statIncreases[5];
            unit.Stats.Def += statIncreases[6];
            unit.Stats.Res += statIncreases[7];
        }
        private int[] CalculateStatIncreases(int[] growths)
        {
            int[] increaseAmount = new int[growths.Length];
            for (int i = 0; i < growths.Length; i++)
            {
                increaseAmount[i] = Method(growths[i]);
            }

            return increaseAmount;
        }
        private int Method(int Growth)
        {
            int rngNumber = (int) (UnityEngine.Random.value * 100f);
            if (Growth > 100)
            {
                return 1 + Method(Growth - 100);
            }

            if (rngNumber <= Growth)
            {
                return 1;
            }

            return 0;
        }
    }
}