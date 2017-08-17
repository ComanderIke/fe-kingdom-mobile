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
        private int y;
		public static float hellebardier_run_speed = 2.7f;
		public static float mage_run_speed = 3f;
        public int pathCounter = 1;
        public MovementPath path;
        private MainScript mainScript;
        private Character character;
        private GameState targetState;
        List<Vector3> mousePath;
        bool drag;
        public MovementState(MainScript mainScript,Character c, int x, int y, bool drag,GameState targetState)
        {
            this.mainScript = mainScript;
            this.x = x;
            this.y = y;
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
        {
            MouseManager.active = false;
            if (mousePath == null)
            {
                if (character.x == x && character.y== y)
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
            if (mousePath == null)
            {
                Debug.Log(character.name + " ");
                path = mainScript.gridScript.getPath((int)character.x, (int)character.y, x, y, character.team, false, new List<int>());
                if(path!=null)
                    path.Reverse();
            }
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
            

            float x = character.gameObject.transform.localPosition.x;
            float y = character.gameObject.transform.localPosition.y;
            float z = character.gameObject.transform.localPosition.z;
            float tx ;
            float ty ;
            if (mousePath != null)
            {
                tx = mousePath[pathCounter].x;
                ty = mousePath[pathCounter].y;

            }
            else
            {
               tx = path.getStep(pathCounter).getX();
               ty = path.getStep(pathCounter).getY();
            }
                
            float walkspeed = 5f;
            float value = walkspeed * Time.deltaTime;
            float offset = 0.005f;

            if (x != tx)
            {
                if (x < tx)
                {
                    if (x + value > tx)
                        character.gameObject.transform.localPosition = new Vector3(tx, y, z);
                    else
                        character.gameObject.transform.localPosition = new Vector3(x + value, y, z);

                }
                else if (x > tx)
                {
                    if (x - value < tx)
                        character.gameObject.transform.localPosition = new Vector3(tx, y, z);
                    else
                        character.gameObject.transform.localPosition = new Vector3(x - value, y, z);
                }
            }
            else if (y != ty)
            {
                if (y > ty)
                {

                    if (y - value < ty)
                        character.gameObject.transform.localPosition = new Vector3(x, ty, z);
                    else
                        character.gameObject.transform.localPosition = new Vector3(x, y-value, z);
                }
                else if (y< ty)
                {

                    if (y + value > ty)
                        character.gameObject.transform.localPosition = new Vector3(x, ty, z);
                    else
                        character.gameObject.transform.localPosition = new Vector3(x, y+value, z);
                }
            }
            if (character.gameObject.transform.localPosition.x + offset > tx && character.gameObject.transform.localPosition.x - offset < tx && character.gameObject.transform.localPosition.y + offset > ty && character.gameObject.transform.localPosition.y - offset < ty)
            {
                pathCounter++;
            }

            if ((path!=null&&pathCounter >= path.getLength())||(mousePath!=null&&pathCounter>=mousePath.Count))
            {
                pathCounter = 0;
                character.SetPosition((int)tx, (int)ty);
                 if(MainScript.endOfMoveCharacterEvent!=null)
                    MainScript.endOfMoveCharacterEvent();
                character.hasMoved = true;
                if (character.player.isPlayerControlled)
                {
                    if (drag)
                    {
                        if (targetState is GameplayState)
                            mainScript.ActiveCharWait();
                        mainScript.SwitchState(targetState);
                    }
                    else
                    {
                        if (targetState is GameplayState)
                            mainScript.ActiveCharWait();
                        mainScript.SwitchState(targetState);
                    }
                }
                else
                {

                    mainScript.SwitchState(targetState);
                }
            }
        }
    }
}
