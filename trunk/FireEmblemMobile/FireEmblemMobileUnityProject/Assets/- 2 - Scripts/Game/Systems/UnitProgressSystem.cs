using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.States;
using GameEngine;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utility;

namespace Game.Mechanics
{
    public class UnitProgressSystem : IEngineSystem, IDependecyInjection
    {
        public ILevelUpRenderer levelUpRenderer;//injected
        public IExpRenderer ExpRenderer;
        private List<Faction> Factions;

        public void Init()
        {
            Debug.Log("TODO INIT PROGRESS SYSTEM");
            // Factions = GridGameManager.Instance.FactionManager.Factions;
            // foreach (var faction in Factions)
            // {
            //     faction.OnAddUnit += UpdateUnit;
            //     Debug.Log("Count" +faction.Units.Count);
            //     foreach (var unit in faction.Units)
            //     {
            //         Debug.Log("unit"+ unit.name);
            //         unit.OnLevelUp += LevelUp;
            //     }
            // }
        }

        public void Deactivate()
        {
           
        }

        public void Activate()
        {
      
        }

        private void UpdateUnit(Unit unit)
        {
            unit.OnLevelUp += LevelUp;
        }
        private void LevelUp(Unit unit)
        {
            int[] statIncreases = CalculateStatIncreases(unit.Growths.GetGrowthsArray());
            Debug.Log(" Level Up Progress!");
           
            levelUpRenderer.UpdateValues(unit.name, unit.ExperienceManager.Level-1, unit.ExperienceManager.Level, unit.Stats.GetStatArray(), statIncreases);
            AnimationQueue.Add(((IAnimation)levelUpRenderer).Play);
            unit.Stats.MaxHp += statIncreases[0];
            unit.Stats.MaxSp += statIncreases[1];
            unit.Stats.Str += statIncreases[2];
            unit.Stats.Mag += statIncreases[3];
            unit.Stats.Spd += statIncreases[4];
            unit.Stats.Skl += statIncreases[5];
            unit.Stats.Def += statIncreases[6];
            unit.Stats.Res += statIncreases[7];
        }

        public void DistributeExperience(IBattleActor attacker, IBattleActor defender)
        {
    
            if (attacker.IsAlive()&&attacker.Faction.IsPlayerControlled)
            {
                var exp = CalculateExperiencePoints(attacker, defender);
                ExpRenderer.UpdateValues(attacker.ExperienceManager.Exp,exp);
                AnimationQueue.Add(((IAnimation) ExpRenderer).Play);
                GridGameManager.Instance.GetSystem<UiSystem>().SelectedCharacter((Unit)attacker);
                attacker.ExperienceManager.AddExp(exp);
            }
            if (defender.IsAlive() && defender.Faction.IsPlayerControlled)
            {
                var exp = CalculateExperiencePoints(defender, attacker);
                ExpRenderer.UpdateValues(defender.ExperienceManager.Exp,exp);
                AnimationQueue.Add(((IAnimation) ExpRenderer).Play);
                GridGameManager.Instance.GetSystem<UiSystem>().SelectedCharacter((Unit)defender);
                defender.ExperienceManager.AddExp(exp);
            }
        }
        public int CalculateExperiencePoints(IBattleActor expReceiver, IBattleActor enemyFought)
        {
 
            int levelDifference = expReceiver.ExperienceManager.Level - enemyFought.ExperienceManager.Level;
            bool killEXP = !enemyFought.IsAlive();
            int expLeft = enemyFought.ExperienceManager.ExpLeftToDrain;
            int maxEXPDrain = ExperienceManager.MAX_EXP_TO_DRAIN;
  
            float chipExpPercent = 0.2f;
            float killExpPercent = 1.0f;
            int exp =(int)( killEXP == true ? killExpPercent * maxEXPDrain : chipExpPercent * maxEXPDrain);
      
            if(exp > expLeft)
            {
                exp = expLeft;
            }
            if(!killEXP&&expLeft-exp < maxEXPDrain / 5)
            {
                exp = expLeft - maxEXPDrain / 5;
            }
            enemyFought.ExperienceManager.ExpLeftToDrain -= exp;
            if (enemyFought.ExperienceManager.ExpLeftToDrain < 0)
                enemyFought.ExperienceManager.ExpLeftToDrain = 0;
            if (levelDifference < 0)
            {
                exp = (int)(exp * (1f + ((levelDifference * -1) / 10f)));
            }
            if (levelDifference >= 0)
            {
                exp = (int)(exp * (1f - ((levelDifference) / 10f)));
            }
            if (exp <= 0)
                exp = 0;
            Debug.Log("EXP : " +exp);
            return exp;
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