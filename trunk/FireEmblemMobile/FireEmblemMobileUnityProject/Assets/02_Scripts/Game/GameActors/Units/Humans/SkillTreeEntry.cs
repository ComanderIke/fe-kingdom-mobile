using Game.GameActors.Units;
using Game.GameActors.Units.Skills;

using UnityEngine;

[System.Serializable]
public class SkillTreeEntry
{
    public int levelRequirement;

    public int row;
    // public int[] statRequirements;
    //public Skill[] skillRequirements;
    public Skill skill;
    public SkillState SkillState = SkillState.NotLearnable;
    public SkillTree tree;


    public void Init()
    {
        if(skill!=null)
            skill = ScriptableObject.Instantiate(skill);
    }
}