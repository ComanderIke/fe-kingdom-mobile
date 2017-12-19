using Assets.Scripts.Commands;
using Assets.Scripts.Injuries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.AttackPatterns
{
    public enum AttackPatternType
    {
        Aggressive,
        Defensive,
        Passive
    }
    public abstract class AttackPattern : Command
    {
        public String Name { get; protected set; }
        public int Damage { get; protected set; }
        public int Hit { get; protected set; }
        public int TargetCount { get; protected set; }
        public int MaxTargetCount { get; protected set; }
        public  AttackPatternType Type { get; protected set; }
        public List<Vector2> TargetPositions { get; protected set; }
        public List<Injury> PossibleInjuries { get; protected set; }
    }
}
