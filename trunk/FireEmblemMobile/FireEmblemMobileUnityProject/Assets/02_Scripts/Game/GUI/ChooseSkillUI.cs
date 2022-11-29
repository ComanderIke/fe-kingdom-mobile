using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;
[System.Serializable]



public class ChooseSkillUI : MonoBehaviour
{
    public event Action onSkill1Chosen;
    public event Action onSkill2Chosen;
    public event Action onSkill3Chosen;
    [SerializeField] private Animator uIIdleAnimation;
    [SerializeField] private ChooseSkillButtonUI chooseSkill1;
    [SerializeField] private ChooseSkillButtonUI chooseSkill2;
    [SerializeField] private ChooseSkillButtonUI chooseSkill3;

    public void Show(Unit unit, Skill skill1, Skill skill2, Skill skill3)
    {
        uIIdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        chooseSkill1.SetSkill(skill1, unit);
        chooseSkill2.SetSkill(skill2, unit);
        chooseSkill3.SetSkill(skill3, unit);

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
