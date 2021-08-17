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

    private bool playing;
    public void Show(BattleSimulation battleSimulation)
    {
        var background=GameObject.Instantiate(battleBackground, transform);
        background.transform.position = new Vector3(camera.transform.position.x, background.transform.position.y,
            background.transform.position.z);
        foreach (var paralaxController in battleBackground.GetComponentsInChildren<ParalaxController>())
        {
            paralaxController.camera = camera.transform;
        }
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
        playing = true;
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) =>
        {
            volume.weight = value;
        });
        Invoke("IntroFinished", (float)playableDirector.duration);
    }

    private void IntroFinished()
    {
        playableDirector.Stop();
        playableDirector.playableAsset = cameraZoomIn;
        playableDirector.Play();
        Invoke("ZoomInFinished", (float)playableDirector.duration);
    }
    private void ZoomInFinished()
    {
        playableDirector.Stop();
        playableDirector.playableAsset = cameraZoomOut;
        playableDirector.Play();
        Invoke("ZoomOutFinished", (float)playableDirector.duration);
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
