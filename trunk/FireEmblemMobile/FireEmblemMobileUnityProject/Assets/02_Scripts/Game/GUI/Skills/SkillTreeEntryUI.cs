using System.Collections.Generic;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Skills
{
    public class SkillTreeEntryUI : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public GameObject connectionPrefab;
        private List<SkillTreeEntryUI> children;
        private List<SkillTreeEntryUI> parents;
        private Dictionary<SkillTreeEntryUI,GameObject> connectionsToParents;
        public Image skillIcon;
        public SkillTreeEntry skillEntry;
        public TextMeshProUGUI skillLevelText;
        public float secretAlpha = .5f;
        public Image backGroundImage;
        public Color learned;
        public Color notLearned;
        public Color notLearnable;
        public Color locked;
        public Color secretIconColor;
        public Color normalIconColor;
        public Color maxTextColor;
        public Color normalTextColor;
        private SkillTreeRenderer controller;
        public float offset = 30;

        public void Setup(SkillTreeEntry skill,  SkillTreeRenderer controller, List<SkillTreeEntryUI> parents=null)
        {
            this.controller = controller;
            this.skillEntry = skill;
            this.parents = parents;
            connectionsToParents = new Dictionary<SkillTreeEntryUI, GameObject>();
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
                    connectionsToParents.Add(parent, connection);
                }
       
       
            }
            UpdateUI();


        }

        public void UpdateUI()
        {
            Debug.Log("UPDATE SKILL BUTTON UI!!!!!! "+skillEntry.SkillState);
            skillIcon.sprite = skillEntry.Skill.Icon;
            skillLevelText.text = ""+skillEntry.Skill.Level;

            skillLevelText.color = normalTextColor;
            skillIcon.color = normalIconColor;
            CanvasGroup.alpha  = 1;
            switch (skillEntry.SkillState)
            {
                case SkillState.Learnable:
                    backGroundImage.color = notLearned; break;
                case SkillState.NotLearnable:  backGroundImage.color = locked;
                    skillIcon.color = secretIconColor;
                    CanvasGroup.alpha = secretAlpha; break;
                case SkillState.Learned:  backGroundImage.color = learned; break;
                case SkillState.Maxed:  backGroundImage.color = learned;
                    skillLevelText.color = maxTextColor; break;
                case SkillState.Locked:  backGroundImage.color = locked;
                    skillIcon.color = secretIconColor;
                    CanvasGroup.alpha = secretAlpha;
                    break;
            }

            foreach (var keyvaluePair in connectionsToParents)
            {
                if (keyvaluePair.Key.skillEntry.SkillState == SkillState.Learned && skillEntry.SkillState == SkillState.Learned)
                    keyvaluePair.Value.GetComponent<Image>().color = Color.white;
                else
                    keyvaluePair.Value.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
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
            UpdateUI();
        }

  
    }
}