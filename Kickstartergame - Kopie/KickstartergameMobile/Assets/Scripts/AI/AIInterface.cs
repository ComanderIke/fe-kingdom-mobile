
using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class AIInterface
    {
        protected Player player;
        public const float PAUSE_BETWEEN_ACTIONS = 0.5f;
        const float PAUSE_START = 1.0f;
        const float PAUSE_END = 1.0f;
        protected MainScript mainScript;
        public static float pausetime = 0;
        public static float starttime = 0;
        public static float endtime = 0;
        protected bool endturn = false;

        public AIInterface(Player p)
        {
            this.player = p;
            mainScript = MainScript.GetInstance();
        }

        public void update()
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
                    mainScript.EndTurn();
                }
            }
            else
            {
                if (pausetime >= PAUSE_BETWEEN_ACTIONS)
                {//wait 1second so the player can follow what the AI is doing
                    pausetime = 0;
                    Think();

                }
                pausetime += Time.deltaTime;
            }
        }

        public abstract void Think();

        protected List<LivingObject> GetUnitsLeftToMove()
        {
            List<LivingObject> units = new List<LivingObject>();//= GetCharactersLeftToMove();
            foreach (LivingObject c in player.getCharacters())
            {
                if (c.isWaiting == false && c.isAlive)
                {
                    units.Add(c);
                }
            }
            return units;
        }
        protected List<Vector2> GetMoveLocations(LivingObject c)
        {
            List<Vector2> locations = new List<Vector2>();
            mainScript.gridScript.HideMovement();
            locations.Add(new Vector2(c.x, c.y));
            mainScript.gridScript.GetMovement((int)c.gameObject.transform.position.x, (int)c.gameObject.transform.position.y, locations, c.movRange, 0, c.team);
            return locations;
        }

        protected List<CharacterAction> GetActionsForUnit(LivingObject c)
        {//TODO not testet
            List<CharacterAction> actions = new List<CharacterAction>();
            actions.Add(CharacterAction.Wait);
            //if (mainScript.GetAttackTargets(c).Count != 0)
            //{
            //        actions.Add(CharacterAction.Attack);
            //}
            return actions;
        }
        protected void SetCharacterPosition(LivingObject c, Vector2 pos)
        {
            int OldPosX = (int)c.GetPositionOnGrid().x;
            int OldPosY = (int)c.GetPositionOnGrid().y;
            c.SetInternPosition((int)pos.x, (int)pos.y);
        }

        private GameState DoCombatAction(LivingObject c, CombatAction action)
        {
            switch (action.type)
            {
                case CharacterAction.Wait:
                    //Do nothing
                    return new AIState(c.player);
                case CharacterAction.Attack:
                    return new FightState((Character)c,action.target, new AIState(c.player));
            }
            return null;
        }

        protected void SubmitMove(LivingObject character, Vector2 location, CombatAction combatAction)
        {
            GameState gameState = DoCombatAction(character, combatAction);
            if (character.x == location.x && character.y == location.y && combatAction.type == CharacterAction.Wait)
                pausetime = PAUSE_BETWEEN_ACTIONS;
            mainScript.SwitchState(new MovementState(mainScript, (Character)character, (int)location.x, (int)location.y, false, gameState));
        }

    }
}
