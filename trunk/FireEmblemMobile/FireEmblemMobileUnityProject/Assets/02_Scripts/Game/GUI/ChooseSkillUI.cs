using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.States;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;


public interface ISkillUIRenderer:IAnimation
{
    public void Show(Unit unit, Skill skill1, Skill skill2, Skill skill3);
    public void Hide();
    public Action OnFinished { get; set; }
}
public class ChooseSkillUI : MonoBehaviour, ISkillUIRenderer
{
    public event Action onSkill1Chosen;
    public event Action onSkill2Chosen;
    public event Action onSkill3Chosen;
    public Action OnFinished { get; set; }
    
    [SerializeField] private Animator uIIdleAnimation;
    [SerializeField] private ChooseSkillButtonUI chooseSkill1;
    [SerializeField] private ChooseSkillButtonUI chooseSkill2;
    [SerializeField] private ChooseSkillButtonUI chooseSkill3;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Canvas canvas;

    public void Show(Unit unit, Skill skill1, Skill skill2, Skill skill3)
    {
        canvas.enabled = true;
        uIIdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        chooseSkill1.SetSkill(skill1, unit);
        chooseSkill2.SetSkill(skill2, unit);
        chooseSkill3.SetSkill(skill3, unit);
        TweenUtility.FadeIn(canvasGroup);

    }

    public void Hide()
    {
       
        TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>
        {
            canvas.enabled = false;
            OnFinished?.Invoke();
        });
    }

   

    public void Skill1Clicked()
    {
        onSkill1Chosen?.Invoke();

    }

    public void Skill2Clicked()
    {
        onSkill2Chosen?.Invoke();
    }

    public void Skill3Clicked()
    {
        onSkill3Chosen?.Invoke();
    }
}
