using Assets.Scripts.Characters;
using Assets.Scripts.Grid.PathFinding;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameStates
{
    class MovementState : GameState
    {
        private int x;
        private int y;
        public int pathCounter = 1;
        public MovementPath path;
        private MainScript mainScript;
        private LivingObject character;
        private GameState targetState;
        List<Vector2> mousePath;
        bool drag;

        public MovementState(LivingObject c, int x, int y, bool drag,GameState targetState)
        {
            this.x = x;
            this.y = y;
            this.targetState = targetState;
            this.drag = drag;
            mainScript = MainScript.GetInstance();
            character = c;
        }
        public MovementState( LivingObject c,int x,int y, bool drag, GameState targetState, List<Vector2> path):this(c, x, y, drag, targetState)
        {
            mousePath = path;
        }

        public override void enter()
        {
            MouseManager.active = false;
            if (mousePath == null)
            {
                if (character.GridPosition.x == x && character.GridPosition.y== y)
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
                mainScript.gridManager.HideMovement();
            }
            if ( mousePath.Count == 0)
            {
                path = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, new List<int>());
                if (path!=null)
                    path.Reverse();
                pathCounter = 1;
            }
            if(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter is Monster)
            {
                for (int i = 0; i < mousePath.Count; i++)
                {
                    mousePath[i] = new Vector3(mousePath[i].x - 0.5f, mousePath[i].y - 0.5f, 0);
                }
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
            float x = character.GameTransform.GameObject.transform.localPosition.x;
            float y = character.GameTransform.GameObject.transform.localPosition.y;
            float z = character.GameTransform.GameObject.transform.localPosition.z;
            float tx ;
            float ty ;
            if (mousePath != null&&mousePath.Count>0)
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
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(tx, y, z);
                    else
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x + value, y, z);

                }
                else if (x > tx)
                {
                    if (x - value < tx)
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(tx, y, z);
                    else
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x - value, y, z);
                }
            }
            else if (y != ty)
            {
                if (y > ty)
                {

                    if (y - value < ty)
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x, ty, z);
                    else
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x, y-value, z);
                }
                else if (y< ty)
                {

                    if (y + value > ty)
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x, ty, z);
                    else
                        character.GameTransform.GameObject.transform.localPosition = new Vector3(x, y+value, z);
                }
            }
            if (character.GameTransform.GameObject.transform.localPosition.x + offset > tx && character.GameTransform.GameObject.transform.localPosition.x - offset < tx && character.GameTransform.GameObject.transform.localPosition.y + offset > ty && character.GameTransform.GameObject.transform.localPosition.y - offset < ty)
            {
                pathCounter++;
            }

            if ((path!=null&&pathCounter >= path.getLength())||(path==null&&mousePath!=null&&pathCounter>=mousePath.Count))
            {
                pathCounter = 0;
                character.SetPosition((int)tx, (int)ty);
                 if(MainScript.endOfMoveCharacterEvent!=null)
                    MainScript.endOfMoveCharacterEvent();
                character.UnitTurnState.HasMoved = true;
                if (character.Player.IsHumanPlayer)
                {
                    if (drag)
                    {
                        if (targetState is GameplayState)
                            mainScript.GetSystem<UnitActionManager>().ActiveCharWait();
                        mainScript.SwitchState(targetState);
                    }
                    else
                    {
                        if (targetState is GameplayState)
                            mainScript.GetSystem<UnitActionManager>().ActiveCharWait();
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
