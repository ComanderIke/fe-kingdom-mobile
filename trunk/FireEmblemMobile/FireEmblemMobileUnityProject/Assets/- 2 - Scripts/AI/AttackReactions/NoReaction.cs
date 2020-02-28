using UnityEngine;
using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.AI.AttackReactions;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class NoReaction : AttackReaction
    {
        

        public NoReaction(Unit unit):base(unit)
        {
            
            Name = "No Reaction";
        }

        public override void Execute()
        {
            Debug.Log("Do Nothing!");
            UnitActionSystem.onReactionFinished();
        }
    }
}
