using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.PopUpText;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEditor;
using UnityEngine;
using AttackData = Game.Mechanics.AttackData;

public class MapBattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{

    private Unit attacker;
    private Unit defender;
    private Destroyable Destroyable;
  
    private int attackSequenceIndex;
    private List<AttackData> attackData;
    public void Show(BattleSimulation battleSimulation,BattlePreview battlePreview, IBattleActor attacker, IAttackableTarget defender)
    {
        
        this.attacker = (Unit)attacker;
        Destroyable = null;
        attackSequenceIndex = 0;
        if (defender is Destroyable)
        {
            Destroyable = (Destroyable)defender;
        }
        else
        {
            this.defender = (Unit)defender;
            
            this.defender.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += DefenderAttackConnected;
            this.defender.GameTransformManager.UnitAnimator.OnAnimationEnded += BattleAnimation;
        }

        attacker.GameTransformManager.UnitAnimator.OnAttackAnimationConnected += AttackerAttackConnected;
       
        attacker.GameTransformManager.UnitAnimator.OnAnimationEnded += BattleAnimation;
       
        attackData = battleSimulation.combatRounds[0].AttacksData;
        BattleAnimation();
        //Invoke("Finished",2.0f);
    }

    private void AttackerAttackConnected()
    {
        int index = attackSequenceIndex - 1;
        if (attackData[index].hit)
        {
            if (Destroyable != null)
            {
                DamagePopUp.CreateForBattleView(
                    Destroyable.Controller.GetCenterPosition() + new Vector3(0, 0.2f), attackData[index].Dmg,
                    TextStyle.Damage, 1.0f, new Vector3(0, .5f));
            }
            else
            {
                DamagePopUp.CreateForBattleView(
                    defender.GameTransformManager.GetCenterPosition() + new Vector3(0, 0.2f), attackData[index].Dmg,
                    TextStyle.Damage, 1.0f, new Vector3(0, .5f));
            }
        }
        else
        {
            if (Destroyable != null)
            {
                DamagePopUp.CreateForBattleView(
                    Destroyable.Controller.GetCenterPosition() + new Vector3(0, 0.2f), attackData[index].Dmg,
                    TextStyle.Damage, 1.0f, new Vector3(0, .5f));
            }
            else
            {
                DamagePopUp.CreateMiss(defender.GameTransformManager.GetCenterPosition() + new Vector3(0, 0.2f),
                    TextStyle.Missed, 0.8f, new Vector3(0, .5f));
            }
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

            Debug.Log("Attack Round: "+attackSequenceIndex);
            if (attackSequenceIndex >= attackData.Count)
            {
                StartCoroutine(DelayAction(Finished, 0.4f));
                return;
            }

            int attackerX = (int)attacker.GameTransformManager.GetPosition().x;
            int attackerY = (int)attacker.GameTransformManager.GetPosition().y;
            int defenderX=0;
            int defenderY=0;
            if (Destroyable == null)
            {
                defenderX = (int)defender.GameTransformManager.GetPosition().x;
                defenderY = (int)defender.GameTransformManager.GetPosition().y;
            }
            else
            {
                defenderX = Destroyable.Controller.X;
                defenderY = Destroyable.Controller.Y;
            }

            if (attackData[attackSequenceIndex].attacker)
            {
                Debug.Log("BattleAnimationTest; "+ attackerX+" "+attackerY+" "+defenderX+" "+defenderY);
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
            else if(Destroyable==null)
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
        Debug.Log("BattleAnimationFinished");
        OnFinished?.Invoke(0);
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

    public event Action<int> OnFinished;
    public void Cleanup()
    {
        
    }
}
