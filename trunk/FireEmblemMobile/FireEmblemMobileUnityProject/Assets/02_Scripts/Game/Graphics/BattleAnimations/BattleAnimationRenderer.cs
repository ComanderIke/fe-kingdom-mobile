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
    
    private bool playing;

    public void Show(BattleSimulation battleSimulation, IBattleActor attackingActor, IAttackableTarget defendingActor)
    {
        gameObject.SetActive(true);
        Debug.Log("SHow Battle Canvdass!");
        canvas.Show();
        OnShow?.Invoke(battleSimulation, attackingActor, defendingActor);
       
        animationStateManager = new AnimationStateManager(battleSimulation, GetComponent<TimeLineController>(),GetComponent<CharacterCombatAnimations>());
        animationStateManager.Start();
        animationStateManager.OnFinished -= Finished;
        animationStateManager.OnFinished += Finished;
        playing = true;
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) => { volume.weight = value; });
        
    }

    void Finished()
    {
        Debug.Log("BattleRenderer Finished");
        OnFinished?.Invoke();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&animationStateManager!=null)
        {
            CancelInvoke();//TODO DO THIS ON COROUTINE MONOBEHAVIOUR 
            Debug.Log("TODO Reset Cameras and Volumes!");
            animationStateManager.BattleFinished();
        }
    }
    public void Hide()
    {
        canvas.Hide();
        //light.SetActive(false);
        LeanTween.value(volume.weight, 0, 0.4f).setEaseInQuad().setOnUpdate((value) => { volume.weight = value; })
            .setOnComplete(() => gameObject.SetActive(false));
    }
}