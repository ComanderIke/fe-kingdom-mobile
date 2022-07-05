﻿using System;
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
        private List<Unit> units;
        private List<Faction> factions;

        public UnitProgressSystem(FactionManager fm)
        {
            factions = new List<Faction>();
            units = new List<Unit>();
            foreach(var faction in fm.Factions)
                AddFaction(faction);
        }
        private void AddFaction(Faction faction)
        {
            factions.Add(faction);
            foreach (var unit in faction.Units)
            {
                AddUnit(unit);
            }
            faction.OnAddUnit += AddUnit;
        }
        private void AddUnit(Unit u)
        {
            u.OnLevelUp -= LevelUp;
            u.OnLevelUp += LevelUp;
            units.Add(u);

        }

        public void Init()
        {
            
        }

        public void Deactivate()
        {
            foreach (var unit in units)
            {
                unit.OnLevelUp -= LevelUp;
            }
        }

        public void Activate()
        {
            foreach (var unit in units)
            {
                unit.OnLevelUp -= LevelUp;
                unit.OnLevelUp += LevelUp;
            }
        }


        private void LevelUp(Unit unit)
        {
            Debug.Log("LevelUp System Called!");
            int[] statIncreases = CalculateStatIncreases(unit.Growths.AsArray());

            if (levelUpRenderer != null)
            {
                levelUpRenderer.UpdateValues(unit.name, unit.visuals.CharacterSpriteSet.FaceSprite,unit.ExperienceManager.Level - 1, unit.ExperienceManager.Level,
                    unit.Stats.Attributes.AsArray(), statIncreases);
                Debug.Log("Add LevelUpAnimation!");
                AnimationQueue.Add(((IAnimation) levelUpRenderer).Play);
            }

            if (unit.ExperienceManager.Level % 2 == 0)
            {
                unit.SkillManager.SkillPoints += 1;
            }

            unit.Stats.Attributes.Update(statIncreases);
           
        }

        public int DistributeDefenderExperience(IBattleActor attacker, IBattleActor defender)
        {
            int exp=0;
            if (defender.IsAlive() && defender.Faction.IsPlayerControlled)
            {
                exp = CalculateExperiencePoints(defender, attacker);
                if (exp != 0)
                {


                    var expRenderer = ((Unit)defender).visuals.UnitCharacterCircleUI.GetExpRenderer();
                    expRenderer.UpdateValues(defender.ExperienceManager.GetMaxEXP(exp));
                   
                    GridGameManager.Instance.GetSystem<UiSystem>().SelectedCharacter((Unit)defender);

                    defender.ExperienceManager.AddExp(attacker.GameTransformManager.Transform.position, exp);
                }
            }

            return exp;
        }
        public int DistributeAttackerExperience(IBattleActor attacker, IBattleActor defender)
        {
            int exp = 0;
            Debug.Log("Distribute EXP"+ attacker.Faction.IsPlayerControlled+ " "+attacker.IsAlive());
            if (attacker.IsAlive()&&attacker.Faction.IsPlayerControlled)
            {
                exp = CalculateExperiencePoints(attacker, defender);
                if (exp != 0)
                {

                    
                    var expRenderer = ((Unit)attacker).visuals.UnitCharacterCircleUI.GetExpRenderer();
                    expRenderer.UpdateValues(attacker.ExperienceManager.GetMaxEXP(exp));
                 


                    GridGameManager.Instance.GetSystem<UiSystem>().SelectedCharacter((Unit)attacker);
                    attacker.ExperienceManager.AddExp(defender.GameTransformManager.Transform.position, exp);
                }

                
            }
            return exp;
            
        }
        private int CalculateExperiencePoints(IBattleActor expReceiver, IBattleActor enemyFought)
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
            bool atleast1 = false;
            while (!atleast1)
            {
                for (int i = 0; i < growths.Length; i++)
                {
                    increaseAmount[i] = Method(growths[i]);
                    if (increaseAmount[i] > 0)
                        atleast1 = true;
                }
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