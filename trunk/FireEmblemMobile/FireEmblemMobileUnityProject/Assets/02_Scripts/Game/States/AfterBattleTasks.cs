using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class AfterBattleTasks
    {

        public event Action OnFinished;
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
         

            ServiceProvider.Instance.StartChildCoroutine(ExpCoroutine());


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
                            if(expForTargets)
                                yield return ExpForDefender(unitDefender);
                        }

                    }
                    
                }
            yield return new WaitForSeconds(WaitTimeWhenFinished);
            Finished();
            
        }
        void Finished()
        {
            OnFinished?.Invoke();
            
        }
    }
}