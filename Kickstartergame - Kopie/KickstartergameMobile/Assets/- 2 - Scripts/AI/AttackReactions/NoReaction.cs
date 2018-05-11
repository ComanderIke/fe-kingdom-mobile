using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Injuries;
using UnityEngine;
using Assets.Scripts.Grid;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;
using Assets.Scripts.Events;
using System.Collections;
using Assets.Scripts.Characters.CharStateEffects;
using Assets.Scripts.AI.AttackReactions;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class NoReaction : AttackReaction
    {
        

        public NoReaction(LivingObject unit):base(unit)
        {
            
            Name = "No Reaction";
        }

        public override void Execute()
        {
            Debug.Log("Do Nothing!");
            EventContainer.reactionFinished();
        }
    }
}
