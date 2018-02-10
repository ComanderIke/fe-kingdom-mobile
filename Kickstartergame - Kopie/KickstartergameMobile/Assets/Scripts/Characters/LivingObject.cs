using Assets.Scripts.AI;
using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Characters.Debuffs;
using Assets.Scripts.Events;
using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    
    public abstract class LivingObject
    {


        public GameTransform GameTransform { get; set; }
        public BattleStats BattleStats { get; set; }
        public UnitTurnState UnitTurnState { get; set; }
        public ExperienceManager ExperienceManager { get; set; }
        public Stats Stats { get; set; }
        public StatsGrowth StatsGrowth { get; set; }
        public String Name { get; set; }
        public Player Player { get; set; }
        public List<Debuff> Debuffs { get; set; }
        public List<Buff> Buffs { get; set; }
        public Sprite Sprite { get; set; }
        public GridPosition GridPosition { get; set; }
        public MoveActions MoveActions { get; set; }
        public List<Goal> AIGoals { get; set; }

        public LivingObject(string name, Sprite sprite)
        {
            Name = name;
            ExperienceManager = new ExperienceManager();
            BattleStats = new BattleStats(this);
            UnitTurnState = new UnitTurnState(this);
            GridPosition = new GridPosition(this);
            MoveActions = new MoveActions(this);
            GameTransform = new GameTransform();
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
            Sprite = sprite;
            List<int> attackRanges = new List<int>();
            AIGoals = new List<Goal>();
            attackRanges.Add(1);
            Stats = new Stats(15, 5, 10, 5, 2, 5, 2,3,attackRanges);
            
        }

        public void EndTurn()
        {
            Debug.Log("EndTurn");
            UnitTurnState.Reset();
        }
        public void UpdateTurn()
        {
            List<Debuff> debuffEnd = new List<Debuff>();
            List<CharacterState> buffEnd = new List<CharacterState>();
            foreach (Debuff d in Debuffs)
            {
                if (d.TakeEffect(this))
                    debuffEnd.Add(d);
            }
            foreach (CharacterState b in Buffs)
            {
                if (b.TakeEffect(this))
                    buffEnd.Add(b);
            }
            foreach (Debuff d in debuffEnd)
            {
                d.Remove(this);
            }
            foreach (CharacterState b in buffEnd)
            {
                b.Remove(this);
            }
            UnitTurnState.Reset();
        }
        public void SetInternPosition(int x, int y)
        {
            GridPosition.SetPosition(x, y);
        }
       
        public virtual void SetPosition(int x, int y)
        {
            GridPosition.SetPosition(x, y);
            
            GameTransform.SetPosition(x, y);
        }
        public void ResetPosition()
        {
            GameTransform.SetPosition(GridPosition.x,GridPosition.y);
            GameTransform.EnableCollider();
        }
        public void Die()
        {
            EventContainer.unitDied(this);
            GridPosition.RemoveCharacter();
            GameTransform.Destroy();
        }
        public void Heal(int heal)
        {
            Stats.HP += heal;
        }
        public bool CanAttack(int range)
        {
            return Stats.AttackRanges.Contains(range);
        }


        public bool IsAlive()
        {
            return Stats.HP > 0;
        }

        public void UpdateOnWholeTurn()
        {
            UnitTurnState.Reset();
           
        }

        public int InflictDamage(int dmg, LivingObject damagedealer)
        {
            float multiplier = 1.0f;
            int inflictedDmg = (int)((dmg - Stats.Defense) * multiplier);
            if (inflictedDmg <= 0)
                inflictedDmg = 1;
            Stats.HP -= inflictedDmg;
            if (Stats.HP > 0)
            {
                if (inflictedDmg <= 5)
                {
                    CameraShake.Shake(0.3f, 0.02f);
                }
                else
                {
                    CameraShake.Shake(0.35f, 0.05f);
                }
            }
            else
            {
                CameraShake.Shake(0.35f, 0.12f);
            }
            return inflictedDmg;
        }
        public T GetType<T>()
        {
            if(this is T) { 
                 return (T)Convert.ChangeType(this, typeof(T));
            }
            return default(T);
        }


    }
}
