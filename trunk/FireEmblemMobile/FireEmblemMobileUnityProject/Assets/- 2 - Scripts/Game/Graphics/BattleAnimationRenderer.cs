using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public BattleCanvasController canvas;
    public PlayableDirector playableDirector;
    public event Action OnFinished;
    public Volume volume;
    private GameObject characterLeft;
    private GameObject characterRight;
    public Camera camera;
    public GameObject battleBackground;
    public TimelineAsset cameraIntro;
    public TimelineAsset cameraZoomIn;
    public TimelineAsset cameraZoomOut;
    public Transform rightCharacterPosition;
    private bool playing;
    private BattleSimulation battleSimulation;
    public CameraShake cameraShake;
    public void Show(BattleSimulation battleSimulation)
    {
        this.battleSimulation = battleSimulation;
        var background=GameObject.Instantiate(battleBackground, transform);
        background.transform.position = new Vector3(camera.transform.position.x, background.transform.position.y,
            background.transform.position.z);
        foreach (var paralaxController in battleBackground.GetComponentsInChildren<ParalaxController>())
        {
            paralaxController.camera = camera.transform;
        }

        leftCharacterAttacker = battleSimulation.Attacker.Faction.IsPlayerControlled;
        if (battleSimulation.Attacker.Faction.IsPlayerControlled)
        {
            characterLeft = Instantiate(((Unit) battleSimulation.Attacker).visuals.CharacterSpriteSet.battleAnimatedSprite,
                    transform);
            characterRight = Instantiate(((Unit) battleSimulation.Defender).visuals.CharacterSpriteSet.battleAnimatedSprite,
                    transform);
        }
        else
        {
            characterLeft = Instantiate(((Unit) battleSimulation.Defender).visuals.CharacterSpriteSet.battleAnimatedSprite,
                    transform);
            characterRight = Instantiate(((Unit) battleSimulation.Attacker).visuals.CharacterSpriteSet.battleAnimatedSprite,
                    transform);
        }
        gameObject.SetActive(true);
        canvas.Show();
        playableDirector.Stop();
        playableDirector.playableAsset = cameraIntro;
        playableDirector.Play();
        characterLeft.GetComponent<BattleAnimationSpriteController>().WalkIn();
        characterRight.transform.localScale = new Vector3(-characterRight.transform.localScale.x, characterRight.transform.localScale.y,
            characterRight.transform.localScale.z);
        characterRight.transform.position = rightCharacterPosition.position;
        playing = true;
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) =>
        {
            volume.weight = value;
        });
        Invoke("IntroFinished", (float)playableDirector.duration+introWaitDuration);
    }

    private float introWaitDuration = 1.0f;

    private bool leftCharacterAttacker = false;
    //private float zoomInWaitDuration = 1.0f;
    private void IntroFinished()
    {
        playableDirector.Stop();
        playableDirector.playableAsset = cameraZoomIn;
        playableDirector.Play();
       
        Invoke("ZoomInFinished", (float)playableDirector.duration);
    }

    private int attackSequenzIndex = 0;
  
    private float attackDuration = 1.0f;
    public float magnitude = 0.4f;

    public float duration = 1.9f;
    private void ContinueBattle()
    {
        StartCoroutine(cameraShake.Shake(duration, magnitude));
        if (attackSequenzIndex >= battleSimulation.AttackSequence.Count)
        {
            AllAttacksFinished();
            return;
        }
        if (battleSimulation.AttackSequence[attackSequenzIndex])
        {
            var attackingCharacter = leftCharacterAttacker ? characterLeft : characterRight;
            var defendingCharacter = leftCharacterAttacker ? characterRight : characterLeft;
            attackingCharacter.GetComponent<BattleAnimationSpriteController>().Attack(!leftCharacterAttacker);
            defendingCharacter.GetComponent<BattleAnimationSpriteController>().Dodge(leftCharacterAttacker);
        }
        else
        {
            var attackingCharacter = leftCharacterAttacker ? characterRight : characterLeft;
            var defendingCharacter = leftCharacterAttacker ? characterLeft : characterRight;
            attackingCharacter.GetComponent<BattleAnimationSpriteController>().Attack(leftCharacterAttacker);
            defendingCharacter.GetComponent<BattleAnimationSpriteController>().Dodge(!leftCharacterAttacker);
        }
        attackSequenzIndex++;
        Invoke("FinishAttack", attackDuration);
      
    }

    private float timeBetweenAttacks = 1.0f;
    private void FinishAttack()
    {

        Invoke("ContinueBattle",timeBetweenAttacks );
    }

    private void AllAttacksFinished()
    {
        playableDirector.playableAsset = cameraZoomOut;
        playableDirector.Play();
        Invoke("ZoomOutFinished", (float)playableDirector.duration);
    }
    private void ZoomInFinished()
    {
        playableDirector.Stop();
        ContinueBattle();
      
    }

    private void ZoomOutFinished()
    {
        OnFinished?.Invoke();
    }

    public void Hide()
    {
        canvas.Hide();
        LeanTween.value(volume.weight, 0, 0.4f).setEaseInQuad().setOnUpdate((value) =>
        {
            volume.weight = value;
        }).setOnComplete(()=> gameObject.SetActive(false));
       
        
    }

    
}
