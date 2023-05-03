using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public BattleCanvasController canvas;
    
    private AnimationStateManager animationStateManager;
    public static event Action<BattleSimulation, IBattleActor, IAttackableTarget> OnShow;
    
    public Volume volume;
    public event Action OnFinished;
    

    public void Show(BattleSimulation battleSimulation, IBattleActor attackingActor, IAttackableTarget defendingActor)
    {
        gameObject.SetActive(true);
    
        canvas.Show();
        OnShow?.Invoke(battleSimulation, attackingActor, defendingActor);
       
        animationStateManager = new AnimationStateManager(attackingActor, defendingActor, battleSimulation, GetComponent<TimeLineController>(),GetComponent<CharacterCombatAnimations>());
        animationStateManager.Start();
        animationStateManager.OnFinished -= Finished;
        animationStateManager.OnFinished += Finished;
        
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) => { volume.weight = value; });
        
    }

    void Finished()
    {
        Cleanup();
        OnFinished?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&animationStateManager!=null)
        {
            CancelInvoke();//TODO DO THIS ON COROUTINE MONOBEHAVIOUR 
            Debug.Log("TODO Reset Cameras and Volumes!");
            animationStateManager.BattleFinished();
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