using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Assets.Scripts.Characters.Attributes;

public class LevelUpDialogScript : MonoBehaviour
{

    private enum LevelUpDialogState
    {
        inactive,
        start,
        skill,
        attribute
    }
    public enum LevelUpButtonType
    {
        attributeButton,
        dialogbutton,
        skillbutton
    }
    private const float angle = 60;
    public Text attributePointText;
    Button[] attributebuttons;
    Button[] skillbuttons;
    private int selectedSkill = 0;
    private LevelUpButtonType activeButtonType = LevelUpButtonType.attributeButton;
    private LevelUpDialogState state = LevelUpDialogState.inactive;
    public Text hpValue;
    public Text strValue;
    public Text agiValue;
    public GameObject startGameObject;
    public GameObject skillGameObject;
    public GameObject attributeGameObject;
    public Text defValue;
    public Text crtValue;
    private int attributePointsLeft = 5;
    private Character character;
    Stats oldstats;
    public bool active = false;
    private int activeAttributeButton = 0;

    void Start() { }

    public void show(Character c)
    {
        MyInputManager.fixedPosition = true;
        MyInputManager.isLevelUpState = true;
        MyInputManager.levelUpCharacter = c;
        character = c;
        oldstats = c.charclass.stats;
        state = LevelUpDialogState.start;
        GetComponent<AudioSource>().Play();
        showBool = true;
    }

    public void OnFinishedClicked()
    {
        MyInputManager.isLevelUpState = false;
        MyInputManager.levelUpCharacter = null;
        //transform.parent.position = new Vector3(-800, 0, 0);
        skillGameObject.SetActive(false);
        attributeGameObject.SetActive(false);
        startGameObject.GetComponentInChildren<Image>().enabled = false;
        startGameObject.GetComponentInChildren<Text>().enabled = false;
        active = false;
        MyInputManager.fixedPosition = false;
    }

    public void OnSkipClicked()
    {
        attributePointsLeft = 5;
        character.charclass.stats = oldstats;
    }
    public void OnSkill1Clicked()
    {
        character.charclass.skills[0].Level++;
    }
    public void OnSkill2Clicked()
    {
        character.charclass.skills[1].Level++;
    }
    public void OnAttributeHPClicked()
    {
        if (attributePointsLeft > 0)
        {
            character.stats.maxHP += 1;
            attributePointsLeft -= 1;
        }

    }
    public void OnAttributeSTRClicked()
    {
        if (attributePointsLeft > 0)
        {
            character.stats.attack += 1;
            attributePointsLeft -= 1;
        }
    }
    public void OnAttributeAGIClicked()
    {
        if (attributePointsLeft > 0)
        {
            character.stats.speed += 1;
            attributePointsLeft -= 1;
        }
    }
    public void OnAttributeDEFClicked()
    {
        if (attributePointsLeft > 0)
        {
            character.stats.defense += 1;
            attributePointsLeft -= 1;
        }
    }
    public void OnAttributeCritClicked()
    {
        if (attributePointsLeft > 0)
        {
            character.stats.accuracy += 1;
            attributePointsLeft -= 1;
        }
    }

    public int ActiveAttributeButton
    {
        get
        {
            return activeAttributeButton;
        }
        set
        {
            if (value < 0)
            {
                activeAttributeButton = 5;
            }
            else if (value > 5)
            {
                activeAttributeButton = 0;
            }
            else
                activeAttributeButton = value;
            attributebuttons[activeAttributeButton].Select();

        }
    }
    bool showBool = false;
    float showTime = 0;
    private void SelectSkill(int s)
    {
        skillGameObject.SetActive(false);
        attributeGameObject.SetActive(true);
        attributebuttons = attributeGameObject.GetComponentsInChildren<Button>();
        character.charclass.skills[s].Level++;
        state = LevelUpDialogState.attribute;
    }
    void Update()
    {
        if (showBool == true)
        {
            showTime += Time.deltaTime;
            

        }
        if(showTime>= 1.5f)
        {
            showTime = 0;
            showBool = false;
            active = true;
            attributePointsLeft = 5;
            skillGameObject.SetActive(false);
            attributeGameObject.SetActive(false);
            startGameObject.GetComponentInChildren<Image>().enabled = true;
            startGameObject.GetComponentInChildren<Text>().enabled = true;
        }
        if (active)
        {
            #region start
            if (state == LevelUpDialogState.start)
            {
                //if (MyInputManager.GAMEPAD_INPUT)
                //{
                if (MyInputManager.IsAButtonDown())
                {
                    state = LevelUpDialogState.skill;

                    startGameObject.GetComponent<Image>().enabled = false;
                    startGameObject.GetComponentInChildren<Text>().enabled = false;
                    skillGameObject.SetActive(true);
                    skillbuttons = skillGameObject.GetComponentsInChildren<Button>();
                    int cnt = 0;
                    foreach (Button b in skillbuttons)
                    {
                        b.image.sprite = character.charclass.skills[cnt].sprite;
                        cnt++;
                        b.GetComponentInChildren<Text>().text = "";
                    }
                }
            }
            #endregion

            #region skill
            else if (state == LevelUpDialogState.skill)
            {
                if (MyInputManager.IsAButtonDown())
                {
                    SelectSkill(selectedSkill);
                }
                float myangle = MyInputManager.GetAngle();
                if (myangle >= 0 && myangle < 180)
                {
                    //skillGameObject.GetComponentsInChildren<Image>()[4].color = Color.blue;
                    //skillGameObject.GetComponentsInChildren<Image>()[3].color = Color.white;
                    selectedSkill = 1;
                    skillbuttons[1].image.sprite = character.charclass.skills[1].sprite_hovered;
                    skillbuttons[0].image.sprite = character.charclass.skills[0].sprite;
                }
                else
                {
                    //skillGameObject.GetComponentsInChildren<Image>()[3].color = Color.blue;
                    //skillGameObject.GetComponentsInChildren<Image>()[4].color = Color.white;
                    selectedSkill = 0;
                    skillbuttons[1].image.sprite = character.charclass.skills[1].sprite;
                    skillbuttons[0].image.sprite = character.charclass.skills[0].sprite_hovered;
                }
            }
            #endregion

            #region attribute
            else if (state == LevelUpDialogState.attribute)
            {
                attributePointText.text = "" + attributePointsLeft;
                hpValue.text = "" + character.stats.maxHP;
                strValue.text = "" + character.stats.attack;
                agiValue.text = "" + character.stats.speed;
                defValue.text = "" + character.stats.defense;
                crtValue.text = "" + character.stats.accuracy;
                if (MyInputManager.IsAButtonDown())
                {
                    if (activeButtonType == LevelUpButtonType.attributeButton)
                    {
                        var pointer = new PointerEventData(EventSystem.current);
                        ExecuteEvents.Execute(attributebuttons[activeAttributeButton].gameObject, pointer, ExecuteEvents.submitHandler);
                    }

                }
                else if (MyInputManager.IsBButtonDown())
                {
                    attributePointsLeft = 5;
                    character.stats = oldstats;
                }

                float myangle = MyInputManager.GetAngle();
                if (myangle >= 360 - angle / 2 || myangle <= angle / 2)
                {
                    ActiveAttributeButton = 2;
                }
                else if (myangle > angle / 2 && myangle < angle / 2 + angle)
                {
                    ActiveAttributeButton = 3;
                }
                else if (myangle > angle / 2 + angle && myangle < angle / 2 + 2 * angle)
                {
                    ActiveAttributeButton = 4;
                }
                else if (myangle > angle / 2 + 2 * angle && myangle < angle / 2 + 3 * angle)
                {
                    ActiveAttributeButton = 5;
                }
                else if (myangle > angle / 2 + 3 * angle && myangle < angle / 2 + 4 * angle)
                {
                    ActiveAttributeButton = 0;
                }
                else if (myangle > angle / 2 + 4 * angle && myangle < angle / 2 + 5 * angle)
                {
                    ActiveAttributeButton = 1;
                }
            }
            #endregion
        }
    }
}

