
using Assets.Scripts.Characters;
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
            mainScript.gridManager.HideMovement();
            locations.Add(new Vector2(c.GridPosition.x, c.GridPosition.y));
            Debug.Log("TODO: GetMOVELOCATIONS");
            //mainScript.gridManager.GetMovement((int)c.GameTransform.GameObject.transform.position.x, (int)c.GameTransform.GameObject.transform.position.y, locations, c.Stats.MoveRange, 0, c.Player.number);
            return locations;
        }

        protected List<CharacterAction> GetActionsForUnit(LivingObject c)
        {
            List<CharacterAction> actions = new List<CharacterAction>();
            actions.Add(CharacterAction.Wait);
            return actions;
        }

        protected void SetCharacterPosition(LivingObject c, Vector2 pos)
        {
            int OldPosX = c.GridPosition.x;
            int OldPosY = c.GridPosition.y;
            c.SetInternPosition((int)pos.x, (int)pos.y);
        }

        private GameState DoCombatAction(LivingObject c, CombatAction action)
        {
            switch (action.type)
            {
                case CharacterAction.Wait:
                    //Do nothing
                    return new AIState(c.Player);
                case CharacterAction.Attack:
                    return new FightState((LivingObject)c,action.target, new AIState(c.Player));
            }
            return null;
        }

        protected void SubmitMove(LivingObject character, Vector2 location, CombatAction combatAction)
        {
            GameState gameState = DoCombatAction(character, combatAction);
            if (character.GridPosition.x == location.x && character.GridPosition.y == location.y && combatAction.type == CharacterAction.Wait)
                pausetime = PAUSE_BETWEEN_ACTIONS;
            mainScript.SwitchState(new MovementState(character, (int)location.x, (int)location.y, false, gameState));
        }

    }
}
