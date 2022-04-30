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
using Random = UnityEngine.Random;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public BattleCanvasController canvas;
    public PlayableDirector playableDirector;
    public event Action OnFinished;
    public Volume volume;
    private GameObject characterLeft;
    private GameObject characterRight;
    public new Camera camera;
    public GameObject battleBackground;
    public TimelineAsset cameraIntro;
    public TimelineAsset cameraIntroInverse;
    public TimelineAsset cameraZoomIn;
    public TimelineAsset cameraZoomOut;
    public Transform rightCharacterPosition;
    private bool playing;
    private BattleSimulation battleSimulation;
    public CameraShake cameraShake;
    public BattleUI BattleUI;
    private IBattleActor attacker;
    private IBattleActor defender;
    private bool leftCharacterDied;
    private bool rightCharacterDied;
    public new GameObject light;
    public float playSpeed = 1.0f;
    public float introWalkInPlaySpeed = 1.0f;
    public float timeBetweenAttacks = 1.0f;
    public float introWaitDuration = 1.0f;

    private bool leftCharacterAttacker = false;
    private int attackSequenzIndex = 0;

    private float attackDuration = 0.0f;
    public float magnitude = 0.4f;
    public float EndBattleWaitDuration = 0.5f;
    public float duration = 1.9f;

    void PlayAtSpeed(PlayableDirector playableDirector, float speed)
    {
        playableDirector.RebuildGraph(); // the graph must be created before getting the playable graph
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
        playableDirector.Play();
    }

    public void Show(BattleSimulation battleSimulation, IBattleActor attackingActor, IBattleActor defendingActor)
    {
        this.battleSimulation = battleSimulation;

        leftCharacterDied = false;
        rightCharacterDied = false;
        this.attacker = battleSimulation.Attacker;
        this.defender = battleSimulation.Defender;
        Debug.Log(attacker.Hp + " " + attackingActor.Hp);

        BattleUI.Show(battleSimulation, (Unit)attackingActor, (Unit)defendingActor);

        leftCharacterAttacker = battleSimulation.Attacker.Faction.IsPlayerControlled;
        if (characterLeft != null)
            Destroy(characterLeft);
        if (characterRight != null)
            Destroy(characterRight);
        attackSequenzIndex = 0;
        light.SetActive(true);
        if (battleSimulation.Attacker.Faction.IsPlayerControlled)
        {
            characterLeft = Instantiate(
                ((Unit)battleSimulation.Attacker).visuals.CharacterSpriteSet.battleAnimatedSprite,
                transform);
            characterRight = Instantiate(
                ((Unit)battleSimulation.Defender).visuals.CharacterSpriteSet.battleAnimatedSprite,
                transform);
        }
        else
        {
            characterLeft = Instantiate(
                ((Unit)battleSimulation.Defender).visuals.CharacterSpriteSet.battleAnimatedSprite,
                transform);
            characterRight = Instantiate(
                ((Unit)battleSimulation.Attacker).visuals.CharacterSpriteSet.battleAnimatedSprite,
                transform);
        }

        gameObject.SetActive(true);
        canvas.Show();
        playableDirector.Stop();

        if (battleSimulation.Attacker.Faction.IsPlayerControlled)
        {
            playableDirector.playableAsset = cameraIntro;
            camera.transform.localPosition =
                new Vector3(-80, camera.transform.localPosition.y, camera.transform.localPosition.z);
        }
        else
        {
            playableDirector.playableAsset = cameraIntroInverse;
            camera.transform.localPosition =
                new Vector3(80, camera.transform.localPosition.y, camera.transform.localPosition.z);
        }

        PlayAtSpeed(playableDirector, introWalkInPlaySpeed);

        var background = GameObject.Instantiate(battleBackground, transform);
        background.transform.position = new Vector3(camera.transform.position.x, background.transform.position.y,
            background.transform.position.z);
        if (battleSimulation.Attacker.Faction.IsPlayerControlled)
        {
            characterLeft.GetComponentInChildren<BattleAnimationSpriteController>().WalkIn(introWalkInPlaySpeed);
            characterRight.GetComponentInChildren<BattleAnimationSpriteController>().Idle(introWalkInPlaySpeed);
        }
        else
        {
            characterLeft.GetComponentInChildren<BattleAnimationSpriteController>().Idle(introWalkInPlaySpeed);
            characterRight.GetComponentInChildren<BattleAnimationSpriteController>().WalkIn(introWalkInPlaySpeed);
        }

        characterRight.transform.localScale = new Vector3(-characterRight.transform.localScale.x,
            characterRight.transform.localScale.y,
            characterRight.transform.localScale.z);
        //characterRight.transform.position = rightCharacterPosition.position;
        playing = true;
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) => { volume.weight = value; });
        Invoke("IntroFinished", (float)playableDirector.duration / introWalkInPlaySpeed + introWaitDuration);
    }


    //private float zoomInWaitDuration = 1.0f;
    private void IntroFinished()
    {
        playableDirector.Stop();
        playableDirector.playableAsset = cameraZoomIn;
        PlayAtSpeed(playableDirector, 1);

        Invoke("ZoomInFinished", (float)playableDirector.duration);
    }


    IEnumerator Delay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public float BattleTextMovementScale = 8f;
    public float DamageNumberHorizontalMinValue;
    public float DamageNumberHorizontalMaxValue;
    public float DamageNumberVerticalMinValue;
    public float DamageNumberVerticalMaxValue;

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

            attackerSpriteController.Attack(playSpeed);
            float textMoveDirection = leftCharacterAttacker ? 1 : -1;
            if (battleSimulation.AttacksData[attackSequenzIndex].hit)
            {
                BattleUI.UpdateDefenderHPBar(battleSimulation.AttacksData[attackSequenzIndex]);
                if (battleSimulation.AttacksData[attackSequenzIndex].kill)
                {
                    defenderSpriteController.Death(playSpeed);
                    if (leftCharacterAttacker)
                        rightCharacterDied = true;
                    else
                    {
                        leftCharacterDied = true;
                    }
                }
                else
                    defenderSpriteController.Damaged(playSpeed);
                TextStyle style = TextStyle.Damage;
                if (dmg == 0)
                    style = TextStyle.NoDamage;
                if(battleSimulation.AttacksData[attackSequenzIndex].crit)
                    style = TextStyle.Critical;
               
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateForBattleView(defenderImpactPosition.transform.position,
                        dmg, style, 5.0f, new Vector3(textMoveDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue)) *BattleTextMovementScale)));
            }
            else
            {
                defenderSpriteController.Dodge(playSpeed);
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateMiss(defenderImpactPosition.transform.position,
                       TextStyle.Missed, 4.0f, new Vector2(textMoveDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue)) * BattleTextMovementScale)));
            }

            attackDuration = (float)attackerSpriteController.GetAttackDuration();
        }
        else
        {
            var attackingCharacter = leftCharacterAttacker ? characterRight : characterLeft;
            var defendingCharacter = leftCharacterAttacker ? characterLeft : characterRight;
            var attackerSpriteController = attackingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderSpriteController = defendingCharacter.GetComponentInChildren<BattleAnimationSpriteController>();
            var defenderImpactPosition = defendingCharacter.GetComponentInChildren<ImpactPosition>();
            attackerSpriteController.Attack(playSpeed);
            float textMoveDirection = leftCharacterAttacker ? -1 : 1;
            if (battleSimulation.AttacksData[attackSequenzIndex].hit)
            {
                BattleUI.UpdateAttackerHPBar(battleSimulation.AttacksData[attackSequenzIndex]);
                if (battleSimulation.AttacksData[attackSequenzIndex].kill)
                {
                    defenderSpriteController.Death(playSpeed);
                    if (leftCharacterAttacker)
                        leftCharacterDied = true;
                    else
                    {
                        rightCharacterDied = true;
                    }
                }
                else
                    defenderSpriteController.Damaged(playSpeed);

                TextStyle style = TextStyle.Damage;
                if (dmg == 0)
                    style = TextStyle.NoDamage;
                if(battleSimulation.AttacksData[attackSequenzIndex].crit)
                    style = TextStyle.Critical;
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateForBattleView(defenderImpactPosition.transform.position,
                        dmg, style, 5.0f, new Vector2(textMoveDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue)) * BattleTextMovementScale)));
            }
            else
            {
                defenderSpriteController.Dodge(playSpeed);
                StartCoroutine(Delay(0.05f, () =>
                    DamagePopUp.CreateMiss(defenderImpactPosition.transform.position,
                        TextStyle.Missed, 4.0f, new Vector2(textMoveDirection*Random.Range(DamageNumberHorizontalMinValue,DamageNumberHorizontalMaxValue), Random.Range(DamageNumberVerticalMinValue,DamageNumberVerticalMaxValue)) * BattleTextMovementScale)));
            }

            attackDuration = (float)attackerSpriteController.GetAttackDuration();
        }


        attackSequenzIndex++;
        Invoke("FinishAttack", attackDuration / playSpeed);
    }


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
        PlayAtSpeed(playableDirector, 1);

        Debug.Log("ALive: " + attacker.IsAlive() + " " + defender.IsAlive());
        if (leftCharacterDied)
        {
            characterLeft.SetActive(false);
        }
        else
        {
            characterLeft.GetComponentInChildren<BattleAnimationSpriteController>().Idle(playSpeed);
        }

        if (rightCharacterDied)
        {
            characterRight.SetActive(false);
        }
        else
        {
            characterRight.GetComponentInChildren<BattleAnimationSpriteController>().Idle(playSpeed);
        }

        if (attackSequenzIndex >= battleSimulation.AttacksData.Count)
        {
            Invoke("ZoomOutFinished", 0);
        }
        else
        {
            Invoke("ZoomOutFinished", timeBetweenAttacks);
        }
    }


    private void AllAttacksFinished()
    {
        Invoke("BattleFinished", (float)(playableDirector.duration / playSpeed + EndBattleWaitDuration));

        // Invoke("ZoomOutFinished", (float)playableDirector.duration);
    }

    private void BattleFinished()
    {
        Debug.Log("BattleFINISHED!");
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
        PlayAtSpeed(playableDirector, 1);

        Invoke("ZoomInFinished", (float)playableDirector.duration);
        //OnFinished?.Invoke();
    }

    public void Hide()
    {
        canvas.Hide();
        light.SetActive(false);
        LeanTween.value(volume.weight, 0, 0.4f).setEaseInQuad().setOnUpdate((value) => { volume.weight = value; })
            .setOnComplete(() => gameObject.SetActive(false));
    }
}