using Game.GameActors.Units.Skills;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    // public TextMeshProUGUI headerText;
    // public TextMeshProUGUI descriptionText;
    // public Image skillIcon;

    [SerializeField] private ChooseSkillButtonUI chooseSkillButtonUI;

    private Skill skill;
    [SerializeField]
    private RectTransform rectTransform;

    // Start is called before the first frame update
    

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateTextWrap(transform.position);
        }
    }
    void UpdateTextWrap(Vector3 position)
    {

       // transform.position = position+ new Vector3(0,100,0);
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(Skill skill, bool blessed, Vector3 position)
    {
        this.skill = skill;
        // if (string.IsNullOrEmpty(header))
        // {
        //     headerText.gameObject.SetActive(false);
        // }
        // else
        // {
        //     headerText.gameObject.SetActive(true);
        //     headerText.text = header;
        // }
        //
        // descriptionText.text = description;
        // skillIcon.sprite = icon;
        chooseSkillButtonUI.SetSkill(skill, blessed);
        rectTransform.anchoredPosition=position+ new Vector3(0,080+((chooseSkillButtonUI.transform as RectTransform).rect.height/2),0);
        UpdateTextWrap(position);

    }
}