using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.PopUpText;
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
    public BattleUI BattleUI;
    public void Show(BattleSimulation battleSimulation, IBattleActor attacker, IBattleActor defender)
    {
        this.battleSimulation = battleSimulation;
        var background=GameObject.Instantiate(battleBackground, transform);
        background.transform.position = new Vector3(camera.transform.position.x, background.transform.position.y,
            background.transform.position.z);
    
        BattleUI.Show(battleSimulation, (Unit)attacker, (Unit)defender);
        leftCharacterAttacker = battleSimulation.Attacker.Faction.IsPlayerControlled;
        if(characterLeft!=null)
            Destroy(characterLeft);
        if(characterRight!=null)
            Destroy(characterRight);
        attackSequenzIndex = 0;
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
        characterLeft.GetComponentInChildren<BattleAnimationSpriteController>().WalkIn();
        characterRight.GetComponentInChildren<BattleAnimationSpriteController>().Idle();
        characterRight.transform.localScale = new Vector3(-characterRight.transform.localScale.x, characterRight.transform.localScale.y,
            characterRight.transform.localScale.z);
        //characterRight.transform.position = rightCharacterPosition.position;
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
  
    private float attackDuration = 0.0f;
    public float magnitude = 0.4f;

    public float duration = 1.9f;

   

    IEnumerator Delay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    private void ContinueBattle()
    {
       
        if (attackSequenzIndex >= battleSimulation.AttacksData.Count)
        {
            AllAttacksFinished();
            return;
        }
        StartCoroutine(cameraShake.Shake(duration, magnitude));
        var dmg = battleSimulation.AttacksData[attackSequenzIndex].Dmg;
    
        if (battleSimulation.AttacksData[attackSequenzIndex].attacker)
        {
          
            var attackingCharacter = leftCharacterAttacker ? characterLeft : characterRight;
            var defendingCharacter = leftCharacterAttacker ? characterRight : characterLeft;
            var attackerSpriteController = attackingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderSpriteController = defendingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderImpactPosition = defendingCharacter.GetComponentInChildren<ImpactPosition>();
           
            attackerSpriteController.Attack();
            if (battleSimulation.AttacksData[attackSequenzIndex].hit)
            {
                BattleUI.UpdateDefenderHPBar(battleSimulation.AttacksData[attackSequenzIndex]);
                defenderSpriteController.Damaged();
              
                StartCoroutine(Delay(0.05f,()=>
                    DamagePopUp.CreateForBattleView(defenderImpactPosition.transform.position,
                        dmg, Color.red, 5.0f,!leftCharacterAttacker)));

            }
            else
            {
                defenderSpriteController.Dodge();
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateMiss(defenderImpactPosition.transform.position,
                        Color.white, 4.0f, !leftCharacterAttacker)));
            }
            
            attackDuration= (float) attackerSpriteController.GetAttackDuration();
        }
        else
        {
           
            var attackingCharacter = leftCharacterAttacker ? characterRight : characterLeft;
            var defendingCharacter = leftCharacterAttacker ? characterLeft : characterRight;
            var attackerSpriteController = attackingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderSpriteController = defendingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderImpactPosition = defendingCharacter.GetComponentInChildren<ImpactPosition>();
            attackerSpriteController.Attack();
            if (battleSimulation.AttacksData[attackSequenzIndex].hit)
            {
                BattleUI.UpdateAttackerHPBar(battleSimulation.AttacksData[attackSequenzIndex]);
                defenderSpriteController.Damaged();
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateForBattleView(defenderImpactPosition.transform.position,
                        dmg, Color.red, 5.0f, leftCharacterAttacker)));
            }
            else
            {
                defenderSpriteController.Dodge();
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateMiss(defenderImpactPosition.transform.position,
                        Color.white, 4.0f, leftCharacterAttacker)));
            }
            attackDuration= (float) attackerSpriteController.GetAttackDuration();
        }

        
        attackSequenzIndex++;
        Invoke("FinishAttack", attackDuration);
      
    }

    private float timeBetweenAttacks = 1.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CancelInvoke();
            Debug.Log("TODO Reset Cameras and Volumes!");
            BattleFinished();
            
        }
    }

    private void FinishAttack()
    {
        playableDirector.playableAsset = cameraZoomOut;
        playableDirector.Play();
        characterLeft.GetComponentInChildren<BattleAnimationSpriteController>().Idle();
        characterRight.GetComponentInChildren<BattleAnimationSpriteController>().Idle();
        Invoke("ZoomOutFinished",timeBetweenAttacks );
    }

    private void AllAttacksFinished()
    {
         // playableDirector.playableAsset = cameraZoomOut;
         // playableDirector.Play();
         Invoke("BattleFinished",(float) (playableDirector.duration+0.5f) );
        
        // Invoke("ZoomOutFinished", (float)playableDirector.duration);
    }

    private void BattleFinished()
    {
        OnFinished?.Invoke();
    }
    private void ZoomInFinished()
    {
        playableDirector.Stop();
        ContinueBattle();
      
    }

    private void ZoomOutFinished()
    {
        if (attackSequenzIndex >= battleSimulation.AttacksData.Count)
        {
            AllAttacksFinished();
            return;
        }
        playableDirector.playableAsset = cameraZoomIn;
        playableDirector.Play();
       
        Invoke("ZoomInFinished", (float)playableDirector.duration);
        //OnFinished?.Invoke();
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
