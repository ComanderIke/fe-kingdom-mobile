using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using GameEngine;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public enum SkillRarity
{
    Common, Rare, Epic, Legendary, Mythic
}

public class SkillSystem : IEngineSystem
{
    private SkillGenerationConfiguration config;
    private ISkillUIRenderer renderer;

    public SkillSystem(SkillGenerationConfiguration config, ISkillUIRenderer renderer)
    {
        this.config = config;
        this.renderer = renderer;
    }
    public void Init()
    {
        

    }

    public void LearnNewSkill(Unit unit)
    {
        var skills = GenerateSkills(unit);
        renderer.OnFinished += FinishedAnimation;
        AnimationQueue.Add(()=>renderer.Show(unit, skills[0], skills[1], skills[2]));
    }
    public void LearnNewSkill(Unit unit, Skill skill1, Skill skill2, Skill skill3)
    {
        renderer.OnFinished += FinishedAnimation;
        AnimationQueue.Add(()=>renderer.Show(unit, skill1, skill2, skill3));
    }

    void FinishedAnimation()
    {
        renderer.OnFinished -= FinishedAnimation;
        AnimationQueue.OnAnimationEnded?.Invoke();
    }
    private List<Skill> GenerateSkills(Unit unit)
    {
        List<Skill> skills = new List<Skill>();
        while (skills.Count != 3)
        {
            var skill = GenerateSkill(unit);
            if (!skills.Contains(skill))
            {
                skills.Add(skill);
            }
        }

        return skills;
    }
    private Skill GenerateSkill(Unit unit)
    {
        var skillPool = new List<SkillBp>(config.CommonSkillPool);
        skillPool.AddRange(config.GetClassSkillPool(unit.rpgClass));
        int rng = Random.Range(0, skillPool.Count);
        var skill = skillPool[rng].Create();
        GenerateSkillRarity(skill);
        return skill;
    }
    private void GenerateSkillRarity(Skill skill)
    {
        float rng = Random.value;
        float epicChance = config.EpicChance;
        
        if (rng <= config.MythicChance)
        {
            skill.Tier = 0;
            skill.Level = 5;
        }
        else if (rng <= config.LegendaryChance)
        {
            skill.Tier = 1;
            skill.Level = 4;
        }
        else if (rng <= config.EpicChance)
        {
            skill.Tier = 2;
            skill.Level = 3;
        }
        else if (rng <= config.RareChance)
        {
            skill.Tier = 3;
            skill.Level = 2;
        }
        else
        {
            skill.Tier = 4;
            skill.Level = 1;
        }
    }

    public void Deactivate()
    {
        
    }

    public void Activate()
    {
        
    }
}