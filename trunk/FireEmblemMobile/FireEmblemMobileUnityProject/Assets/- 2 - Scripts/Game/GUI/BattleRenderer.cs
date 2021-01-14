using System;
using System.Collections;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.PopUpText;
using Game.Manager;
using UnityEngine;

namespace Game.GUI
{
    public class BattleRenderer : MonoBehaviour
    {
        private const float DELAY = 0.4f;

        public delegate void OnFinishedEvent();

        public static OnFinishedEvent OnFinished;
        public delegate void OnAttackConnectedEvent(Unit attacker, Unit defender);

        public static OnAttackConnectedEvent OnAttackConnected;


        private Unit attacker;
        private Unit defender;
        private bool[] attackSequence;
        private int attackSequenceIndex;

        private void OnEnable()
        {
            GridInputSystem.SetActive(false);
        }

        private void OnDisable()
        {
            GridInputSystem.SetActive(true);
        }

        public void Show(Unit attacker, Unit defender, bool[] attackSequence)
        {
            this.attacker = attacker;
            this.defender = defender;

            gameObject.SetActive(true);
            this.attackSequence = attackSequence;
            //foreach (bool b in attackSequence)
            //{
            //    Debug.Log(b);
            //}
            StartCoroutine(DelayAction(StartBattleAnimation, DELAY));
        }
        IEnumerator DelayAction(Action callback, float delay)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
        private void StartBattleAnimation()
        {
            attackSequenceIndex = 0;
            attacker.GameTransform.UnitAnimator.OnAttackAnimationConnected += AttackerAttackConnected;
            defender.GameTransform.UnitAnimator.OnAttackAnimationConnected += DefenderAttackConnected;
            attacker.GameTransform.UnitAnimator.OnAnimationEnded += ContinueBattleAnimation;
            defender.GameTransform.UnitAnimator.OnAnimationEnded += ContinueBattleAnimation;
            ContinueBattleAnimation();
        }

        private void AttackerAttackConnected()
        {
            OnAttackConnected(attacker, defender);
        }
        private void DefenderAttackConnected()
        {
            OnAttackConnected(defender, attacker);
        }
        private void ContinueBattleAnimation()
        {
            StartCoroutine(DelayAction(BattleAnimation, DELAY));

        }
        private void BattleAnimation()
        {

            if (attackSequenceIndex >= attackSequence.Length)
            {
                StartCoroutine(DelayAction(EndBattleAnimation, DELAY));
                return;
            }
            if (attackSequence[attackSequenceIndex])
            {
                if(attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationRight();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationUp();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationDown();

                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationDownLeft();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationUpLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationUpRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitAnimator.BattleAnimationDownRight();
          
            }
            else
            {
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationLeft();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationDown();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationUp();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationUpRight();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationDownRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationDownLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitAnimator.BattleAnimationUpLeft();
            }

            attackSequenceIndex++;
        }
        private void EndBattleAnimation()
        {
            attacker.GameTransform.UnitAnimator.OnAttackAnimationConnected -= AttackerAttackConnected;
            defender.GameTransform.UnitAnimator.OnAttackAnimationConnected -= DefenderAttackConnected;
            attacker.GameTransform.UnitAnimator.OnAnimationEnded -= ContinueBattleAnimation;
            defender.GameTransform.UnitAnimator.OnAnimationEnded -= ContinueBattleAnimation;
            OnFinished?.Invoke();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowCounterDamageText(int damage)
        {
            // GridGameManager.Instance.GetSystem<PopUpTextSystem>()
            //     .CreateAttackPopUpTextRed("" + damage, attacker.GameTransform.GameObject.transform);
        }

        public void ShowDamageText(int damage)
        {
            // GridGameManager.Instance.GetSystem<PopUpTextSystem>()
            //         .CreateAttackPopUpTextRed("" + damage, defender.GameTransform.GameObject.transform);
        }


       

    }
}