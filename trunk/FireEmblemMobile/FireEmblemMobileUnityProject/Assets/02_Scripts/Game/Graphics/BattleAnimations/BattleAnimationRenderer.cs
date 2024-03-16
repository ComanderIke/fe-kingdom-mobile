using System;
using System.Collections.Generic;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.GUI;
using Game.GUI.Battles;
using Game.GUI.Controller;
using Game.Manager;
using Game.States;
using Game.States.Mechanics;
using Game.States.Mechanics.Battle;
using Game.Systems;
using UnityEngine;
using UnityEngine.Rendering;
using AttackData = Game.States.Mechanics.AttackData;

namespace Game.Graphics.BattleAnimations
{
    public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
    {
        public BattleCanvasController canvas;
    
        private AnimationStateManager animationStateManager;
        [SerializeField] private SkillActivationRenderer skillActivationRenderer;
        public static event Action<BattleSimulation,BattlePreview, IBattleActor, IAttackableTarget> OnShow;
    
        public Volume volume;
        public event Action<int> OnFinished;


        void ShowActivatedAttackSkills(IBattleActor activater, AttackData attackData)
        {
            bool playerControlled = false;
            if(activater.Faction!=null)
                playerControlled = activater.Faction.IsPlayerControlled;
            else
            {
                playerControlled = ((Unit)activater).Party!=null;
            }
            skillActivationRenderer.Show((Unit)activater, attackData.activatedAttackSkills, playerControlled);
            Debug.Log("ACTIVATE DEFENSE SKILL: "+activater+" "+attackData.attacker);
            skillActivationRenderer.Show((Unit)activater, attackData.activatedDefenseSkills, !playerControlled);
        }
        void ShowActivatedCombatSkills(Unit activater,List<Skill> skills, bool attacker)
        {
            skillActivationRenderer.Show(activater, skills, attacker);
        }

        void Surrender()
        {
            animationStateManager.Surrender();
        }
        public void Show(BattleSimulation battleSimulation, BattlePreview battlePreview, IBattleActor attackingActor, IAttackableTarget defendingActor)
        {
            var uiSystem = ServiceProvider.Instance.GetSystem<UiSystem>();
            if (uiSystem != null)
            {
                uiSystem.HideMainCanvas();
                uiSystem.BottomUI.HideAll();
                uiSystem.charCircleCanvas.enabled = false;
            }
            
            gameObject.SetActive(true);
            this.battleSimulation = battleSimulation;
            canvas.Show();
            // Debug.Log("Test: "+attackingActor+" "+defendingActor);
            OnShow?.Invoke(battleSimulation,battlePreview, attackingActor, defendingActor);
            BattleUI.onSurrender -= Surrender;
            BattleUI.onSurrender += Surrender;
            BattleUI.onSkip -= Skip;
            BattleUI.onSkip += Skip;
       
            animationStateManager = new AnimationStateManager(attackingActor, defendingActor, battleSimulation, GetComponent<TimeLineController>(),GetComponent<CharacterCombatAnimations>());
            animationStateManager.OnCharacterAttack -= ShowActivatedAttackSkills;
            animationStateManager.OnCharacterAttack += ShowActivatedAttackSkills;
            animationStateManager.OnCharacterCrit -= CharacterCrit;
            animationStateManager.OnCharacterCrit += CharacterCrit;
            ShowActivatedCombatSkills((Unit)battleSimulation.Attacker, battleSimulation.AttackerActivatedCombatSkills, true);
            ShowActivatedCombatSkills((Unit)battleSimulation.Defender,battleSimulation.DefenderActivatedCombatSkills, false);
            animationStateManager.Start();
            animationStateManager.OnFinished -= Finished;
            animationStateManager.OnFinished += Finished;
       
        
            LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) => { volume.weight = value; });
        
        }

        void CharacterCrit(IBattleActor unit)
        {
            canvas.ShowCritical((Unit)unit);
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
            var uiSystem = ServiceProvider.Instance.GetSystem<UiSystem>();
            if (uiSystem != null)
            {
                uiSystem.ShowMainCanvas();
                uiSystem.charCircleCanvas.enabled = true;
            }
           
            //light.SetActive(false);
            // Debug.Log("Volume: "+volume.weight);
            // Debug.Log("GameObject: "+gameObject);
            LeanTween.value(volume.weight, 0, 0.4f).setEaseInQuad().setOnUpdate((value) => { volume.weight = value; })
                .setOnComplete(() => gameObject.SetActive(false));
        }
    }
}
