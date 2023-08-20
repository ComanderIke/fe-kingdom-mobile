﻿using System;
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
        public int Hit;
        public int Crit;
        public int AttackCount;
        public int CurrentHp;
        public int MaxHp;
        public int AfterBattleHp;
        
        public BattlePreviewStats(int attack, int speed, DamageType damageType, int defense, int skill, int damage, int hit,int crit, int attackCount, int currentHp, int maxHp, int afterBattleHp)//, int currentSp, int maxSp, int afterBattleSp, int currentSpBars,int afterSpBars, int maxSpBars)
        {
            Attack = attack;
            Speed = speed;
            DamageType = damageType;
            Defense = defense;
            Skill = skill;
            Damage = damage;
            Hit = hit;
            Crit = crit;
            AttackCount = attackCount;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            AfterBattleHp = afterBattleHp;
            // CurrentSp = currentSp;
            // MaxSp = maxSp;
            // AfterBattleSp = afterBattleSp;
            // CurrentSpBars = currentSpBars;
            // AfterSpBars = afterSpBars;
            // MaxSpBars = maxSpBars;
        }

        public BattlePreviewStats(DuringBattleCharacterStats stats, int hpBefore):this(stats.Attack, stats.Speed, stats.DamageType, stats.Defense, stats.Skill, stats.Damage, stats.Hit,
            stats.Crit, stats.AttackCount, hpBefore, stats.MaxHp, stats.CurrentHp)
        {
            
        }
    }
    [Serializable]
    public class DuringBattleCharacterStats
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
        public int Hit;
        public int Crit;
        public int AttackCount;
        public int CurrentHp;
        public int MaxHp;

        public DuringBattleCharacterStats(int attack, int speed, DamageType damageType, int defense, int skill, int damage, int hit,int crit, int attackCount, int currentHp, int maxHp)//, int currentSp, int maxSp, int afterBattleSp, int currentSpBars,int afterSpBars, int maxSpBars)
        {
            Attack = attack;
            Speed = speed;
            DamageType = damageType;
            Defense = defense;
            Skill = skill;
            Damage = damage;
            Hit = hit;
            Crit = crit;
            AttackCount = attackCount;
            CurrentHp = currentHp;
            MaxHp = maxHp;
         
        }

        public static DuringBattleCharacterStats CreateInstance(int attack, int speed, DamageType damageType, int defense, int skill, int damage, int hit, int crit, int attackCount, int currentHp, int maxHp)
        {
            return new DuringBattleCharacterStats(attack, speed, damageType, defense, skill, damage, hit, crit, attackCount, currentHp, maxHp);
        }
    }
}
