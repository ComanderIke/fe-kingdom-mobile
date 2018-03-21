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
    public void HairStyleClicked(int colorIndex)
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
        RessourceScript ss = GameObject.FindObjectOfType<RessourceScript>();
        selectedWeaponIndex = weaponIndex;
        sprite.sprite = ss.sprites.GetCharacterOnMapSprites(selectedWeaponIndex);
    }
    public void CompleteCreation()
    {
        string name = characterName.text;
        RessourceScript ss = FindObjectOfType<RessourceScript>();
        DataScript ws = FindObjectOfType<DataScript>();
        Human newChar = new Human(name, ss.sprites.GetCharacterOnMapSprites(0));

        if (selectedWeaponIndex == 0)
        {
            ws.weapons.woodenSword.use(newChar);
        }
        else if (selectedWeaponIndex == 1)
        {
            ws.weapons.basicBow.use(newChar);
        }
        else if (selectedWeaponIndex == 2)
        {
            ws.weapons.woodenAxe.use(newChar);
        }
        else if (selectedWeaponIndex == 3)
        {
            ws.weapons.woodenSpear.use(newChar);     
        }
        newChar.Sprite = ss.sprites.GetCharacterOnMapSprites(selectedWeaponIndex);
        FindObjectOfType<GameData>().AddUnit(newChar);
      
        newChar.Inventory.AddItem(newChar.EquipedWeapon);
        Debug.Log(newChar);
        Debug.Log(newChar.Name);
        SceneManager.LoadScene("Level2");
    }
    public void MainMenuClicked()
    {
        StartCoroutine(DelayMenu(1.0f));
    }
    IEnumerator DelayMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }
}
