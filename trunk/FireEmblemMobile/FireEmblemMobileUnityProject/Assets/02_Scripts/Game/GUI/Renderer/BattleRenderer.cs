using System;
using System.Collections;
using Game.GameActors.Units.Interfaces;
using Game.Graphics.Interfaces;
using UnityEngine;

namespace Game.GUI.Renderer
{
    public class BattleRenderer : MonoBehaviour, IBattleRenderer
    {
        private const float DELAY = 0.4f;

        public delegate void OnFinishedEvent();
        
        public static OnFinishedEvent OnFinished;
        public delegate void OnAttackConnectedEvent(IBattleActor attacker, IBattleActor defender);

        public static OnAttackConnectedEvent OnAttackConnected;


        public Canvas canvas;
        private IBattleActor attacker;
        private IBattleActor defender;
        private bool[] attackSequence;
        private int attackSequenceIndex;

    

        public void Show(IBattleActor attacker, IBattleActor defender, bool[] attackSequence)
        {
            this.attacker = attacker;
            this.defender = defender;
            canvas.enabled = true;
            //GridInputSystem.SetActive(false);
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
            attacker.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += AttackerAttackConnected;
            defender.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += DefenderAttackConnected;
            attacker.GameTransformManager.UnitAnimator.OnAnimationEnded += ContinueBattleAnimation;
            defender.GameTransformManager.UnitAnimator.OnAnimationEnded += ContinueBattleAnimation;
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

            int attackerX = (int)attacker.GameTransformManager.GetPosition().x;
            int attackerY = (int)attacker.GameTransformManager.GetPosition().y;
            int defenderX = (int)defender.GameTransformManager.GetPosition().x;
            int defenderY = (int)defender.GameTransformManager.GetPosition().y;
            if (attackSequence[attackSequenceIndex])
            {
                if(attackerX> defenderX && attackerY == defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationLeft();
                if (attackerX < defenderX && attackerY ==defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationRight();
                if (attackerX == defenderX && attackerY< defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationUp();
                if (attackerX == defenderX && attackerY > defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationDown();

                if (attackerX> defenderX && attackerY > defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationDownLeft();
                if (attackerX > defenderX && attackerY < defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationUpLeft();
                if (attackerX< defenderX&& attackerY < defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationUpRight();
                if (attackerX < defenderX && attackerY > defenderY)
                    attacker.GameTransformManager.UnitAnimator.BattleAnimationDownRight();
          
            }
            else
            {
                if (attackerX> defenderX && attackerY == defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationRight();
                if (attackerX < defenderX && attackerY == defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationLeft();
                if (attackerX == defenderX && attackerY < defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationDown();
                if (attackerX == defenderX&& attackerY > defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationUp();
                if (attackerX > defenderX && attackerY > defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationUpRight();
                if (attackerX > defenderX && attackerY < defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationDownRight();
                if (attackerX < defenderX && attackerY < defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationDownLeft();
                if (attackerX < defenderX && attackerY > defenderY)
                    defender.GameTransformManager.UnitAnimator.BattleAnimationUpLeft();
            }

            attackSequenceIndex++;
        }
        private void EndBattleAnimation()
        {
            attacker.GameTransformManager.UnitAnimator.OnAttackAnimationConnected -= AttackerAttackConnected;
            defender.GameTransformManager.UnitAnimator.OnAttackAnimationConnected -= DefenderAttackConnected;
            attacker.GameTransformManager.UnitAnimator.OnAnimationEnded -= ContinueBattleAnimation;
            defender.GameTransformManager.UnitAnimator.OnAnimationEnded -= ContinueBattleAnimation;
            OnFinished?.Invoke();
            Hide();
        }

        public void Hide()
        {
            //GridInputSystem.SetActive(true);
            canvas.enabled = false;
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