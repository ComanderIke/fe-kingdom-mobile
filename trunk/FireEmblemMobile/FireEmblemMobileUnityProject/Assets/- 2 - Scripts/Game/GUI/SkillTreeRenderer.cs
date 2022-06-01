using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
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

        foreach (var skillEntry in skillTree.skillsRow0)
        {
            InitSkill(skillEntry,0);
        }
        foreach (var skillEntry in skillTree.skillsRow1)
        {
            InitSkill(skillEntry,1);
        }
        foreach (var skillEntry in skillTree.skillsRow2)
        {
            InitSkill(skillEntry,2);
        }
        foreach (var skillEntry in skillTree.skillsRow3)
        {
            InitSkill(skillEntry,3);
        }
        foreach (var skillEntry in skillTree.skillsRow4)
        {
            InitSkill(skillEntry,4);
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
        var skillUI = skills[cnt];
        SkillState state = SkillState.NotLearnable;
        if (u.ExperienceManager.Level >= skillEntry.levelRequirement)
            state = SkillState.Learnable;
        if (u.SkillManager.Skills.Contains(skillEntry.skill))
            state = SkillState.Learned;
        if (depth == 0)
            skillUI.Setup(skillEntry.skill, state, this);
        else
            skillUI.Setup(skillEntry.skill, state, this, skillsPerRow[depth - 1]);
    }
    void SetupSkills(Unit u)
        {
            Debug.Log("TODO SKILLSTATE CHECK REQUIREMENTS");
            int cnt = 0;
            foreach (var skillEntry in skillTree.skillsRow0)
            {
                SetupSkill(skillEntry,u, cnt,0);
                cnt++;
            }
            foreach (var skillEntry in skillTree.skillsRow1)
            {
                SetupSkill(skillEntry,u, cnt,1);
                cnt++;
            }
            foreach (var skillEntry in skillTree.skillsRow2)
            {
                SetupSkill(skillEntry,u, cnt,2);
                cnt++;
            }
            foreach (var skillEntry in skillTree.skillsRow3)
            {
                SetupSkill(skillEntry,u, cnt,3);
                cnt++;
            }
            foreach (var skillEntry in skillTree.skillsRow4)
            {
                SetupSkill(skillEntry,u, cnt,4);
                cnt++;
            }
        }


        public void LearnClicked(SkillUI skillUI)
        {
            controller.LearnSkillClicked(skillUI);
        }
    }