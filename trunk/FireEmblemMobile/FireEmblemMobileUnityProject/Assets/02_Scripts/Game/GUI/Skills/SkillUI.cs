using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public GameObject connectionPrefab;
    private List<SkillUI> children;
    private List<SkillUI> parents;
    private List<GameObject> connectionsToParents;
    public Image skillIcon;
    public SkillTreeEntry skillEntry;
    public TextMeshProUGUI skillLevelText;

    public Image backGroundImage;
    public Color learned;
    public Color notLearned;
    public Color notLearnable;
    public Color locked;
    private SkillTreeRenderer controller;
    public SkillState skillState;
    public float offset = 30;

    public void Setup(SkillTreeEntry skill, SkillState skillState, SkillTreeRenderer controller, List<SkillUI> parents=null)
    {
        this.controller = controller;
        this.skillEntry = skill;
        this.parents = parents;
        connectionsToParents = new List<GameObject>();
        skillIcon.sprite = skill.skill.Icon;
        skillLevelText.text = ""+skill.skill.Level+"/"+skill.skill.MaxLevel;
        this.skillState = skillState;
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

                connection.transform.localPosition = new Vector3(0,offset,0);
                connection.transform.SetSiblingIndex(0);
                Vector3 relative = connection.transform.InverseTransformPoint(parent.transform.position-new Vector3(0,offset,0));
                //Debug.Log("Self: "+this.gameObject.name+" Parent: " +parent.gameObject.name+" " +parent.transform.position+" "+this.transform.position);
                float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
             //   Debug.Log("Angle: " + angle);
                connection.transform.rotation = Quaternion.Euler(0, 0, -angle);
              //  Debug.Log("Distance: " + Vector2.Distance(parent.transform.position, this.transform.position));
                connection.GetComponent<RectTransform>().sizeDelta =
                    new Vector2((int)(Vector2.Distance(parent.transform.position, connection.transform.position)-offset/2), 100);
                connectionsToParents.Add(connection);
                if(parent.skillState == SkillState.Learned&&skillState==SkillState.Learned)
                    connection.GetComponent<Image>().color = Color.white;
                else
                    connection.GetComponent<Image>().color = new Color(1,1,1, 0.2f);
            }
        }


    }

    public void Clicked()
    {
    
        Debug.Log("SkillUI Clicked");
        //ToolTipSystem.ShowSkill(this, transform.position, skill.Name, skill.Description, skill.Icon);
        controller.Clicked(this);
    }

    public void LearnClicked()
    {
       
        controller.LearnClicked(this);
    }

  
}