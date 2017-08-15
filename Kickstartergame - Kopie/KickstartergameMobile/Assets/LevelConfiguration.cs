using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration : MonoBehaviour {


    bool init = false;
    void Update () {
        if (!init)
        {
            init = true;
            MainScript.players[0].getCharacters()[0].gameObject.SetActive(true);
            MainScript.players[0].getCharacters()[1].gameObject.SetActive(true);
            MainScript.players[0].getCharacters()[2].gameObject.SetActive(true);
            MainScript.players[0].getCharacters()[0].InstantiateWeapon();
            MainScript.players[0].getCharacters()[1].InstantiateWeapon();
            MainScript.players[0].getCharacters()[2].InstantiateWeapon();
            CharacterScript leilaCS = MainScript.players[0].getCharacters()[1].gameObject.GetComponent<CharacterScript>();
           // leilaCS.ForceIdle();
            CharacterScript floraCS = MainScript.players[0].getCharacters()[0].gameObject.GetComponent<CharacterScript>();
           // floraCS.ForceIdle();
            CharacterScript hectorCS = MainScript.players[0].getCharacters()[2].gameObject.GetComponent<CharacterScript>();
           // hectorCS.ForceIdle();
            CharacterScript enemyCS = MainScript.players[1].getCharacters()[0].gameObject.GetComponent<CharacterScript>();
           // enemyCS.ForceIdle();
            float height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)leilaCS.transform.position.x, (int)leilaCS.transform.position.z);
            leilaCS.transform.position = new Vector3(leilaCS.transform.position.x, height, leilaCS.transform.position.z);
            height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)floraCS.transform.position.x, (int)floraCS.transform.position.z);
            floraCS.transform.position = new Vector3(floraCS.transform.position.x, height, floraCS.transform.position.z);
            height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)hectorCS.transform.position.x, (int)hectorCS.transform.position.z);
            hectorCS.transform.position = new Vector3(hectorCS.transform.position.x, height, hectorCS.transform.position.z);
            MainScript.players[0].getCharacters()[1].SetPosition(0, 14);
            MainScript.players[0].getCharacters()[0].SetPosition(1, 14);
            MainScript.players[0].getCharacters()[0].SetRotation(270);
            MainScript.players[0].getCharacters()[2].SetRotation(90);
            MainScript.players[1].getCharacters()[0].SetRotation(270);
            MainScript.players[0].getCharacters()[2].SetPosition(3, 13);
            MainScript.players[1].getCharacters()[0].SetPosition(4, 13);
            MainScript.players[1].getCharacters()[0].HP = 8;
            if (GameObject.Find("sight_restrictions") != null)
                GameObject.Find("sight_restrictions").SetActive(false);
            leilaCS = MainScript.players[0].getCharacters()[1].gameObject.GetComponent<CharacterScript>();
            //leilaCS.ForceIdle();
            floraCS = MainScript.players[0].getCharacters()[0].gameObject.GetComponent<CharacterScript>();
            //floraCS.ForceIdle();
            hectorCS = MainScript.players[0].getCharacters()[2].gameObject.GetComponent<CharacterScript>();
            //hectorCS.ForceIdle();
            height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)leilaCS.transform.position.x, (int)leilaCS.transform.position.z);
            leilaCS.transform.position = new Vector3(leilaCS.transform.position.x, height, leilaCS.transform.position.z);
            height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)floraCS.transform.position.x, (int)floraCS.transform.position.z);
            floraCS.transform.position = new Vector3(floraCS.transform.position.x, height, floraCS.transform.position.z);
            height = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.GetHeight((int)hectorCS.transform.position.x, (int)hectorCS.transform.position.z);
            hectorCS.transform.position = new Vector3(hectorCS.transform.position.x, height, hectorCS.transform.position.z);
            MainScript.players[0].getCharacters()[1].HP = 20;
            MainScript.players[0].getCharacters()[2].HP = 25;
            //MainScript.players [1].getCharacters () [0].gameObject.GetComponent<CharacterScript> ().PlayDeath ();
            MainScript.players[0].getCharacters()[1].SetPosition(4, 14);
            MainScript.players[0].getCharacters()[1].SetRotation(180);
            MainScript.GetInstance().activeCharacter = null;
            MainScript.players[0].getCharacters()[0].SetPosition(1, 14);
            CameraMovement.locked = false;
            //GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().gridScript.fields[4, 13].character = null;//TODO WHY?
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().SwitchState(new GameplayState());
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().activePlayer = MainScript.players[0];
            MainScript.ActivePlayerNumber = 0;
        }
		
	}
}
