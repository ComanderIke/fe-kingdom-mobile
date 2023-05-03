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
    private List<SkillTreeEntryUI> skills;
    public Transform[] rows;
    private SkillTree skillTree;
    private List<SkillTreeEntryUI>[] skillsPerRow;
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

        skills = new List<SkillTreeEntryUI>();

        skillsPerRow = new[]
            { new List<SkillTreeEntryUI>(), new List<SkillTreeEntryUI>(), new List<SkillTreeEntryUI>(), new List<SkillTreeEntryUI>(), new List<SkillTreeEntryUI>() };

        foreach (var skillEntry in skillTree.skillEntries)
        {
            InitSkill(skillEntry,skillEntry.row);
        }

        this.CallWithDelay(() => SetupSkills(u), 0.05f);
    }

    void InitSkill(SkillTreeEntry skillEntry, int depth)
    {
        var go = Instantiate(skillPrefab, rows[depth]);
        go.name = skillEntry.Skill.Name;
        var skillUI = go.GetComponent<SkillTreeEntryUI>();
        skillsPerRow[depth].Add(skillUI);
        skills.Add(skillUI);
    }

    void SetupSkill(SkillTreeEntry skillEntry, Unit u, int cnt, int depth=0)
    {
        var skillUI = skills[cnt];
        
        if (depth == 0)
            skillUI.Setup(skillEntry,  this);
        else
            skillUI.Setup(skillEntry,  this, skillsPerRow[depth - 1]);
    }
    void SetupSkills(Unit u)
        {
            Debug.Log("SET UP SKILLS!!!!!!!!!!!!!!!!!!");
            int cnt = 0;

            foreach (var skillEntry in skillTree.skillEntries)
            {
                SetupSkill(skillEntry,u, cnt,skillEntry.row);
                cnt++;
            }
        }


        public void LearnClicked(SkillTreeEntryUI skillTreeEntryUI)
        {
            controller.LearnSkillClicked(skillTreeEntryUI);
        }

        public void Clicked(SkillTreeEntryUI skillTreeEntryUI)
        {
            controller.SkillSelected(skillTreeEntryUI);
           
        }
    }