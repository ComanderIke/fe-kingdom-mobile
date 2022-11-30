using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using GameEngine;
using Random = UnityEngine.Random;

public enum SkillRarity
{
    Common, Rare, Epic, Legendary, Mythic
}

public class SkillSystem : IEngineSystem
{
    private SkillGenerationConfiguration config;

    public SkillSystem(SkillGenerationConfiguration config)
    {
        this.config = config;
    }
    public void Init()
    {
        
    }

    public List<Skill> GenerateSkills(Unit unit)
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
    public Skill GenerateSkill(Unit unit)
    {
        var skillPool = config.CommonSkillPool;
        skillPool.AddRange(config.GetClassSkillPool(unit.rpgClass));
        int rng = Random.Range(0, skillPool.Count);
        var skill = skillPool[rng].Create();
        GenerateSkillRarity(skill);
        return skill;
    }
    public void GenerateSkillRarity(Skill skill)
    {
        float rng = Random.value;
        float epicChance = config.EpicChance;
        if (rng <= config.MythicChance)
        {
            skill.Level = 5;
        }
        else if (rng <= config.LegendaryChance)
        {
            skill.Level = 4;
        }
        else if (rng <= config.EpicChance)
        {
            skill.Level = 3;
        }
        else if (rng <= config.RareChance)
        {
            skill.Level = 2;
        }
        else
        {
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