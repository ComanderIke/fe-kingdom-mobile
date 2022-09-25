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
            foreach (var defender in defenders)
            {
                if (!attacker.IsAlive())
                {
                    attacker.Die();
                }
                if (defender is Unit unitDefender)
                {
                    
                    progressSystem.DistributeExperience(attacker, unitDefender);
                    yield return new WaitUntil(()=>progressSystem.IsFinished());
                    progressSystem.DistributeExperience(unitDefender, attacker);
                    yield return new WaitUntil(()=>progressSystem.IsFinished());
                }
                if (!defender.IsAlive())
                {
                    defender.Die();
                }
            }
            Finished();
            
        }
        void Finished()
        {
            Debug.Log("Finished AfterBattle task");
            OnFinished?.Invoke();
            
        }
    }
}