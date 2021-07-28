using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public BattleCanvasController canvas;
    public PlayableDirector timeline;
    public event Action OnFinished;
    public Volume volume;
    

    public void Show()
    {
        gameObject.SetActive(true);
        canvas.Show();
        timeline.Stop();
        timeline.Play();
        LeanTween.value(volume.weight, 1, 1.2f).setEaseOutQuad().setOnUpdate((value) =>
        {
            volume.weight = value;
        });
        Invoke("TimelineFinished", (float)timeline.duration);
    }

    private void TimelineFinished()
    {
        OnFinished?.Invoke();
    }

    public void Update()
    {
       
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
