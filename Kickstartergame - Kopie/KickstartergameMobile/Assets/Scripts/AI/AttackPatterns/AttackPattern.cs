using Assets.Scripts.Injuries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.AttackPatterns
{
    public interface AttackPattern
    {
        String Name { get; }
        int Damage { get; }
        int Hit { get; }
        int TargetCount { get; }
        int MaxTargetCount { get; }
        List<Vector2> TargetPositions { get; }
        List<Injury> PossibleInjuries { get; }
        void Execute();
    }
}
