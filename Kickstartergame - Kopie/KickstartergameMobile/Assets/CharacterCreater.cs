using Assets.Scripts.Characters;
using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreater : MonoBehaviour
{

    [SerializeField]
    InputField characterName;
    [SerializeField]
    Image sprite;
    int selectedWeaponIndex = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        SpriteScript ss = GameObject.FindObjectOfType<SpriteScript>();
        selectedWeaponIndex = weaponIndex;
        if (selectedWeaponIndex == 0)
            sprite.sprite = ss.swordActiveSprite;
        else if (selectedWeaponIndex == 1)
            sprite.sprite = ss.archerActiveSprite;
        else if (selectedWeaponIndex == 2)
            sprite.sprite = ss.axeActiveSprite;
        else if (selectedWeaponIndex == 3)
            sprite.sprite = ss.lancerActiveSprite;
    }
    public void CompleteCreation()
    {
        string name = characterName.text;
        SpriteScript ss = FindObjectOfType<SpriteScript>();
        WeaponScript ws = FindObjectOfType<WeaponScript>();
        Human newChar = new Human(name, ss.swordActiveSprite);
       


        if (selectedWeaponIndex == 0)
        {
            ws.woodenSword.use(newChar);
            newChar.Sprite = ss.swordActiveSprite;
        }
        else if (selectedWeaponIndex == 1)
        {
            ws.basicBow.use(newChar);
            newChar.Sprite = ss.archerActiveSprite;
        }
        else if (selectedWeaponIndex == 2)
        {
            ws.woodenAxe.use(newChar);
            newChar.Sprite = ss.axeActiveSprite;
        }
        else if (selectedWeaponIndex == 3)
        {
            ws.woodenSpear.use(newChar);
            newChar.Sprite = ss.lancerActiveSprite;
        }
        FindObjectOfType<GameData>().AddUnit(newChar);
      
        newChar.Inventory.AddItem(newChar.EquipedWeapon);
        Debug.Log(newChar);
        Debug.Log(newChar.Name);
        SceneManager.LoadScene("Level2");
    }
}
