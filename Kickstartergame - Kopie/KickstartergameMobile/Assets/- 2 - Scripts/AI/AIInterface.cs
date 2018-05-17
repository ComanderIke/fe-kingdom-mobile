﻿
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class AIInterface
    {
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        const float PAUSE_START = 1.0f;
        const float PAUSE_END = 1.0f;

        public static float pausetime = 0;
        public static float starttime = 0;
        public static float endtime = 0;

        protected MainScript mainScript;
        protected Player player;
        protected bool endturn = false;

        public AIInterface(Player p)
        {
            this.player = p;
            mainScript = MainScript.GetInstance();
        }

        public void Update()
        {
            starttime += Time.deltaTime;
            if (starttime < PAUSE_START)
            {
                return;
            }
            if (endturn)
            {
                endtime += Time.deltaTime;
                if (endtime >= PAUSE_END)
                {

                    mainScript.SwitchState(new GameplayState());
                    EventContainer.endTurn();
                }
            }
            else
            {
                //wait 1second so the player can follow what the AI is doing
                if (pausetime >= PAUSE_BETWEEN_ACTIONS)
                {
                    pausetime = 0;
                    Think();

                }
                pausetime += Time.deltaTime;
            }
        }

        public abstract void Think();

        protected List<LivingObject> GetUnitsLeftToMove()
        {
            List<LivingObject> units = new List<LivingObject>();
            foreach (LivingObject c in player.Units)
            {
                if (c.UnitTurnState.IsWaiting == false && c.IsAlive())
                {
                    units.Add(c);
                }
            }
            return units;
        }
        protected List<Vector2> GetMoveLocations(LivingObject c)
        {
            List<Vector2> locations = new List<Vector2>();
            mainScript.GetSystem<GridSystem>().HideMovement();
            locations = mainScript.GetSystem<GridSystem>().GetMovement((int)c.GameTransform.GameObject.transform.position.x, (int)c.GameTransform.GameObject.transform.position.y, c.Stats.MoveRange, c.Player.ID);
            return locations;
        }

        protected void SetCharacterPosition(LivingObject c, Vector2 pos)
        {
            c.SetInternPosition((int)pos.x, (int)pos.y);
        }

        //private GameState DoCombatAction(LivingObject c, CombatAction action)
        //{
        //    switch (action.type)
        //    {
        //        case CharacterAction.Wait:
        //            //Do nothing
        //            return new AIState(c.Player);
        //        case CharacterAction.Attack:
        //            return new FightState((LivingObject)c, action.target);//, new AIState(c.Player));
        //    }
        //    return null;
        //}

        protected void SubmitMove(LivingObject character, Vector2 location)
        {
            //just adding the Command but not executing it yet
            //mainScript.GetSystem<CameraSystem>().MoveCameraTo((int)location.x, (int)location.y);
            mainScript.GetSystem<UnitActionSystem>().MoveCharacter(character, (int)location.x,(int) location.y);
        }

    }
}
