using System;
using System.Collections;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;

public class AnimationStateManager
{
    public event Action OnFinished;
    
    public TimeLineController TimeLineController;
    public float timeBetweenAttacks = 1.0f;
    public float EndBattleWaitDuration = 0.5f;
    public CharacterCombatAnimations characterAnimations;
    public CombatTextRenderer CombatTextRenderer;
    private BattleSimulation battleSimulation;
    private CombatRound currentRound;
    private bool leftCharacterAttacker;
    private int attackSequenzIndex;
    private bool playerControlled;
 
    
    public AnimationStateManager(BattleSimulation battleSimulation)
    {
        this.battleSimulation = battleSimulation;
        TimeLineController.zoomInFinished += ContinueBattle;
        leftCharacterAttacker = battleSimulation.Attacker.Faction==null||battleSimulation.Attacker.Faction.IsPlayerControlled;
        characterAnimations.Reset();
        characterAnimations.SetLeftCharacterAttacker(leftCharacterAttacker);
        playerControlled = battleSimulation.Attacker.Faction == null || battleSimulation.Attacker.Faction.IsPlayerControlled;
        attackSequenzIndex = 0;
        CombatTextRenderer = new CombatTextRenderer();
        
    }
    public void Start()
    {
        currentRound = battleSimulation.combatRounds[0];
        if (playerControlled)
        {
            characterAnimations.SpawnLeftCharacter((Unit)battleSimulation.Attacker);
            characterAnimations.SpawnRightCharacter((Unit)battleSimulation.Defender);
        }
        else
        {
            characterAnimations.SpawnLeftCharacter((Unit)battleSimulation.Defender);
            characterAnimations.SpawnRightCharacter((Unit)battleSimulation.Attacker);
        }
        TimeLineController.Start(playerControlled);
        
        characterAnimations.SetPlaySpeed(TimeLineController.introWalkInPlaySpeed);
        characterAnimations.WalkIn(playerControlled);
    }
    private void ContinueBattle()
    {
        bool prepare = false;
        if (attackSequenzIndex >= currentRound.AttacksData.Count)
        {
            AllAttacksFinished();
            return;
        }
        
        characterAnimations.CharacterAttack(currentRound.AttacksData[attackSequenzIndex].attacker, leftCharacterAttacker);
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
        MonoUtility.DelayFunction(BattleFinished, EndBattleWaitDuration);
    }
    public void BattleFinished()
    {
        OnFinished?.Invoke();
    }

   

    
}