using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using LostGrace;
using TMPro;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public SkillTreeRenderer[] skillTreeRenderer;
    public TextMeshProUGUI skillPoints;
    public Canvas canvas;
    private Unit u;
    [SerializeField] private SkillUpgradeDetailPanel detailPanel;
    [SerializeField] private SkillSelectCursor cursor;
    public void Show(Unit u)
    {
        UpdateUI(u);

        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    void UpdateUI(Unit u)
    {
        this.u = u;
        skillPoints.SetText(""+u.SkillManager.SkillPoints);
      
    }
    public void LearnSkillClicked(SkillTreeEntryUI clickedSkillTreeEntry)
    {
        u.SkillManager.LearnSkillEntry((clickedSkillTreeEntry.skillEntry));
        UpdateUI(u);
    }

    public void SkillSelected(SkillTreeEntryUI skillTreeEntryUI)
    {
        Debug.Log("Show Cursor and DetailPanel");
        cursor.Show(skillTreeEntryUI.gameObject);
        detailPanel.Show(skillTreeEntryUI, u);
    }
}
