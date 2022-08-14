using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeRenderer : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI specializationName;
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
        specializationName.text = skillTree.name;
        foreach (var row in rows)
        {
            row.transform.DeleteAllChildren();
        }

        skills = new List<SkillUI>();

        skillsPerRow = new[]
            { new List<SkillUI>(), new List<SkillUI>(), new List<SkillUI>(), new List<SkillUI>(), new List<SkillUI>() };

        foreach (var skillEntry in skillTree.skillEntries)
        {
            InitSkill(skillEntry,skillEntry.row);
        }

        this.CallWithDelay(() => SetupSkills(u), 0.05f);
    }

    void InitSkill(SkillTreeEntry skillEntry, int depth)
    {
        var go = Instantiate(skillPrefab, rows[depth]);
        go.name = skillEntry.skill.name;
        var skillUI = go.GetComponent<SkillUI>();
        skillsPerRow[depth].Add(skillUI);
        skills.Add(skillUI);
    }

    void SetupSkill(SkillTreeEntry skillEntry, Unit u, int cnt, int depth=0)
    {
        Skill skill = null;
        var skillUI = skills[cnt];
        SkillState state = SkillState.NotLearnable;
        
        if (u.SkillManager.Skills.Contains(skillEntry.skill))
        {
            skill = u.SkillManager.Skills.Find((skill) => skillEntry.skill.name == skill.name);
          
            state = SkillState.Learned;
            if (skill.Level == skill.MaxLevel)
                state = SkillState.Maxed;
        }
        if (u.ExperienceManager.Level >= skillEntry.levelRequirement)
            state = SkillState.Learnable;


        
        if (depth == 0)
            skillUI.Setup(skillEntry, state, this);
        else
            skillUI.Setup(skillEntry, state, this, skillsPerRow[depth - 1]);
    }
    void SetupSkills(Unit u)
        {
            Debug.Log("TODO SKILLSTATE CHECK REQUIREMENTS");
            int cnt = 0;

            foreach (var skillEntry in skillTree.skillEntries)
            {
                SetupSkill(skillEntry,u, cnt,skillEntry.row);
                cnt++;
            }
        }


        public void LearnClicked(SkillUI skillUI)
        {
            controller.LearnSkillClicked(skillUI);
        }

        public void Clicked(SkillUI skillUI)
        {
            controller.SkillSelected(skillUI);
           
        }
    }