using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;
    public Image skillIcon;


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
    public void SetValues(Skill skill, string header, string description, Sprite icon, Vector3 position)
    {
        this.skill = skill;
        if (string.IsNullOrEmpty(header))
        {
            headerText.gameObject.SetActive(false);
        }
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }

        descriptionText.text = description;
        skillIcon.sprite = icon;
        rectTransform.anchoredPosition=position+ new Vector3(0,150,0);
        UpdateTextWrap(position);

    }
}