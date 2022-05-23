using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;

[CreateAssetMenu(menuName="GameData/Skills/SkillTree", fileName="SkillTree1")]
public class SkillTree :ScriptableObject
{
    [SerializeField]
    public List<SkillTreeEntry> skills;

}

[System.Serializable]
public class SkillTreeEntry
{
    public int levelRequirement;
    public int[] statRequirements;
    public Skill[] skillRequirements;
    public int depth;
    public Skill skill;
}