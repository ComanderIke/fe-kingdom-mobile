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
       
        private List<IAttackableTarget> defenders;
        public AfterBattleTasks(UnitProgressSystem system,Unit attacker, IAttackableTarget defender)
        {
            this.progressSystem = system;
            this.attacker = attacker;
            defenders = new List<IAttackableTarget>();
            defenders.Add(defender);
        }
        public AfterBattleTasks(UnitProgressSystem system,Unit attacker, List<IAttackableTarget> defenders)
        {
            this.progressSystem = system;
            this.attacker = attacker;
            this.defenders = defenders;
        }
        public void StartTask()
        {
            Debug.Log("Start AfterBattle task");
           
            Debug.Log("Attacker: "+attacker+" , DefenderCount: "+defenders.Count);

            ServiceProvider.Instance.StartChildCoroutine(ExpCoroutine());


        }

        IEnumerator ExpCoroutine()
        {
            Debug.Log("Start EXP Coroutine: "+attacker+" "+defenders.Count);
            foreach (var defender in defenders)
            {
                Debug.Log( "Attacker: "+attacker+"Defender: "+defender);
                if (!attacker.IsAlive())
                {
                    attacker.Die();
                }
                if (defender is Unit unitDefender)
                {
                    int lvl = unitDefender.ExperienceManager.Level;
                    progressSystem.DistributeExperience(attacker, unitDefender);
                    yield return new WaitUntil(() => progressSystem.IsFinished());
                    if (lvl < unitDefender.ExperienceManager.Level)
                    {
                        Debug.Log("Start LevelUp  defender");
                        yield return new WaitForSeconds(.5f);
                        progressSystem.DoLevelUp(unitDefender);
                        yield return new WaitUntil(() => progressSystem.IsFinished());
                    }

                    progressSystem.DistributeExperience(unitDefender, attacker);
                    yield return new WaitUntil(()=>progressSystem.IsFinished());
                    if (lvl < attacker.ExperienceManager.Level)
                    {
                        Debug.Log("Start LevelUp  attacker");
                        yield return new WaitForSeconds(.5f);
                        progressSystem.DoLevelUp(attacker);
                        yield return new WaitUntil(() => progressSystem.IsFinished());
                    }
             
                }
                if (!defender.IsAlive())
                {
                    defender.Die();
                }
            }
            Debug.Log("Almost Finished");
            yield return new WaitForSeconds(WaitTimeWhenFinished);
            Finished();
            
        }
        void Finished()
        {
            Debug.Log("Finished AfterBattle task");
            OnFinished?.Invoke();
            
        }
    }
}