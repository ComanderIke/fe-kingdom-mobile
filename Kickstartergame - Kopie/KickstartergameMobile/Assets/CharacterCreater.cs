using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreater : MonoBehaviour {

    [SerializeField]
    InputField characterName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MaleGenderClicked()
    {

    }
    public void FemaleGenderClicked()
    {

    }
    public void HairColorClicked(int colorIndex)
    {

    }
    public void SkinToneChanged(int colorIndex)
    {

    }
    public void StaturChanged(int index)
    {

    }
    public void FearSelected()
    {

    }
    public void TraitSelected()
    {

    }
    public void WeaponSelected(int weaponIndex)
    {

    }
    public void CompleteCreation()
    {
        string name = characterName.text;
        Human newChar = new Human(name);
        SpriteScript ss = GameObject.FindObjectOfType<SpriteScript>();
        newChar.Sprite = ss.lancerActiveSprite;
        FindObjectOfType<GameData>().AddUnit(newChar);
        Debug.Log(newChar);
        Debug.Log(newChar.Name);
        SceneManager.LoadScene("Level2");
    }
}
