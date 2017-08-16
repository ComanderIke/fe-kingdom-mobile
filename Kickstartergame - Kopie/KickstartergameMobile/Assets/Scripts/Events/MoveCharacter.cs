using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Characters.Classes;

namespace AssemblyCSharp
{
	public class MoveCharacter : MonoBehaviour
	{
		private float hellebardier_run_speed = 2.7f;
		private float mage_run_speed = 3f;
		bool moving=false;
		Character character;
		Vector3 pos;
		MovementPath path;
		MainScript mainScript;
		int pathCounter;
		void Start(){
			MainScript.moveCharacterEvent += StartMovingCharacter;
		}
		void StartMovingCharacter(Character c, int x, int y){
			moving = true;
			character = c;
			this.pos = new Vector3(x,y,0);
			mainScript = GameObject.Find (MainScript.MAIN_GAME_OBJ).GetComponent<MainScript> ();
			//character.gameObject.GetComponent<CharacterScript>().PlayRun();
			path = mainScript.gridScript.getPath(c.x, c.y, (int)pos.x, (int)pos.y, character.team, false, new List<int>());
            Debug.Log("Move " + c.name + " from " + c.x + " " + c.y + " to " + pos.x + " " + pos.y + " ");
			path.Reverse();
			pathCounter = 1;
		}

		void Update(){
			if (!moving)
				return;
/*
			character.gameObject.GetComponent<CharacterScript>().setRunning();

			float x = character.gameObject.transform.localPosition.x;
			float y = character.gameObject.transform.localPosition.y;
			float z = character.gameObject.transform.localPosition.z;
			float tx = path.getStep(pathCounter).getX();
			float ty =path.getStep(pathCounter).getY();
			float tz = path.getStep(pathCounter).getZ();
			float walkspeed = 5f;
			if (character.characterClassType == CharacterClassType.Mage)
			{
				walkspeed = mage_run_speed;
			}
			if (character.characterClassType == CharacterClassType.SwordFighter)
			{
				walkspeed = 3.3f;
			}
			if (character.characterClassType == CharacterClassType.Hellebardier)
			{
				walkspeed = hellebardier_run_speed;
			}
			float value = walkspeed * Time.deltaTime;
			float yvalue = walkspeed * Time.deltaTime;
			float offset = 0.005f;
			if (y != ty)
			{
				if (y > ty)
				{
					if (y - yvalue < ty)
						character.gameObject.transform.localPosition = new Vector3(x, ty, z);
					else
						character.gameObject.transform.localPosition = new Vector3(x, y - yvalue, z);
				}
				else if (y < ty)
				{
					if (y + yvalue > ty)
						character.gameObject.transform.localPosition = new Vector3(x, ty, z);
					else
						character.gameObject.transform.localPosition = new Vector3(x, y + yvalue, z);
				}
			}
			y = character.gameObject.transform.localPosition.y;

			if (x != tx)
			{
				if (x < tx)
				{
					character.rotation = 90;
					character.gameObject.transform.localRotation = Quaternion.AngleAxis(90, Vector3.up);
					if (x + value > tx)
						character.gameObject.transform.localPosition = new Vector3(tx, y, z);
					else
						character.gameObject.transform.localPosition = new Vector3(x + value, y, z);

				}
				else if (x > tx)
				{
					character.rotation = 270;
					character.gameObject.transform.localRotation = Quaternion.AngleAxis(270, Vector3.up);
					if (x - value < tx)
						character.gameObject.transform.localPosition = new Vector3(tx, y, z);
					else
						character.gameObject.transform.localPosition = new Vector3(x - value, y, z);
				}
			}
			else if (z != tz)
			{
				if (z > tz)
				{
					character.rotation = 180;
					character.gameObject.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up);
					if (z - value < tz)
						character.gameObject.transform.localPosition = new Vector3(x, y, tz);
					else
						character.gameObject.transform.localPosition = new Vector3(x, y, z - value);
				}
				else if (z < tz)
				{
					character.rotation = 0;
					character.gameObject.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
					if (z + value > tz)
						character.gameObject.transform.localPosition = new Vector3(x, y, tz);
					else
						character.gameObject.transform.localPosition = new Vector3(x, y, z + value);
				}
			}
			if (character.gameObject.transform.localPosition.x + offset > tx && character.gameObject.transform.localPosition.x - offset < tx && character.gameObject.transform.localPosition.z + offset > tz && character.gameObject.transform.localPosition.z - offset < tz)
			{
				pathCounter++;
			}

			if (pathCounter >= path.getLength())
			{
				pathCounter = 0;
				character.SetPosition((int)tx, (int)tz);
				character.gameObject.GetComponent<CharacterScript>().StopRun();
				character.gameObject.GetComponent<CharacterScript>().setIdle();
                if(MainScript.endOfMoveCharacterEvent!=null)
				MainScript.endOfMoveCharacterEvent ();
				moving = false;
			}
            */
		}
	}
}

