using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillState{
Learned, Learnable, NotLearnable, Locked
}
public class SkillUI : MonoBehaviour
{
    public GameObject connectionPrefab;
    private List<SkillUI> children;
    private List<SkillUI> parents;
    private List<GameObject> connectionsToParents;
    public Image skillIcon;
    public Skill skill;
    public TextMeshProUGUI skillLevelText;

    public Image backGroundImage;
    public Color learned;
    public Color notLearned;
    public Color notLearnable;
    public Color locked;
    private SkillTreeRenderer controller;
    public void Setup(Skill skill, SkillState skillState, SkillTreeRenderer controller, List<SkillUI> parents=null)
    {
        this.controller = controller;
        this.skill = skill;
        this.parents = parents;
        connectionsToParents = new List<GameObject>();
        skillIcon.sprite = skill.Icon;
        skillLevelText.text = ""+skill.Level+"/"+skill.MaxLevel;
        switch (skillState)
        {
            case SkillState.Learnable:
                backGroundImage.color = notLearned; break;
            case SkillState.NotLearnable:  backGroundImage.color = notLearnable; break;
            case SkillState.Learned:  backGroundImage.color = learned; break;
            case SkillState.Locked:  backGroundImage.color = locked; break;
        }

        if (parents != null)
        {
            foreach (var parent in parents)
            {
                var connection = Instantiate(connectionPrefab, this.transform);

                connection.transform.localPosition = Vector3.zero;
                Vector3 relative = transform.InverseTransformPoint(parent.transform.position);
                Debug.Log("Self: "+this.gameObject.name+" Parent: " +parent.gameObject.name+" " +parent.transform.position+" "+this.transform.position);
                float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
                Debug.Log("Angle: " + angle);
                connection.transform.rotation = Quaternion.Euler(0, 0, -angle-90);
                Debug.Log("Distance: " + Vector2.Distance(parent.transform.position, this.transform.position));
                connection.GetComponent<RectTransform>().sizeDelta =
                    new Vector2((int)(Vector2.Distance(parent.transform.position, this.transform.position)), 100);
                connectionsToParents.Add(connection);
            }
        }


    }

    public void Clicked()
    {
        ToolTipSystem.ShowSkill(this, transform.position, skill.Name, skill.Description, skill.Icon);
        //controller.Clicked(this);
    }

    public void LearnClicked()
    {
        controller.LearnClicked(this);
    }
}