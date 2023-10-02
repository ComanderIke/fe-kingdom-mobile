using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.Mechanics;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;
using UnityEngine.Rendering;
using AttackData = Game.Mechanics.AttackData;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public BattleCanvasController canvas;
    
    private AnimationStateManager animationStateManager;
    [SerializeField] private SkillActivationRenderer skillActivationRenderer;
    public static event Action<BattleSimulation,BattlePreview, IBattleActor, IAttackableTarget> OnShow;
    
    public Volume volume;
    public event Action<int> OnFinished;


    void ShowActivatedAttackSkills(AttackData attackData)
    {
        skillActivationRenderer.Show(attackData.activatedAttackSkills, attackData.attacker);
        skillActivationRenderer.Show(attackData.activatedDefenseSkills, !attackData.attacker);
    }
    void ShowActivatedCombatSkills(List<Skill> skills, bool attacker)
    {
        skillActivationRenderer.Show(skills, attacker);
    }

    void Surrender()
    {
        animationStateManager.Surrender();
    }
    public void Show(BattleSimulation battleSimulation, BattlePreview battlePreview, IBattleActor attackingActor, IAttackableTarget defendingActor)
    {
        gameObject.SetActive(true);
        battleSimulation = battleSimulation;
        canvas.Show();
        Debug.Log("Test: "+attackingActor+" "+defendingActor);
        OnShow?.Invoke(battleSimulation,battlePreview, attackingActor, defendingActor);
        BattleUI.onSurrender -= Surrender;
        BattleUI.onSurrender += Surrender;
        BattleUI.onSkip -= Skip;
        BattleUI.onSkip += Skip;
       
        animationStateManager = new AnimationStateManager(attackingActor, defendingActor, battleSimulation, GetComponent<TimeLineController>(),GetComponent<CharacterCombatAnimations>());
        animationStateManager.OnCharacterAttack -= ShowActivatedAttackSkills;
        animationStateManager.OnCharacterAttack += ShowActivatedAttackSkills;
        ShowActivatedCombatSkills(battleSimulation.AttackerActivatedCombatSkills, true);
        ShowActivatedCombatSkills(battleSimulation.DefenderActivatedCombatSkills, false);
        animationStateManager.Start();
        animationStateManager.OnFinished -= Finished;
        animationStateManager.OnFinished += Finished;
       
        
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) => { volume.weight = value; });
        
    }

    private BattleSimulation battleSimulation;
    
    void Finished(int lastCombatRoundIndex)
    {
        Cleanup();
        BattleUI.onSurrender -= Surrender;
        BattleUI.onSkip -= Skip;
        OnFinished?.Invoke(lastCombatRoundIndex);
    }

    public void Skip()
    {
        if (animationStateManager!=null)
        {
            Debug.Log("UPDATE BUTTON CLICKED");
            CancelInvoke();//TODO DO THIS ON COROUTINE MONOBEHAVIOUR 
            Debug.Log("TODO Reset Cameras and Volumes!");
            if(battleSimulation != null && battleSimulation.combatRounds!=null)
                animationStateManager.BattleFinished(battleSimulation.combatRounds.Count-1);
            else
            {
                animationStateManager.BattleFinished(0);
            }
            //Hide(); Hide should be called from battle finished event
        }
    }
    public void Cleanup()
    {
        animationStateManager?.CleanUp();
    }
    public void Hide()
    {
        canvas.Hide();
        //light.SetActive(false);
        LeanTween.value(volume.weight, 0, 0.4f).setEaseInQuad().setOnUpdate((value) => { volume.weight = value; })
            .setOnComplete(() => gameObject.SetActive(false));
    }
}