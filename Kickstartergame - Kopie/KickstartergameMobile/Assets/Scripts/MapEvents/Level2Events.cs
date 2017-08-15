using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	abstract class Event{
		public abstract bool Update (MainScript mainScript);
	}
	public class Level2Events
	{
		List<Event> events;
		MainScript mainScript;
		List<Event> delete;
		public Level2Events (MainScript mainScript)
		{
			events=new List<Event>();
			events.Add (new BridgeCollapseEvent ());
			this.mainScript = mainScript;
			delete = new List<Event> ();
		}
		public void Update(){
			foreach (Event e in events) {
				if(e.Update(mainScript))
					delete.Add(e);
			}
			foreach (Event e in delete) {
				events.Remove (e);
			}
			delete.Clear();

		}
	}
	class BridgeCollapseEvent: Event{

		public override bool Update(MainScript mainScript){
			if (mainScript.characterRooms.Count!=0&&!mainScript.characterRooms.ContainsValue (1) && mainScript.activeCharacter == null) {
				Debug.Log ("Characterrooms: " + mainScript.characterRooms.Count);
				Debug.Log ("Collapse Bridge!");
				//mainScript.MoveCameraTo (8, 14);
				mainScript.gridScript.fields [8, 14].isAccessible = false;
				mainScript.gridScript.fields [9, 14].isAccessible = false;
				GameObject.Find ("prefab_anim_bridge_higher_destruction").GetComponent<Animator> ().SetTrigger ("Destruction");
				return true;
			}
			else {
				return false;
			}
		}
	}

}

