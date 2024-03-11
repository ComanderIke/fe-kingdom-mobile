﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.Systems;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class AfterBattleTasks
    {

        public static event Action OnFinished;
        private Unit attacker;
        private UnitProgressSystem progressSystem;
        private const float WaitTimeWhenFinished = 1.5f;
        private const float initialDelay= 0.5f;
        private List<IAttackableTarget> defenders;
        private bool expForTargets;
        public AfterBattleTasks(UnitProgressSystem system,Unit attacker, IAttackableTarget defender, bool expForTarget=true)
        {
            this.progressSystem = system;
            this.attacker = attacker;
            this.expForTargets = expForTarget;
            defenders = new List<IAttackableTarget>();
            defenders.Add(defender);
        }
        public AfterBattleTasks(UnitProgressSystem system,Unit attacker, List<IAttackableTarget> defenders, bool expForTargets=true)
        {
            this.progressSystem = system;
            this.attacker = attacker;
            this.defenders = defenders;
            this.expForTargets = expForTargets;
        }
        public void StartTask()
        {


            CheckForItemDrops();
            ServiceProvider.Instance.StartChildCoroutine(ExpCoroutine());


        }

        void CheckForItemDrops()
        {
            foreach (var defender in defenders)
            {
                var unit = ((Unit)defender);
                if (!defender.IsAlive())
                {
                    if (unit.DropableItem != null)
                    {
                        Player.Instance.Party.AddItem(unit.DropableItem);
                    }
                }
            }
        }

        IEnumerator ExpForAttacker(Unit defenderAsUnit)
        {
            int lvl = attacker.ExperienceManager.Level;
            progressSystem.DistributeExperience(defenderAsUnit, attacker);
            yield return new WaitUntil(() => progressSystem.IsFinished());
        }

        IEnumerator ExpForDefender(Unit defenderAsUnit)
        {
            int lvl = defenderAsUnit.ExperienceManager.Level;
            progressSystem.DistributeExperience(attacker, defenderAsUnit);
            yield return new WaitUntil(() => progressSystem.IsFinished());
        }
        IEnumerator ExpCoroutine()
        {
            yield return new WaitForSeconds(initialDelay);

            if (attacker is Unit unitAttacker && attacker.IsPlayerControlled())
            {
                if(unitAttacker.Blessing!=null)
                    unitAttacker.Bonds.Increase(unitAttacker.Blessing.God, unitAttacker.Stats.CombinedAttributes().FAITH);
            }
            
            //Debug.Log("Start EXP Coroutine: "+attacker+" "+defenders.Count);
            if(defenders!=null)
                foreach (var defender in defenders)
                {
                    if (!attacker.IsAlive())
                    {
                        attacker.Die((Unit)defender);
                        yield return new WaitForSeconds(1.5f);
                    }
                    if (!defender.IsAlive())
                    {
                        defender.Die(attacker);
                        yield return new WaitForSeconds(1.5f);
                    }
                    if (defender is Unit unitDefender)
                    {
                        

                        if(attacker.IsAlive()&& attacker.IsPlayerControlled())
                            yield return ExpForAttacker(unitDefender);
                        
                        if (defender.IsAlive() && unitDefender.IsPlayerControlled())
                        {
                            if(unitDefender.Blessing!=null)
                                unitDefender.Bonds.Increase(unitDefender.Blessing.God, unitDefender.Stats.CombinedAttributes().FAITH);
                            if(expForTargets)
                                yield return ExpForDefender(unitDefender);
                        }

                    }
                    
                }
            //yield return new WaitForSeconds(WaitTimeWhenFinished);
            Finished();
            
        }
        void Finished()
        {
            OnFinished?.Invoke();
            
        }
    }
}