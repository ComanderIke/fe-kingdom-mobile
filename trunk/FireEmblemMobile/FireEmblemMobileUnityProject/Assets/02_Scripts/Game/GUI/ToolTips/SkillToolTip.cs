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

    [SerializeField] private Canvas canvas;
    [SerializeField] private ChooseSkillButtonUI chooseSkillButtonUI;
   
    [SerializeField] private Image curseIcon;
    [SerializeField] private Vector3 offset;
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

    void ClampOnScreen()
    {
        var canvasRect = canvas.transform as RectTransform;
        if (rectTransform.anchoredPosition.x +rectTransform.rect.width> canvasRect.rect.width)
        {
            rectTransform.anchoredPosition = new Vector2(canvasRect.rect.width - rectTransform.rect.width,rectTransform.anchoredPosition.y);
        }
        if (rectTransform.anchoredPosition.y +rectTransform.rect.height> canvasRect.rect.height)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, canvasRect.rect.height - rectTransform.rect.height);
        }
        if (rectTransform.anchoredPosition.x -rectTransform.pivot.x*rectTransform.rect.width< 0)
        {
            rectTransform.anchoredPosition = new Vector2( -rectTransform.pivot.x*rectTransform.rect.width,rectTransform.anchoredPosition.y);
        }
        if (rectTransform.anchoredPosition.y-rectTransform.pivot.y*rectTransform.rect.height< 0)
        {
            rectTransform.anchoredPosition = new Vector2( rectTransform.anchoredPosition.x,-rectTransform.pivot.y*rectTransform.rect.height);
        }
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(Skill skill, bool blessed, bool upgrade, Vector3 position)
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
       
        if (skill is Curse curse)
            curseIcon.sprite = curse.Icon;
        chooseSkillButtonUI.SetSkill(skill, blessed, upgrade);
        rectTransform.anchoredPosition=position+ offset;
        //UpdateTextWrap(position);
        ClampOnScreen();

    }
}