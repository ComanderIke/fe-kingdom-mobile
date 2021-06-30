namespace Unused.Dialogs
{
	[System.Serializable]
	public class Dialogue
	{
		public string sentences;

		public string name;
		/*
		private int characterindex;
		private int stringindex = 0;
		private int letterindex = 0;
		DialogText strings;
		private String oldCharacter;
		public float speed = 0.03f;
		AnimatedDialog dialog;
		MainScript mainScript;
		IEnumerator enumerator;
		public Dialog(DialogText text)
		{
			text = text;
			stringindex = 0;
			letterindex = 0;
			strings = text;
			mainScript = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
			dialog = GameObject.Find("Dialog").GetComponent<AnimatedDialog>();
			dialog.text.text = "";

		}
		public void start(){
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
		public void stop()
		{
			dialog.StopCoroutine(enumerator);
			dialog.dialog.SetActive(false);
			dialog.GetComponent<AudioSource>().enabled = false;
		}

		public void update()
		{
			dialog.dialog.SetActive(true);
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
					stop ();
				}
			}
			if (Input.GetMouseButtonDown(1))
			{
				stop ();
			}
			if (Input.GetMouseButtonDown(0))
			{

				// speed = 0.01f;
			}
			else { speed = 0.03f; }
		}
        */
	}
}