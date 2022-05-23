using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeRenderer : MonoBehaviour
{
    // Start is called before the first frame update
     
    public Image skillPrefab;
    private List<SkillUI> skills;
    public Transform[] rows;
    private SkillTree skillTree;
    private List<SkillUI>[] skillsPerRow;
    private SkillTreeUI controller;

    public void Show(SkillTree skillTree, Unit u, SkillTreeUI controller)
    {
        this.skillTree = skillTree;
        this.controller = controller;
        foreach (var row in rows)
        {
            row.transform.DeleteAllChildren();
        }
        skills = new List<SkillUI>();
        
        skillsPerRow = new [] { new List<SkillUI>(),new List<SkillUI>(),new List<SkillUI>(),new List<SkillUI>(),new List<SkillUI>()};
        
        foreach (var skillEntry in skillTree.skills)
        {
            var go=Instantiate(skillPrefab, rows[skillEntry.depth]);
            go.name=skillEntry.skill.name;
            var skillUI=go.GetComponent<SkillUI>();
            skillsPerRow[skillEntry.depth].Add(skillUI);
            skills.Add(skillUI);
           
        }
        this.CallWithDelay(()=>SetupSkills(u),0.05f);
    }

    void SetupSkills(Unit u)
    {
        Debug.Log("TODO SKILLSTATE CHECK REQUIREMENTS");
        int cnt = 0;
        foreach (var skillEntry in skillTree.skills)
        {
            var skillUI = skills[cnt];
            SkillState state = SkillState.NotLearnable;
            if (u.ExperienceManager.Level >= skillEntry.levelRequirement)
                state = SkillState.Learnable;
            if (u.SkillManager.Skills.Contains(skillEntry.skill))
                state = SkillState.Learned;
            if (skillEntry.depth == 0)
                skillUI.Setup(skillEntry.skill, state, this);
            else
                skillUI.Setup(skillEntry.skill, state, this, skillsPerRow[skillEntry.depth - 1]);
            cnt++;
        }
    }

   


    public void LearnClicked(SkillUI skillUI)
    {
        controller.LearnSkillClicked(skillUI);
    }
}
