using System;
using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using Assets.GUI.PopUpText;
using Assets.Mechanics;
using Assets.Utility;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.GameInput;

namespace Assets.GUI
{
    public class BattleRenderer : MonoBehaviour
    {
        private const float DELAY = 0.4f;

        public delegate void OnFinishedEvent();

        public static OnFinishedEvent OnFinished;
        public delegate void OnAttackConnectedEvent();

        public static OnAttackConnectedEvent OnAttackConnected;


        private Unit attacker;
        private Unit defender;
        private bool[] attackSequence;
        private int attackSequenceIndex;

        private void OnEnable()
        {
            InputSystem.OnSetActive?.Invoke(false);
        }

        private void OnDisable()
        {
            InputSystem.OnSetActive?.Invoke(true);
        }

        public void Show(Unit attacker, Unit defender, bool[] attackSequence)
        {
            this.attacker = attacker;
            this.defender = defender;

            gameObject.SetActive(true);
            this.attackSequence = attackSequence;
            foreach (bool b in attackSequence)
            {
                Debug.Log(b);
            }
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
            attacker.GameTransform.UnitController.OnAttackAnimationConnected += AttackConnected;
            defender.GameTransform.UnitController.OnAttackAnimationConnected += AttackConnected;
            attacker.GameTransform.UnitController.OnAnimationEnded += ContinueBattleAnimation;
            defender.GameTransform.UnitController.OnAnimationEnded += ContinueBattleAnimation;
            ContinueBattleAnimation();
        }

        private void AttackConnected()
        {
            OnAttackConnected();
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
                    attacker.GameTransform.UnitController.BattleAnimationLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationRight();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationUp();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationDown();

                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationDownLeft();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationUpLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationUpRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    attacker.GameTransform.UnitController.BattleAnimationDownRight();
          
            }
            else
            {
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y == defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationLeft();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationDown();
                if (attacker.GridPosition.X == defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationUp();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationUpRight();
                if (attacker.GridPosition.X > defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationDownRight();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y < defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationDownLeft();
                if (attacker.GridPosition.X < defender.GridPosition.X && attacker.GridPosition.Y > defender.GridPosition.Y)
                    defender.GameTransform.UnitController.BattleAnimationUpLeft();
            }

            attackSequenceIndex++;
        }
        private void EndBattleAnimation()
        {
            attacker.GameTransform.UnitController.OnAttackAnimationConnected -= AttackConnected;
            defender.GameTransform.UnitController.OnAttackAnimationConnected -= AttackConnected;
            attacker.GameTransform.UnitController.OnAnimationEnded -= ContinueBattleAnimation;
            defender.GameTransform.UnitController.OnAnimationEnded -= ContinueBattleAnimation;
            OnFinished?.Invoke();
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowCounterDamageText(int damage)
        {
            GridGameManager.Instance.GetSystem<PopUpTextSystem>()
                .CreateAttackPopUpTextRed("" + damage, attacker.GameTransform.GameObject.transform);
        }

        public void ShowDamageText(int damage)
        {
            GridGameManager.Instance.GetSystem<PopUpTextSystem>()
                    .CreateAttackPopUpTextRed("" + damage, defender.GameTransform.GameObject.transform);
        }


       

    }
}