using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using TMPro;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public SkillTreeRenderer[] skillTreeRenderer;
    public TextMeshProUGUI skillPoints;
    public Canvas canvas;
    private Unit u;
    public void Show(Unit u)
    {
        this.u = u;
        int cnt = 0;
        skillPoints.SetText(""+u.SkillManager.SkillPoints);
        foreach (var skillTree in u.SkillManager.SkillTrees)
        {
            skillTreeRenderer[cnt].Show(skillTree, u, this);
            cnt++;
        }

        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    public void LearnSkillClicked(SkillUI clickedSkill)
    {
        u.SkillManager.LearnSkill(clickedSkill.skill);
        Show(u);
    }
}
