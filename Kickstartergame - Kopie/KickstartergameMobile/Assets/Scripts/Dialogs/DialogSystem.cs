using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.GameStates;

public class DialogText{
	public List<String> text;
	public List<String> characters;
	public DialogText(List<String> text, List<String>characters){
		this.text = text;
		this.characters = characters;

	}



}
class DialogKey{
	List<string> characternames;
	public DialogKey(List<string>names){
		characternames = names;
	}
	bool contains=true;
	public bool IsKey(List<string> names){
		if (names.Count == characternames.Count) {
			for (int i = 0; i < characternames.Count; i++) {
				if (characternames [i] != names [i]) {
					contains = false;
				}
			}
		} else {
			contains = false;
		}
		return contains;
	}
}
class DialogTextCollection{
	public List<DialogText> texts;
	List<DialogKey> keys;

	public DialogTextCollection(){
		texts = new List<DialogText> ();
		keys = new List<DialogKey>();

	}
	public void Add(DialogKey key, DialogText text){
		texts.Add (text);
		keys.Add (key);
	}
	private int GetIndex(List<Character> characters){
		List<string> names = new List<string> ();
		foreach (Character c in characters) {
			names.Add (c.name);
		}
		int index = 0;
		foreach (DialogKey key in keys) {
			if(key.IsKey(names))
				return index;
			index++;
		}
		return -1;
	}
	public DialogText GetText(List<Character> characters){
		int index = GetIndex (characters);
		if (index != -1)
			return texts [index];
		else
			return null;
	}
}
public class DialogSystem : MonoBehaviour {

	DialogText dialogtext;
	DialogTextCollection dialogData;
	DialogKey testinsert;
	// Use this for initialization
	void Start () {
		List<string> testlist = new List<string> ();
		testlist.Add ("Rosali");
		testlist.Add ("Fritz");
		testinsert = new DialogKey (testlist);
		dialogData = new DialogTextCollection ();
		List<string> testtext = new List<string> ();
		List<string> chars = new List<string> ();
		chars.Add ("Rosali");
		chars.Add ("Fritz");
		testtext.Add ("Who are you?");
		testtext.Add ("I am Fritz!");
		DialogText testdialog = new DialogText(testtext, chars);
		dialogData.Add (testinsert, testdialog);
		testlist = new List<string> ();
		testlist.Add ("Flora");
		testlist.Add ("Fritz");
		testinsert = new DialogKey (testlist);
		testtext = new List<string> ();
		chars = new List<string> ();
		chars.Add ("Flora");
		chars.Add ("Fritz");
		testtext.Add ("Who are you?");
		testtext.Add ("I am Fritz!");
		testdialog = new DialogText(testtext, chars);
		dialogData.Add (testinsert, testdialog);
		testlist = new List<string> ();
		testlist.Add ("Siegfried");
		testlist.Add ("Fritz");
		testinsert = new DialogKey (testlist);
		testtext = new List<string> ();
		chars = new List<string> ();
		chars.Add ("Siegfried");
		chars.Add ("Fritz");
		testtext.Add ("Who are you?");
		testtext.Add ("I am Fritz!");
		testdialog = new DialogText(testtext, chars);
		dialogData.Add (testinsert, testdialog);
		//dialogData.Add (testinsert, testdialog);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Talk(Character character, Character talktarget){
		List<Character> characters = new List<Character> ();
		characters.Add (character);
		characters.Add (talktarget);
		Debug.Log(character.name+" " +talktarget.name);
		DialogText text=dialogData.GetText (characters);
		if(text!=null)
			Debug.Log(text.characters[0]+": \""+text.text[0]+"\"");
		GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ().SwitchState(new DialogState (text));

	}
}
