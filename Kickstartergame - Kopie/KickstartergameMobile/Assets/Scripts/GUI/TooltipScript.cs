using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.GameStates;
using Assets.Scripts.Items;

public class TooltipScript : MonoBehaviour {
    Text text;
    Button[] buttons;
	Button[] inventoryButtons;
    MainScript mainScript;
	Button[] charSprites;
	Button[] attributeButtons;
	GameObject skillToolTip;
	GameObject itemToolTip;
	GameObject weaponToolTip;
	Button hpBar;
	Button manaBar;
	Button expBar;
	Button attackPreviewWeaponButtonLeft;
	Button attackPreviewWeaponButtonRight;
	Button charViewWeaponButton;
    // Use this for initialization
    void Start () {
      /*  text = GetComponent<Text>();
        buttons = GameObject.Find("SkillButtons").GetComponentsInChildren<Button>();
		inventoryButtons= GameObject.Find("Inventory").GetComponentsInChildren<Button>();
		attributeButtons= GameObject.Find("AttributeButtons").GetComponentsInChildren<Button>();
		charSprites = GameObject.Find ("CharacterSprites").GetComponentsInChildren<Button> ();
        mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();

		skillToolTip = GameObject.Find ("SkillToolTip");
		itemToolTip = GameObject.Find ("ItemToolTip");
		weaponToolTip = GameObject.Find ("WeaponToolTip");
		hpBar = GameObject.Find ("ActiveCharHP").GetComponent<Button>();
		manaBar = GameObject.Find ("ActiveCharMana").GetComponent<Button>();
		expBar = GameObject.Find ("ExpButton").GetComponent<Button>();*/
    }
	void ShowSkillToolTip(int i){
		/*//Debug.Log ("SkillToolTip");
		skillToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		if (mainScript.activeCharacter != null&& mainScript.activeCharacter.skillpoints<=0) {
			if (i < mainScript.activeCharacter.charclass.skills.Count) {
				Text description = skillToolTip.GetComponentInChildren<Text> ();
				description.text = mainScript.activeCharacter.charclass.skills [i].description;
				Text manacost = skillToolTip.GetComponentsInChildren<Text> ()[1];
				string mana = "" +mainScript.activeCharacter.charclass.skills [i].manacost;
				if (mainScript.activeCharacter.charclass.skills [i].manacost == 0) {
					mana = "-";
				}
				manacost.text = "Mana Cost: "+mana;
				Text cooldown = skillToolTip.GetComponentsInChildren<Text> ()[2];
				string cd = "" + mainScript.activeCharacter.charclass.skills [i].Cooldown;
				if (mainScript.activeCharacter.charclass.skills [i].Cooldown == 0) {
					cd = "-";
				}
				cooldown.text = "Cooldown: "+cd;
				Text name = skillToolTip.GetComponentsInChildren<Text> ()[3];
				name.text = mainScript.activeCharacter.charclass.skills [i].name+" Lv "+mainScript.activeCharacter.charclass.skills [i].Level;
				Text newMana = skillToolTip.GetComponentsInChildren<Text> ()[4];
				newMana.text = "";
				Text newCD= skillToolTip.GetComponentsInChildren<Text> ()[5];
				newCD.text = "";
				Text newLevel= skillToolTip.GetComponentsInChildren<Text> ()[6];
				newLevel.text = "";
				skillToolTip.GetComponent<Image> ().enabled = true;
			}
		} else if (mainScript.activeCharacter!=null&& mainScript.activeCharacter.skillpoints>0) {
			Character c = mainScript.activeCharacter;
			if (i < c.charclass.skills.Count) {
				Text description = skillToolTip.GetComponentInChildren<Text> ();
				description.text = c.charclass.skills [i].description;
				Text manacost = skillToolTip.GetComponentsInChildren<Text> ()[1];
				string mana = "" +mainScript.activeCharacter.charclass.skills [i].manacost;
				if (mainScript.activeCharacter.charclass.skills [i].manacost == 0) {
					mana = "-";
				}
				manacost.text = "Mana Cost: "+mana;
				Text cooldown = skillToolTip.GetComponentsInChildren<Text> ()[2];
				string cd = "" + mainScript.activeCharacter.charclass.skills [i].Cooldown;
				if (mainScript.activeCharacter.charclass.skills [i].Cooldown == 0) {
					cd = "-";
				}
				cooldown.text = "Cooldown: "+cd;
				Text name = skillToolTip.GetComponentsInChildren<Text> ()[3];
				name.text = mainScript.activeCharacter.charclass.skills [i].name+" Lv "+mainScript.activeCharacter.charclass.skills [i].Level;

				skillToolTip.GetComponent<Image> ().enabled = true;
			}
		}
        */

	}
	void ShowItemToolTip(int i){
		//itemToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//Text description = itemToolTip.GetComponentsInChildren<Text> ()[1];
		//description.text = mainScript.activeCharacter.items [i].Description;
		//Text name = itemToolTip.GetComponentsInChildren<Text> ()[0];
		//name.text = mainScript.activeCharacter.items [i].Name;
		//itemToolTip.GetComponent<Image> ().enabled = true;
		//Image icon = itemToolTip.GetComponentsInChildren<Image> ()[1];
		//icon.enabled = true;
		////Debug.Log (icon.name);
		//icon.sprite = mainScript.activeCharacter.items [i].sprite;
	}
	void ShowWeaponToolTip(int i){
		//weaponToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//Text description = weaponToolTip.GetComponentsInChildren<Text> ()[3];
		//description.text = mainScript.activeCharacter.items [i].Description;
		//Text name = weaponToolTip.GetComponentsInChildren<Text> ()[0];
		//name.text = mainScript.activeCharacter.items [i].Name;
		//weaponToolTip.GetComponent<Image> ().enabled = true;
		//Text damage = weaponToolTip.GetComponentsInChildren<Text> () [1];
		//damage.text = ""+((Weapon)mainScript.activeCharacter.items [i]).dmg;
		//Text hit = weaponToolTip.GetComponentsInChildren<Text> () [2];
		//hit.text =""+ ((Weapon)mainScript.activeCharacter.items [i]).hit;
		//Image icon = weaponToolTip.GetComponentsInChildren<Image> ()[1];
		//icon.sprite = ((Weapon)mainScript.activeCharacter.items [i]).sprite;
		//icon.enabled = true;
	
	}
	void ShowCharViewWeaponToolTip(){
		//weaponToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//Text description = weaponToolTip.GetComponentsInChildren<Text> ()[3];
		//description.text = mainScript.lastClickedCharacter.EquipedWeapon.Description;
		//Text name = weaponToolTip.GetComponentsInChildren<Text> ()[0];
		//name.text = mainScript.lastClickedCharacter.EquipedWeapon.Name;
		//weaponToolTip.GetComponent<Image> ().enabled = true;
		//Text damage = weaponToolTip.GetComponentsInChildren<Text> () [1];
		//damage.text = ""+mainScript.lastClickedCharacter.EquipedWeapon.dmg;
		//Text hit = weaponToolTip.GetComponentsInChildren<Text> () [2];
		//hit.text =""+ mainScript.lastClickedCharacter.EquipedWeapon.hit;
		//Image icon = weaponToolTip.GetComponentsInChildren<Image> ()[1];
		//icon.sprite = mainScript.lastClickedCharacter.EquipedWeapon.sprite;
		//icon.enabled = true;

	}
	void ShowAttackPreviewWeaponToolTip(int i){
		//Text name = weaponToolTip.GetComponentsInChildren<Text> ()[0];
		//Text damage = weaponToolTip.GetComponentsInChildren<Text> () [1];
		//Text hit = weaponToolTip.GetComponentsInChildren<Text> () [2];
		//Image icon = weaponToolTip.GetComponentsInChildren<Image> ()[1];
		//Text description = weaponToolTip.GetComponentsInChildren<Text> ()[3];
		//icon.enabled = true;
		//weaponToolTip.GetComponent<Image> ().enabled = true;
		//if (i == 0) {
		//	Character c = mainScript.activeCharacter;
		//	weaponToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//	description.text = c.EquipedWeapon.Description;
		//	name.text =c.EquipedWeapon.Name;
		//	damage.text = ""+c.EquipedWeapon.dmg;
		//	hit.text =""+ c.EquipedWeapon.hit;
		//	icon.sprite = c.EquipedWeapon.sprite;
		//} else {
		//	Character c = mainScript.fightCharacter;
		//	weaponToolTip.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//	description.text = c.EquipedWeapon.Description;
		//	name.text = c.EquipedWeapon.Name;
		//	damage.text = ""+c.EquipedWeapon.dmg;
		//	hit.text =""+ c.EquipedWeapon.hit;
		//	icon.sprite = c.EquipedWeapon.sprite;
		//}
	}
	void ShowAttributeToolTip(int i){
		//string attr = "";
		//switch (i) {
		//case 0:
		//	attr = "Increases damage";// Strength
		//	break;
		//case 1: // Defense
		//	attr = "Reducdes physical damage";
		//	break;
		//case 2: // Speed
		//	attr = "Improves dodging/attackspeed";
		//	break;
		//case 3: // Skill
		//	attr = "Increases hitchance";
		//	break;
		//case 4: // Resistance
		//	attr = "Reducdes magic damage";
		//	break;
		//}
		//text.text = attr;
		//text.transform.parent.GetComponent<Image> ().enabled = true;
	}
	void ShowHPToolTip(){
		//text.text = "Health Points";
		//text.transform.parent.GetComponent<Image> ().enabled = true;
	}

	void ShowExpToolTip(){
		//text.text = "Experience Points";
		//text.transform.parent.GetComponent<Image> ().enabled = true;
	}
	// Update is called once per frame
	void Update () {
  //      this.transform.parent.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
		//if(show){
		//	mousedowntime += Time.deltaTime;
		//	GameObject tmp = GameObject.Find ("WeaponButtonR");
		//	if (tmp != null) {
		//		attackPreviewWeaponButtonRight = tmp.GetComponent<Button> ();
		//	}
		//	tmp = GameObject.Find ("WeaponButtonL");
		//	if (tmp != null) {
		//		attackPreviewWeaponButtonLeft = tmp.GetComponent<Button> ();
		//	}
		//	tmp = GameObject.Find ("CharViewButton");
		//	if (tmp != null) {
		//		charViewWeaponButton = tmp.GetComponent<Button> ();
		//	}
		//	if (mousedowntime >= 0.5f) {
		//		for (int i = 0; i < buttons.Length; i++) {
		//			if (buttons [i] == hoveredButton) {
		//				ShowSkillToolTip (i);
		//			}
		//		}
		//		for (int i = 0; i < inventoryButtons.Length; i++) {
		//			if (inventoryButtons [i] == hoveredButton) {
		//				if (mainScript.activeCharacter != null) {
		//					if (i < mainScript.activeCharacter.items.Count) {
		//						if (mainScript.activeCharacter.items [i] is Weapon)
		//							ShowWeaponToolTip (i);
		//						else
		//							ShowItemToolTip (i);
		//					}
		//				}
		//			}
		//		}
		//		for (int i = 0; i < charSprites.Length; i++) {
		//			if (charSprites [i] == hoveredButton) {
		//				text.text = mainScript.GetCharactersForHud () [i].name;
		//				text.transform.parent.GetComponent<Image> ().enabled = true;
		//			}
		//		}
		//		for (int i = 0; i < attributeButtons.Length; i++) {
		//			if (attributeButtons [i] == hoveredButton) {
		//				ShowAttributeToolTip (i);

		//			}
		//		}
		//		if (charViewWeaponButton == hoveredButton) {
		//			ShowCharViewWeaponToolTip ();
		//		}
		//		if (attackPreviewWeaponButtonLeft == hoveredButton) {
		//			ShowAttackPreviewWeaponToolTip (0);
		//		}
		//		if (attackPreviewWeaponButtonRight == hoveredButton) {
		//			ShowAttackPreviewWeaponToolTip (1);
		//		}
		//		if (hpBar == hoveredButton) {
		//			ShowHPToolTip ();
		//		}
		//		if (manaBar == hoveredButton) {
		//			ShowManaToolTip ();
		//		}
		//		if (expBar == hoveredButton) {
		//			ShowExpToolTip ();
		//		}
		//		mousedowntime = 0;
		//	}
		//}
    }
	float mousedowntime=0;
	bool show = false;
	Button hoveredButton;
    public void Activate(Button go)
    {
		//mousedowntime = 0;
		//show = true;
		//hoveredButton = go;
    }
    public void DeActivate()
    {
		//show = false;
  //      text.text = "";
  //      text.transform.parent.GetComponent<Image>().enabled = false;
		//skillToolTip.GetComponent<Image> ().enabled = false;
		//itemToolTip.GetComponent<Image> ().enabled = false;
		//weaponToolTip.GetComponent<Image> ().enabled = false;
		//foreach(Image t in skillToolTip.GetComponentsInChildren<Image>()){
		//	t.enabled = false;
		//}
		//foreach(Image t in itemToolTip.GetComponentsInChildren<Image>()){
		//	t.enabled = false;
		//}
		//foreach(Image t in weaponToolTip.GetComponentsInChildren<Image>()){
		//	t.enabled = false;
		//}
		//foreach(Text t in skillToolTip.GetComponentsInChildren<Text>()){
		//	t.text = "";
		//}
		//foreach(Text t in itemToolTip.GetComponentsInChildren<Text>()){
		//	t.text = "";
		//}
		//foreach(Text t in weaponToolTip.GetComponentsInChildren<Text>()){
		//	t.text = "";
		//}
    }
}
