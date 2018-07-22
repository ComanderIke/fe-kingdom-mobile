using Assets.Scripts.AI;
using Assets.Scripts.Dialogs;
using Assets.Scripts.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    
    public abstract class Unit : ScriptableObject, SpeakableObject
    {
        #region Events
        public delegate void OnHPValueChanged();
        public static OnHPValueChanged onHpValueChanged;

        public delegate void OnSPValueChanged();
        public static OnSPValueChanged onSpValueChanged;

        public delegate void OnUnitWaiting(Unit unit, bool waiting);
        public static OnUnitWaiting onUnitWaiting;

        public delegate void OnUnitCanMove(Unit unit, bool canMove);
        public static OnUnitCanMove onUnitCanMove;



        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);
        public static OnUnitShowActiveEffect onUnitShowActiveEffect;

        public delegate void OnUnitDied(Unit unit);
        public static OnUnitDied onUnitDied;
        #endregion

        public String Name;
        public GameTransform GameTransform { get; set; }
        public BattleStats BattleStats { get; set; }
        public UnitTurnState UnitTurnState { get; set; }
        public ExperienceManager ExperienceManager { get; set; }
        [HideInInspector]
        public int hp;
        public int HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (value > Stats.MaxHP)
                    hp = Stats.MaxHP;
                else
                {
                    hp = value;
                }
                if (hp <= 0)
                {
                    hp = 0;
                }
                if (Unit.onHpValueChanged != null)
                    Unit.onHpValueChanged();
            }
        }
        [HideInInspector]
        public int sp;
        
        public int SP
        {
            get
            {
                return sp;
            }
            set
            {
                if (value > Stats.MaxSP)
                    sp = Stats.MaxSP;
                else
                {
                    sp = value;
                }
                if (sp <= 0)
                {
                    sp = 0;
                }
                if (Unit.onSpValueChanged != null)
                    Unit.onSpValueChanged();
            }
        }
        public Stats Stats;

        public AIAgent Agent { get; set; }
        public Army Player { get; set; }
        public List<Debuff> Debuffs { get; set; }
        public List<Buff> Buffs { get; set; }
        public Sprite Sprite;
        public GridPosition GridPosition { get; set; }
        public MoveActions MoveActions { get; set; }



        public void OnEnable()
        {
            ExperienceManager = new ExperienceManager();
            BattleStats = new BattleStats(this);
            UnitTurnState = new UnitTurnState(this);
            GridPosition = new GridPosition(this);
            MoveActions = new MoveActions(this);
            GameTransform = new GameTransform();
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
            List<int> attackRanges = new List<int>();
            Agent = new AIAgent();
            
            attackRanges.Add(1);
            HP = Stats.MaxHP;
            SP = Stats.MaxSP;
        }

        public void EndTurn()
        {
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
        public bool IsActive()
        {
            return UnitTurnState.IsWaiting==false;
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
            onUnitDied(this);
            GridPosition.RemoveCharacter();
            GameTransform.Destroy();
        }
        public void Heal(int heal)
        {
            HP += heal;
        }
        public bool CanAttack(int range)
        {
            return Stats.AttackRanges.Contains(range);
        }
        public bool IsAlive()
        {
            return HP > 0;
        }
        public int InflictDamage(int dmg, Unit damagedealer, bool magic = false)
        {
            float multiplier = 1.0f;
            int inflictedDmg;
            if (magic)
            {
                inflictedDmg = (int)(dmg  * multiplier);
            }
            else
            {
                inflictedDmg = (int)((dmg - Stats.Defense) * multiplier);
            }
            
            if (inflictedDmg <= 0)
                inflictedDmg = 1;
            HP -= inflictedDmg;
            return inflictedDmg;
        }
        public T GetType<T>()
        {
            if(this is T) { 
                 return (T)Convert.ChangeType(this, typeof(T));
            }
            return default(T);
        }
        //Interface SpeakableObject
        public void ShowSpeechBubble(string text)
        {
            SpeechBubble sb = this.GameTransform.UnitController.SpeechBubble;
            if (sb)
                sb.Show(text);
            else
                Debug.Log("No SpeechBubble Component!");
        }

    }
}
