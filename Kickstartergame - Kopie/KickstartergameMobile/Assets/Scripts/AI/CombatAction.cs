using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class CombatAction
    {
        public CharacterAction type;
        public LivingObject target;
        public Vector2 targetPos;

        public CombatAction(CharacterAction type, LivingObject target, Vector2 targetPos)
        {
            this.type = type;
            this.target = target;
            this.targetPos = targetPos;
        }
    }

}
