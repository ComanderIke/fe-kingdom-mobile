﻿using System;
using System.Collections;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;
using AttackData = Game.Mechanics.AttackData;

public class AnimationStateManager
{
    public event Action<int> OnFinished;
    
    private TimeLineController TimeLineController;
    public float timeBetweenAttacks = 0.40f;
    public float EndBattleWaitDuration = 0.3f;
    private CharacterCombatAnimations characterAnimations;
    public CombatTextRenderer CombatTextRenderer;
    private BattleSimulation battleSimulation;
    private CombatRound currentRound;
    private bool leftCharacterAttacker;
    private int attackSequenzIndex;
    private bool playerControlled;
    private IBattleActor realAttacker;
    private IBattleActor realDefender;
    
 
    
    public AnimationStateManager(IBattleActor realAttacker, IAttackableTarget realDefender, BattleSimulation battleSimulation, TimeLineController timeLineController, CharacterCombatAnimations characterAnimations)
    {
        this.realAttacker = realAttacker;
        if(realDefender is IBattleActor)
            this.realDefender = (IBattleActor)realDefender;
        this.battleSimulation = battleSimulation;
        this.TimeLineController = timeLineController;
        this.characterAnimations = characterAnimations;
       
     
        playerControlled = (Player.Instance.Party.members.Contains((Unit)battleSimulation.Attacker));
        leftCharacterAttacker = playerControlled;
      
        characterAnimations.Reset();
        characterAnimations.SetLeftCharacterAttacker(leftCharacterAttacker);
        
     
        attackSequenzIndex = 0;
        CombatTextRenderer = new CombatTextRenderer();
        
    }
    public void Start()
    {
        surrender = false;
        TimeLineController.zoomInFinished -= ContinueBattle;
        TimeLineController.zoomInFinished += ContinueBattle;
        currentRound = battleSimulation.combatRounds[0];
        if (playerControlled)
        {
            characterAnimations.SpawnLeftCharacter((Unit)realAttacker);
            characterAnimations.SpawnRightCharacter((Unit)realDefender);
        }
        else
        {
            characterAnimations.SpawnLeftCharacter((Unit)realDefender);
            characterAnimations.SpawnRightCharacter((Unit)realAttacker);
        }
        TimeLineController.Init(playerControlled);
        
       // characterAnimations.SetPlaySpeed(TimeLineController.introWalkInPlaySpeed);
       MonoUtility.InvokeNextFrame(() =>
       {
           characterAnimations.WalkIn(playerControlled);
       });

       //characterAnimations.SetPlaySpeed(1.0f);
    }

    public event Action<IBattleActor, AttackData> OnCharacterAttack;
    public event Action<IBattleActor, AttackData> OnPreCharacterAttack;
    private bool surrender = false;
    private void ContinueBattle()
    {
     
      
        if (attackSequenzIndex >= currentRound.AttacksData.Count)
        {
            AllAttacksFinished();
           
            return;
        }
        OnCharacterAttack?.Invoke(currentRound.AttacksData[attackSequenzIndex].attacker?realAttacker:realDefender,currentRound.AttacksData[attackSequenzIndex]);
       // OnPreCharacterAttack?.Invoke(currentRound.AttacksData[attackSequenzIndex].attacker?realAttacker:realDefender,currentRound.AttacksData[attackSequenzIndex]);
       
        characterAnimations.CharacterAttack(currentRound.AttacksData[attackSequenzIndex],currentRound.AttacksData[attackSequenzIndex].attacker, leftCharacterAttacker);
        characterAnimations.OnAttackFinished -= AttackFinished;
        characterAnimations.OnAttackFinished += AttackFinished;

        TimeLineController.CameraShake();

    }

    public void Surrender()
    {
        surrender = true;
    }
    void AttackFinished()
    {
        attackSequenzIndex++;
        FinishAttack();
    }

    private void FinishAttack()
    {
        TimeLineController.PlayZoomOut();
       
         if (attackSequenzIndex >= currentRound.AttacksData.Count)
         {
             TimeLineController.zoomOutFinished -= AllAttacksFinished;
             TimeLineController.zoomOutFinished += AllAttacksFinished;
         }
         else
         {
             MonoUtility.DelayFunction(this,TimeLineController.PlayZoomIn, timeBetweenAttacks);
         }
    }

   
    private void AllAttacksFinished()
    {
        TimeLineController.zoomOutFinished -= AllAttacksFinished;
        if (surrender||currentRound.RoundIndex >= battleSimulation.combatRounds.Count-1)
        {
           
            MonoUtility.DelayFunction(this,()=>BattleFinished(currentRound.RoundIndex), EndBattleWaitDuration);
        }
        else
        {
            
            currentRound = battleSimulation.combatRounds[currentRound.RoundIndex + 1];
            attackSequenzIndex = 0;
            MonoUtility.DelayFunction(this, TimeLineController.PlayZoomIn, timeBetweenAttacks);
        }
    }
    public void BattleFinished(int index)
    {
        
        OnFinished?.Invoke(index);
    }

    public void CleanUp()
    {
        
        TimeLineController.zoomInFinished -= ContinueBattle;
        TimeLineController.zoomOutFinished -= AllAttacksFinished;
        characterAnimations.OnAttackFinished -= AttackFinished;
        MonoUtility.StopCoroutines(this);
        characterAnimations.Cleanup();
        CombatTextRenderer.Cleanup();
    }

   

    
}