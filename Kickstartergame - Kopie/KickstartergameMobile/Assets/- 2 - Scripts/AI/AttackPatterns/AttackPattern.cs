using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.AttackPatterns
{
    public enum AttackPatternType
    {
        Aggressive,
        Defensive,
        Passive
    }
    public enum AttackTargetType
    {
        NoTarget,
        SingleEnemy,
        SingleAlly,
        TwoEnemies,
        MultipleEnemies,
        MultipleAllies
    }
    public abstract class AttackPattern : Command
    {

        public delegate void OnAttackPatternUsed(Unit user, AttackPattern pattern);
        public static OnAttackPatternUsed onAttackPatternUsed;

        public String Name { get; protected set; }
        public int Damage { get; protected set; }
        public int Hit { get; protected set; }
        public AttackTargetType TargetType { get; protected set; }
        public int TargetCount { get; protected set; }
        public int MaxTargetCount { get; protected set; }
        public  AttackPatternType Type { get; protected set; }
        public List<Vector2> TargetPositions { get; protected set; }
        public List<Injury> PossibleInjuries { get; protected set; }
    }
}
