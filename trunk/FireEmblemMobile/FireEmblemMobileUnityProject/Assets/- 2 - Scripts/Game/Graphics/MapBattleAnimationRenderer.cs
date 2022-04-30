using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.PopUpText;
using Game.Mechanics;
using UnityEditor;
using UnityEngine;

public class MapBattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{

    private Unit attacker;
    private Unit defender;
  
    private int attackSequenceIndex;
    private List<AttackData> attackData;
    public void Show(BattleSimulation battleSimulation, IBattleActor attacker, IBattleActor defender)
    {
        
        this.attacker = (Unit)attacker;
        this.defender = (Unit)defender;
        attackSequenceIndex = 0;
        attacker.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += AttackerAttackConnected;
        defender.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += DefenderAttackConnected;
        attacker.GameTransformManager.UnitAnimator.OnAnimationEnded += BattleAnimation;
        defender.GameTransformManager.UnitAnimator.OnAnimationEnded += BattleAnimation;
        attackData = battleSimulation.AttacksData;
        BattleAnimation();
        //Invoke("Finished",2.0f);
    }

    private void AttackerAttackConnected()
    {
        int index = attackSequenceIndex - 1;
        if (attackData[index].hit)
        {
            DamagePopUp.CreateForBattleView(defender.GameTransformManager.GetCenterPosition()+new Vector3(0,0.2f),attackData[index].Dmg,
                TextStyle.Damage, 1.0f, new Vector3(0, .5f));
        }
        else
        {
            DamagePopUp.CreateMiss(defender.GameTransformManager.GetCenterPosition()+new Vector3(0,0.2f),
                TextStyle.Missed, 0.8f, new Vector3(0, .5f));
        }
    }

    private void DefenderAttackConnected()
    {
        int index = attackSequenceIndex - 1;
        if (attackData[index].hit)
        {
            DamagePopUp.CreateForBattleView(attacker.GameTransformManager.GetCenterPosition()+new Vector3(0,0.2f),attackData[index].Dmg,
                TextStyle.Damage, 1.0f, new Vector3(0, .5f));
        }
        else
        {
            DamagePopUp.CreateMiss(attacker.GameTransformManager.GetCenterPosition()+new Vector3(0,0.2f),
                TextStyle.Missed, 0.8f, new Vector3(0, .5f));
        }
    }
    IEnumerator DelayAction(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
    private void BattleAnimation()
        {

            if (attackSequenceIndex >= attackData.Count)
            {
                StartCoroutine(DelayAction(Finished, 0.4f));
                return;
            }

            int attackerX = (int)attacker.GameTransformManager.GetPosition().x;
            int attackerY = (int)attacker.GameTransformManager.GetPosition().y;
            int defenderX = (int)defender.GameTransformManager.GetPosition().x;
            int defenderY = (int)defender.GameTransformManager.GetPosition().y;
            if (attackData[attackSequenceIndex].attacker)
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

    void Finished()
    {
        OnFinished?.Invoke();
    }

    public void Hide()
    {
        if (attacker != null)
        {
            attacker.GameTransformManager.UnitAnimator.OnAttackAnimationConnected -= AttackerAttackConnected;
            attacker.GameTransformManager.UnitAnimator.OnAnimationEnded -= BattleAnimation;
        }

        if (defender != null)
        {
            defender.GameTransformManager.UnitAnimator.OnAttackAnimationConnected -= DefenderAttackConnected;

            defender.GameTransformManager.UnitAnimator.OnAnimationEnded -= BattleAnimation;
        }
        //throw new NotImplementedException();
    }

    public event Action OnFinished;
}
