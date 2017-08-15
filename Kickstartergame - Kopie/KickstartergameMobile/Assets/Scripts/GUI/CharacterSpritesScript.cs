using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSpritesScript : MonoBehaviour {
	
    // Use this for initialization
    Button[] buttons;
    MainScript mainScript;
    Character[] characters;
	Image backgroundImage;
	public Sprite background3Chars;
	public Sprite background4Chars;
	public Sprite background5Chars;
	public Sprite background6Chars;

	float healthSpeed = 2;
	void Start () {
        buttons=GetComponentsInChildren<Button>();
        mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
		backgroundImage = GameObject.Find ("CharSpritesBackground").GetComponent<Image>();

	}
	public Button[] GetButtons(){
		return buttons;
	}
	// Update is called once per frame
	void Update () {
		
        //TODO Performance?!
        int cnt = 0;
			foreach (Character c in mainScript.GetCharactersForHud()) {
				if (c == mainScript.clickedCharacter) {
					SpriteState s = new SpriteState ();
					s.disabledSprite = c.passiveSprite_selected;
					buttons [cnt].spriteState = s;
					buttons [cnt].interactable = false;
					Image [] img=buttons [cnt].GetComponentsInChildren<Image> ();
					img[1].enabled = false;
					img[2].enabled = false;
					img[3].enabled = false;
				} else {
					buttons [cnt].image.sprite = c.passiveSpriteObject;
					SpriteState s = new SpriteState ();
					s.highlightedSprite = c.passiveSpriteMOObject;
					buttons [cnt].spriteState = s;
					buttons [cnt].interactable = true;
					Image [] img=buttons [cnt].GetComponentsInChildren<Image> ();
					img[1].enabled = true;
					img[2].enabled = true;
					img[3].enabled = true;

				}
				Image[] images = buttons [cnt].GetComponentsInChildren<Image> ();
				Text text = buttons [cnt].GetComponentInChildren<Text> ();
				text.enabled = true;
				text.text = "Level " + c.level;
				/*foreach (Image img in images) {
					img.enabled = true;
				}*/
				float currentHP = MapValues (c.HP, 0f, c.stats.maxHP, 0f, 1f);
				images [1].fillAmount = Mathf.Lerp (images [1].fillAmount, currentHP, Time.deltaTime * healthSpeed);
				float currentEXP = MapValues (c.exp, 0f, 100, 0f, 1f);
				images [3].fillAmount = Mathf.Lerp (images [3].fillAmount, currentEXP, Time.deltaTime * healthSpeed);
				cnt++;
			}
			switch (cnt) {
			case 3:
				backgroundImage.sprite = background3Chars;
				break;
			case 4:
				backgroundImage.sprite = background4Chars;
				break;
			case 5:
				backgroundImage.sprite = background5Chars;
				break;
			case 6:
				backgroundImage.sprite = background6Chars;
				break;
			}
			for (; cnt < buttons.Length; cnt++) {
				buttons [cnt].gameObject.GetComponent<Image> ().enabled = false;
				buttons [cnt].enabled = false;
				Text text = buttons [cnt].GetComponentInChildren<Text> ();
				text.enabled = false;
				foreach (Image img in buttons[cnt].GetComponentsInChildren<Image>()) {
					img.enabled = false;
				}
			}
	}


	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
	{
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
    public void OnClick(Button b)
    {
		int buttonclicked = -1;
        for( int i=0; i< buttons.Length; i++)
        {
            if(buttons[i]== b)
            {
				buttonclicked = i;
				break;
               
            }
        }
		mainScript.CharacterSpriteButtonClicked (buttonclicked);

    }
}
