using System;
using System.Collections;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;

public class AnimationStateManager
{
    public event Action OnFinished;
    
    private TimeLineController TimeLineController;
    public float timeBetweenAttacks = 1.0f;
    public float EndBattleWaitDuration = 0.5f;
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
       
        TimeLineController.zoomInFinished += ContinueBattle;
        playerControlled = (Player.Instance.Party.members.Contains((Unit)battleSimulation.Attacker));
        leftCharacterAttacker = playerControlled;
      
        characterAnimations.Reset();
        characterAnimations.SetLeftCharacterAttacker(leftCharacterAttacker);
        
     
        attackSequenzIndex = 0;
        CombatTextRenderer = new CombatTextRenderer();
        
    }
    public void Start()
    {
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
        characterAnimations.Init(TimeLineController.camera);
        TimeLineController.Init(playerControlled);
        
        characterAnimations.SetPlaySpeed(TimeLineController.introWalkInPlaySpeed);
        characterAnimations.WalkIn(playerControlled);
        characterAnimations.SetPlaySpeed(1.0f);
    }
    private void ContinueBattle()
    {
     
        if (attackSequenzIndex >= currentRound.AttacksData.Count)
        {
            AllAttacksFinished();
           
            return;
        }
        
        characterAnimations.CharacterAttack(currentRound.AttacksData[attackSequenzIndex],currentRound.AttacksData[attackSequenzIndex].attacker, leftCharacterAttacker);
        characterAnimations.OnAttackFinished -= AttackFinished;
        characterAnimations.OnAttackFinished += AttackFinished;

        TimeLineController.CameraShake();

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
             MonoUtility.DelayFunction(TimeLineController.PlayZoomIn, timeBetweenAttacks);
         }
    }
    
    private void AllAttacksFinished()
    {
        TimeLineController.zoomOutFinished -= AllAttacksFinished;
        if (currentRound.RoundIndex >= battleSimulation.combatRounds.Count-1)
        {
           
            MonoUtility.DelayFunction(BattleFinished, EndBattleWaitDuration);
        }
        else
        {
            
            currentRound = battleSimulation.combatRounds[currentRound.RoundIndex + 1];
            attackSequenzIndex = 0;
            MonoUtility.DelayFunction(TimeLineController.PlayZoomIn, timeBetweenAttacks);
        }
    }
    public void BattleFinished()
    {
        OnFinished?.Invoke();
    }

    public void CleanUp()
    {
        characterAnimations.Cleanup();
    }

   

    
}