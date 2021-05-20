using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using UnityEngine;

namespace Game.Mechanics.Battle
{
    [Serializable]
    public class BattlePreviewStats
    {
        [HideInInspector]
        public int Attack;
        [HideInInspector]
        public int Speed;
        public DamageType DamageType;
        [HideInInspector]
        public int Defense;
        [HideInInspector]
        public int Skill;
        public int Damage;
        public int AttackCount;
        public int CurrentHp;
        public int MaxHp;
        public int AfterBattleHp;
        public List<int> IncomingDamage;
        public int CurrentSp;
        public int MaxSp;
        public int AfterBattleSp;
        public List<int> IncomingSpDamage;

        public BattlePreviewStats(int attack, int speed, DamageType damageType, int defense, int skill, int damage, int attackCount, int currentHp, int maxHp, int afterBattleHp, List<int> incomingDamage, int currentSp, int maxSp, int afterBattleSp, List<int> incomingSpDamage)
        {
            Attack = attack;
            Speed = speed;
            DamageType = damageType;
            Defense = defense;
            Skill = skill;
            Damage = damage;
            AttackCount = attackCount;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            AfterBattleHp = afterBattleHp;
            IncomingDamage = incomingDamage;
            CurrentSp = currentSp;
            MaxSp = maxSp;
            AfterBattleSp = afterBattleSp;
            IncomingSpDamage = incomingSpDamage;
        }
    }
}
