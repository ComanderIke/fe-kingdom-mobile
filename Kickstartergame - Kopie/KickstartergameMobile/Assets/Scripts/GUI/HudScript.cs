using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Characters.Skills;
using Assets.Scripts.GameStates;
using System.Collections.Generic;
using System;
using Assets.Scripts.Characters.Classes;

public class HudScript : MonoBehaviour {

	bool hide = false;
    Button[] buttons;
    MainScript mainScript;
	[HideInInspector]
    private Character activeCharacter;
    Image[] images;
    Text[] texts;
    Image sprite;
    Image healthbar;
	public Sprite backGround;
	public Sprite backGroundMage;
	public Sprite skillSprite;
	public Sprite skillSprite_hovered;
	public Sprite skillSprite_pressed;
	public Sprite skillSprite_available;
	public Sprite skillSprite_available_hovered;
	public Sprite skillSprite_available_pressed;
	public Sprite skillSprite_disabled;
	public Sprite healthbarSprite;
	public Sprite healthbarSprite_disabled;


	[HideInInspector]
	public Button undoButton;
	Image expbar;
    private float currentHP;
	private float currentExp;
    float healthSpeed = 2;
    Text[] attributes;
	GameObject hudSwitch;
	Button[] inventoryButtons;
	Button endTurnButton;
	Image backGroundImage;
    public void Show()
    {
        hudSwitch.SetActive(true);
        hide = false;
    }
    public void Hide()
    {
       hudSwitch.SetActive(false);
       hide = true;
    }
    void Start()
    {


		endTurnButton = GameObject.Find ("EndTurnButton").GetComponent<Button> ();
        sprite=GameObject.Find("ActiveCharSprite").GetComponent<Image>();
		backGroundImage = GameObject.Find ("backGroundImage").GetComponent<Image> ();
		healthbar = GameObject.Find("ActiveCharHP").GetComponent<Image>();
		expbar = GameObject.Find("ActiveCharExp").GetComponent<Image>();
		undoButton = GameObject.Find ("BackButton").GetComponent<Button> ();
        attributes = GameObject.Find("AttributeButtons").GetComponentsInChildren<Text>();
		inventoryButtons = GameObject.Find("Inventory").GetComponentsInChildren<Button>();
        buttons = GameObject.Find("SkillButtons").GetComponentsInChildren<Button>();
        images = GetComponentsInChildren<Image>();
		hudSwitch = GameObject.Find ("HudSwitch");
        texts = GetComponentsInChildren<Text>();
       
        mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
    }
	static bool first_time_end=true;
    // Update is called once per frame
    Text levelText;
    Text charNameText;
    private void OnEnable()
    {

        levelText= GameObject.Find("Level").GetComponent<Text>();
        charNameText= GameObject.Find("CharacterName").GetComponent<Text>();
    }
    void Update()
    {
		if (hide) {
			return;
		} 
        int cnt = 0;
        if (mainScript.clickedCharacter != null)
        {
            activeCharacter = mainScript.clickedCharacter;
        }
        if (activeCharacter != null && Time.frameCount % 5 == 0 && activeCharacter.name != "")
        {
			if (activeCharacter.characterClassType == CharacterClassType.Mage || activeCharacter.characterClassType == CharacterClassType.Priest) {
				backGroundImage.sprite = backGroundMage;
			} else {
				backGroundImage.sprite = backGround;
			}
            levelText.text = String.Empty + activeCharacter.level;
            charNameText.text = activeCharacter.name;
            attributes[0].text = String.Empty + activeCharacter.stats.attack;
            attributes[1].text = String.Empty + activeCharacter.stats.defense;
            attributes[2].text = String.Empty + activeCharacter.stats.speed;
            attributes[3].text = String.Empty + activeCharacter.stats.accuracy;
            attributes[4].text = String.Empty + activeCharacter.stats.spirit;
            cnt = 0;
			/*foreach (Skill s in activeCharacter.charclass.skills) {
					buttons [cnt].enabled = true;
					buttons [cnt].GetComponent<Image> ().enabled = true;
					SpriteState ss = new SpriteState ();
					ss.disabledSprite = s.sprite_disabled;
					ss.highlightedSprite = s.sprite_hovered;
					ss.pressedSprite = s.sprite_hovered;
					buttons [cnt].spriteState = ss;
					buttons [cnt].image.sprite = s.sprite;
					string manacost = s.manacost == 0 ? "":"-"+s.manacost;
					if (buttons [cnt].GetComponentsInChildren<Text> ().Length >= 2)
					buttons [cnt].GetComponentsInChildren<Text> () [1].text = manacost;
					if (buttons [cnt].GetComponentsInChildren<Text> ().Length >= 3)
						buttons [cnt].GetComponentsInChildren<Text> () [2].text = ""+s.Level;
					if (mainScript.activeCharacter != activeCharacter
					                || (s is PassiveSkill && !(s is BasicAttack))
					                || mainScript.activeCharacter == null ||
					                ((!(mainScript.gameState is GameplayState) ))) {
						Text text = buttons [cnt].GetComponentInChildren<Text> ();
						if (text != null)
							text.text = "";
						buttons [cnt].image.sprite = s.sprite;
						buttons [cnt].interactable = false;
					} else {
						if (s.CurrentCooldown != 0) {
							Text text = buttons [cnt].GetComponentInChildren<Text> ();
							if (text != null)
								text.text = ""+s.CurrentCooldown;

							buttons [cnt].interactable = false;
						} else if (mainScript.HasSkillTargets (s)) {
							Text text = buttons [cnt].GetComponentInChildren<Text> ();
							if (text != null)
								text.text = "";
							buttons [cnt].interactable = true;
						} else {
							Text text = buttons [cnt].GetComponentInChildren<Text> ();
							if (text != null)
								text.text = "";
							buttons [cnt].interactable = false;
						}
					}

					cnt++;
				}*/
				foreach (Image i in images) {
					i.enabled = true;
				}

				cnt = 0;
				foreach (Button b in inventoryButtons) {
					if (cnt < activeCharacter.items.Count) {
						b.enabled = true;
						b.GetComponent<Image> ().enabled = true;
						SpriteState s = new SpriteState ();
						s.highlightedSprite = activeCharacter.items [cnt].sprite_hovered;
						s.pressedSprite = activeCharacter.items [cnt].sprite_pressed;
						b.image.sprite = activeCharacter.items [cnt].sprite;
						b.spriteState = s;
						//b.GetComponentInChildren<Text> ().text = activeCharacter.items [cnt].Name;

						b.interactable = true;
					} else {
						b.interactable = false;
						b.enabled = false;
						b.GetComponent<Image> ().enabled = false;
					}
					cnt++;
				}
            foreach (Text i in texts)
            {
                i.enabled = true;
            }
            sprite.sprite = activeCharacter.activeSpriteObject;
            
            currentHP = MapValues(activeCharacter.HP, 0f, activeCharacter.stats.maxHP, 0f, 1f);
            healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, currentHP, Time.deltaTime * healthSpeed);
			currentExp = MapValues(activeCharacter.exp, 0f, 100, 0f, 1f);
			expbar.fillAmount = Mathf.Lerp(expbar.fillAmount, currentExp, Time.deltaTime * healthSpeed);
            Text t = healthbar.GetComponentInChildren<Text>();
            t.text = activeCharacter.HP + "/" + activeCharacter.stats.maxHP;
            t = expbar.GetComponentInChildren<Text>();
			t.text = activeCharacter.exp + "/100";
			cnt = 0;
        }




    }
    public void SetEndTurnButton(bool value)
    {
        endTurnButton.interactable = value;
    }
	bool notificationAnimation=false;
	bool notificationAnimationEnd=false;
	bool notificationSkillPointsAnimation=false;
	bool notificationSkillPointsAnimationEnd=false;
	Color manacolor;


	private void SetUpSkillLevelUpHud(){
		
	}
	private void SetUpAfterLevelUpHud(){
	}
	private void SetUpSkillAfterLevelUpHud(){
		notificationSkillPointsAnimationEnd = true;
	}

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
    public void OnMouseEnter(Button b)
    {
        /*
		if (activeCharacter != null) {
			for (int i = 0; i < buttons.Length; i++) {
				if (buttons [i] == b) {
					
					mainScript.gridScript.HideMovement ();
					if (i < activeCharacter.charclass.skills.Count)
						ShowActionHover (activeCharacter.charclass.skills [i].name);

				}
			}
		}*/
    }
    public void OnMouseExit(Button b)
    {
		if (activeCharacter != null) {
			
			mainScript.gridScript.HideMovement ();
			if (mainScript.gameState is GameplayState) {
				mainScript.ShowMovement (activeCharacter);
			}
		}
    }
    public void ShowActionHover(string action)
    {
        if (action == "Attack")
        {
            mainScript.ShowAttackRange(activeCharacter);
			mainScript.gridScript.ShowStandOnTexture (activeCharacter);
            return;
        }
        foreach (Skill skill in activeCharacter.charclass.skills)
        {
            if (skill.name == action && skill.CanUseSkill(activeCharacter))
            {
                GetCharacterSkillTargets(skill);
            }
        }
    }
    private void GetCharacterSkillTargets(Skill skill)
    {
        foreach (int range in skill.AttackRanges)
        {
            if (skill.target == SkillTarget.Ally)
            {
                GetCharacters((int)activeCharacter.GetPositionOnGrid().x, (int)activeCharacter.GetPositionOnGrid().y, range, new List<int>(), activeCharacter.team, true);
				mainScript.gridScript.ShowStandOnTexture (activeCharacter);
			}
            else if (skill.target == SkillTarget.Enemy)
            {
                GetCharacters((int)activeCharacter.GetPositionOnGrid().x, (int)activeCharacter.GetPositionOnGrid().y, range, new List<int>(), activeCharacter.team, false);
				mainScript.gridScript.ShowStandOnTexture (activeCharacter);
			}
            else
            {
                GetPositions((int)activeCharacter.GetPositionOnGrid().x, (int)activeCharacter.GetPositionOnGrid().y, range, new List<int>(), skill.CanTargetCharacters());
				mainScript.gridScript.ShowStandOnTexture (activeCharacter);
			}

        }
    }
    public bool CheckAttackField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < mainScript.gridScript.grid.width && y < mainScript.gridScript.grid.height)
        {
            return true;
        }
        return false;
    }
    public void GetCharacters(int x, int y, int range, List<int> direction, int team, bool allies)
    {
        
        if (range <= 0)
        {
            Debug.Log("test1");
            MeshRenderer m = mainScript.gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            if (!allies)
                m.material.mainTexture = mainScript.gridScript.AttackTexture;
            else
                m.material.mainTexture = mainScript.gridScript.healTexture;
            global::Character c = mainScript.gridScript.fields[x, y].character;
            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                GetCharacters(x + 1, y, range - 1, newdirection, team, allies);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetCharacters(x - 1, y, range - 1, newdirection, team, allies);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetCharacters(x, y + 1, range - 1, newdirection, team, allies);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetCharacters(x, y - 1, range - 1, newdirection, team, allies);
            }
        }
    }
    public void GetPositions(int x, int y, int range, List<int> direction, bool canTargetCharacters)
    {
        if (range <= 0)
        {
            global::Character c = mainScript.gridScript.fields[x, y].character;
            MeshRenderer m = mainScript.gridScript.fields[x, y].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = mainScript.gridScript.skillRangeTexture;

            return;
        }
        if (!direction.Contains(2))
        {
            if (CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                GetPositions(x + 1, y, range - 1, newdirection, canTargetCharacters);
            }
        }
        if (!direction.Contains(1))
        {
            if (CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                GetPositions(x - 1, y, range - 1, newdirection, canTargetCharacters);
            }
        }
        if (!direction.Contains(4))
        {
            if (CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                GetPositions(x, y + 1, range - 1, newdirection, canTargetCharacters);
            }
        }
        if (!direction.Contains(3))
        {
            if (CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                GetPositions(x, y - 1, range - 1, newdirection, canTargetCharacters);
            }
        }

    }

    public void ResetButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock cb = buttons[i].colors;
            cb.normalColor = Color.white;
            buttons[i].colors = cb;
        }
    }

    public void OnClick(Button b)
    {
		if (MainScript.ActivePlayerNumber != 0|| MainScript.GetInstance().activeCharacter==null) {
			return;
		}
		for (int i = 0; i < inventoryButtons.Length; i++)
		{
			if (inventoryButtons[i] == b)
			{
				mainScript.InventoryButtonClicked(i);
			}
		}
    }


   
}
