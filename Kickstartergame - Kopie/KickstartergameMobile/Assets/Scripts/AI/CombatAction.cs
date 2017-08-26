using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class CombatAction
    {
        public CharacterAction type;
        public Character target;
        public Vector2 targetPos;

        public CombatAction(CharacterAction type, Character target, Vector2 targetPos)
        {
            this.type = type;
            this.target = target;
            this.targetPos = targetPos;
        }
    }

}
