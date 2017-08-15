using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class DialogState : GameState
    {
        private int characterindex;
        private int stringindex = 0;
        private int letterindex = 0;
        DialogText strings;
        private String oldCharacter;
        public float speed = 0.03f;
        AnimatedDialog dialog;
        MainScript mainScript;
        IEnumerator enumerator;
		public DialogState(DialogText text)
        {
			text = text;
            stringindex = 0;
            letterindex = 0;
            strings = text;
          /* Character TutorialGuy = new Character("TutorialGuy", Characters.Classes.CharacterClassType.SwordFighter);
            strings[0] = new DialogText(TutorialGuy,"Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.");
            strings[1] = new DialogText(TutorialGuy, "ipsum lorem ipsum fuAt vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.");
            strings[2] = new DialogText(TutorialGuy, "fuck lorem ipsum fuck");*/
			mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
			dialog = GameObject.Find("Dialog").GetComponent<AnimatedDialog>();
			dialog.text.text = "";



        }
        public override void enter()
        {
			enumerator = DisplayTimer();
			dialog.StartCoroutine(enumerator);
			dialog.dialog.SetActive(true);
			dialog.GetComponent<AudioSource>().enabled = true;
        }
        IEnumerator DisplayTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(speed);
                if (letterindex > strings.text[stringindex].Length)
                {
                    continue;
                }
				dialog.text.text = strings.text[stringindex].Substring(0, letterindex);
                letterindex++;
                dialog.GetComponent<AudioSource>().Play();
            }
        }
        public override void exit()
        {
			Debug.Log ("EXIT");
			dialog.StopCoroutine(enumerator);
			dialog.dialog.SetActive(false);
			dialog.GetComponent<AudioSource>().enabled = false;
			mainScript.dialogState = null;
        }

        public override void update()
        {
			if (strings.characters [stringindex] != "") {
				if (mainScript.GetCharacterByName (strings.characters [stringindex]) != null) {
					dialog.imageleft.sprite = mainScript.GetCharacterByName (strings.characters [stringindex]).activeSpriteObject;
					dialog.leftname.text = mainScript.GetCharacterByName (strings.characters [stringindex]).name;
				}
			}
			if (oldCharacter != null)
			if (mainScript.GetCharacterByName (oldCharacter) != null) {
				dialog.imageright.sprite = mainScript.GetCharacterByName (oldCharacter).activeSpriteObject;
				dialog.rightname.text = mainScript.GetCharacterByName (oldCharacter).name;
			}

            if(stringindex>0)
				oldCharacter = strings.characters[stringindex - 1];
            if (Input.GetMouseButtonDown(0))
            {
				if (letterindex < strings.text [stringindex].Length) {
					letterindex = strings.text [stringindex].Length;
				} else if (stringindex < strings.text.Count - 1) {
					stringindex++;
					letterindex = 0;
				} else {
					mainScript.SwitchState (new GameplayState ());
				}
            }
            if (Input.GetMouseButtonDown(1))
            {
				mainScript.SwitchState (new GameplayState ());
            }
            if (Input.GetMouseButtonDown(0))
            {

               // speed = 0.01f;
            }
            else { speed = 0.03f; }
        }
    }
}
