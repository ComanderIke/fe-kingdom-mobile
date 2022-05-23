using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI descriptionText;

   
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public Image skillIcon;

    public Button learnButton;
    public TextMeshProUGUI useButtonText;
    private SkillUI skill;
    private RectTransform rectTransform;

    public LayoutElement frame;
    // Start is called before the first frame update
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateTextWrap(transform.position);
        }
    }

    public void LearnClicked()
    {
        Debug.Log("LearnSkillClicked");
        skill.LearnClicked();
        gameObject.SetActive(false);
    }
    void UpdateTextWrap(Vector3 position)
    {
        frame.enabled = false;
        frame.enabled = true;
        if(rectTransform==null)
            rectTransform = GetComponent<RectTransform>();
        int headerLength = headerText.text.Length;
        int contentLength = descriptionText.text.Length;
        layoutElement.enabled =
            (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;

        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }

    public void ExitClicked()
    {
        gameObject.SetActive(false);
    }
    public void SetValues(SkillUI skill, string header, string description, Sprite icon, Vector3 position)
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

        // if (skill is EquipableItem eitem)
        // {
        //     Human human = (Human)Player.Instance.Party.ActiveUnit;
        //     if (human.CanEquip(eitem))
        //     {
        //         learnButton.interactable = true;
        //     }
        //     else
        //     {
        //         learnButton.interactable = false;
        //     }
        // }
        // else
        // {
        //     learnButton.interactable = true;
        // }

        // useButton.interactable = 
        useButtonText.text = "Learn";
       
        descriptionText.text = description;
        skillIcon.sprite = icon;
        
        UpdateTextWrap(position);

    }
}