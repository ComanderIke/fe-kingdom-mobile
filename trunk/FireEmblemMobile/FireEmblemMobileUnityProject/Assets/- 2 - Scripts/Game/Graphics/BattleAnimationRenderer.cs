using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.Playables;

public class BattleAnimationRenderer : MonoBehaviour, IBattleAnimation
{
    public Canvas canvas;
    public PlayableDirector timeline;
    public event Action OnFinished;
    

    public void Show()
    {
        gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
        timeline.Stop();
        timeline.Play();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    
}
