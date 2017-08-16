using Assets.Scripts.Characters.Classes;
using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    class MovementState : GameState
    {
        private int x;
        private int z;
		public static float hellebardier_run_speed = 2.7f;
		public static float mage_run_speed = 3f;
        public int pathCounter = 1;
        public MovementPath path;
        private MainScript mainScript;
        private Character character;
        private GameState targetState;
        List<Vector3> mousePath;
        bool drag;
        public MovementState(MainScript mainScript,Character c, int x, int z, bool drag,GameState targetState)
        {
            this.mainScript = mainScript;
            this.x = x;
            this.z = z;
            this.targetState = targetState;
            this.character = c;
            this.drag = drag;
        }
        public MovementState(MainScript mainScript, Character c, List<Vector3>path, bool drag, GameState targetState)
        {
            this.mainScript = mainScript;
            this.targetState = targetState;
            this.character = c;
            this.drag = drag;
            this.mousePath = path;
            Debug.Log(mousePath);
            Debug.Log(mainScript.activeCharacter.name);
        }

        public override void enter()
        {/*
            MouseManager.active = false;
            if (mousePath == null)
            {
                if (character.x == x && character.z == z)
                {
                    mainScript.SwitchState(targetState);
                    return;
                }
            }
            else {
                pathCounter = 0;
            }
            if (drag)
            {
                mainScript.gridScript.HideMovement();
            }
            character.gameObject.GetComponent<CharacterScript>().PlayRun();
            if (mousePath == null)
            {
                Debug.Log(character.name + " ");
                path = mainScript.gridScript.getPath((int)character.x, (int)character.z, x, z, character.team, false, new List<int>());
                if(path!=null)
                    path.Reverse();
            }
            GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().SetPosition(x, MainScript.CURSOROFFSET, z);
            */
        }

        public override void exit()
        {
            MouseManager.active = true;
        }

        public override void update()
        {
            ContinueWalkAnimation();
        }
        void ContinueWalkAnimation()
        {
            /*
            //speedincr=1.0f;
            mainScript.cameraSpeed = 3f;
            character.gameObject.GetComponent<CharacterScript>().setRunning();

            float x = character.gameObject.transform.localPosition.x;

    
            float y = character.gameObject.transform.localPosition.y;
            float z = character.gameObject.transform.localPosition.z;
            float tx ;
            float ty ;
            float tz;
            if (mousePath != null)
            {
                tx = mousePath[pathCounter].x;
                ty = mousePath[pathCounter].y;
                tz = mousePath[pathCounter].z;

            }
            else
            {
               tx = path.getStep(pathCounter).getX();
               ty = path.getStep(pathCounter).getY();
               tz = path.getStep(pathCounter).getZ();
            }
                
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
			if (character.characterClassType == CharacterClassType.Archer)
			{
				walkspeed = 3.8f;
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

            if ((path!=null&&pathCounter >= path.getLength())||(mousePath!=null&&pathCounter>=mousePath.Count))
            {
                pathCounter = 0;
                character.SetPosition((int)tx, (int)tz);
                //mainScript.gridScript.fields[(int)mainScript.oldPosition.x, (int)mainScript.oldPosition.y].character = null;
                //character.gameObject.transform.localPosition = new Vector3(tx, ty, tz);
                //mainScript.gridScript.fields[(int)tx, (int)tz].character = character;
                character.gameObject.GetComponent<CharacterScript>().StopRun();
                GameObject.Find(MainScript.CURSOR_NAME).GetComponent<CursorScript>().SetPosition((int)character.GetPositionOnGrid().x, MainScript.CURSOROFFSET, (int)character.GetPositionOnGrid().y);
                mainScript.MoveCursorTo((int)character.GetPositionOnGrid().x, (int)character.GetPositionOnGrid().y);

                 if(MainScript.endOfMoveCharacterEvent!=null)
                    MainScript.endOfMoveCharacterEvent();
                character.hasMoved = true;
                if (character.player.isPlayerControlled)
                {
                    if (drag)
                    {
                        if (targetState is GameplayState)
                            mainScript.ActiveCharWait();
                        character.gameObject.GetComponent<CharacterScript>().setIdle();
                        mainScript.SwitchState(targetState);
                       
                    }
                    else
                    {
                        // mainScript.ShowActionMenue();//switches State
                        if (targetState is GameplayState)
                            mainScript.ActiveCharWait();

                        character.gameObject.GetComponent<CharacterScript>().setIdle();
                        mainScript.SwitchState(targetState);
                    }
                }
                else
                {
                    //mainScript.hasMoved = true;
                    character.gameObject.GetComponent<CharacterScript>().setIdle();
                    //mainScript.speedincr = 1.0f;
                    mainScript.SwitchState(targetState);
                }
            }
            */
        }
    }
}
