using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;

[CreateAssetMenu(menuName="GameData/Skills/SkillTree", fileName="SkillTree1")]
public class SkillTree :ScriptableObject
{
    [SerializeField]
    public List<SkillTreeEntry> skillsRow0;
    [SerializeField]
    public List<SkillTreeEntry> skillsRow1;
    [SerializeField]
    public List<SkillTreeEntry> skillsRow2;
    [SerializeField]
    public List<SkillTreeEntry> skillsRow3;
    [SerializeField]
    public List<SkillTreeEntry> skillsRow4;

}

[System.Serializable]
public class SkillTreeEntry
{
    public int levelRequirement;
   // public int[] statRequirements;
    //public Skill[] skillRequirements;
    public Skill skill;
}