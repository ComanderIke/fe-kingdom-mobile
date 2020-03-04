using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameActors.Players;
using Assets.GameActors.Units.Attributes;
using Assets.GameActors.Units.CharStateEffects;
using Assets.GameActors.Units.OnGameObject;
using Assets.Grid;
using Assets.Mechanics.Battle;
using Assets.Mechanics.Dialogs;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public abstract class Unit : ScriptableObject, ISpeakableObject
    {
        [HideInInspector] private int hp;

        public string Name;

        [HideInInspector] private int sp;


        public Stats Stats;

        public CharacterSpriteSet CharacterSpriteSet;

        public GameTransform GameTransform { get; private set; }
        public BattleStats BattleStats { get; private set; }
        public UnitTurnState UnitTurnState { get; private set; }
        public ExperienceManager ExperienceManager { get; private set; }

        public int Hp
        {
            get => hp;
            set
            {
                hp = value > Stats.MaxHp ? Stats.MaxHp : value;
                if (hp <= 0) hp = 0;
                HpValueChanged?.Invoke();
            }
        }

        public int Sp
        {
            get => sp;
            set
            {
                sp = value > Stats.MaxSp ? Stats.MaxSp : value;
                if (sp <= 0) sp = 0;
                SpValueChanged?.Invoke();
            }
        }

        public AIAgent Agent { get; private set; }
        public Faction Player { get; set; }
        public List<Debuff> Debuffs { get; private set; }
        public List<Buff> Buffs { get; private set; }
        public GridPosition GridPosition { get; set; }

        public MoveActions MoveActions { get; private set; }
        public Motivation Motivation { get; internal set; }

        

        //Interface SpeakableObject
        public void ShowSpeechBubble(string text)
        {
            Debug.Log("TODO ADD GAME FEATURE???");
        }

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
            Agent = new AIAgent();

            Hp = Stats.MaxHp;
            Sp = Stats.MaxSp;
        }

        public void EndTurn()
        {
            UnitTurnState.Reset();
        }

        public void UpdateTurn()
        {
            var debuffEnd = Debuffs.Where(d => d.TakeEffect(this)).ToList();
            var buffEnd = Buffs.Where(b => b.TakeEffect(this)).ToList();
            foreach (var d in debuffEnd) d.Remove(this);
            foreach (var b in buffEnd) b.Remove(this);
            UnitTurnState.Reset();
        }

        public bool IsActive()
        {
            return UnitTurnState.IsWaiting == false;
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
            GameTransform.SetPosition(GridPosition.X, GridPosition.Y);
            GameTransform.EnableCollider();
        }

        public void Die()
        {
            UnitDied(this);
            GridPosition.RemoveCharacter();
            GameTransform.Destroy();
        }

        public void Heal(int heal)
        {
            Hp += heal;
        }

        public bool CanAttack(int range)
        {
            return Stats.AttackRanges.Contains(range);
        }

        public bool IsAlive()
        {
            return Hp > 0;
        }

        public int InflictDamage(int dmg, Unit damageDealer, bool magic = false)
        {
            var multiplier = 1.0f;
            int inflictedDmg;
            if (magic)
                inflictedDmg = (int) (dmg * multiplier);
            else
                inflictedDmg = (int) ((dmg - Stats.Def) * multiplier);

            if (inflictedDmg <= 0)
                inflictedDmg = 1;
            Hp -= inflictedDmg;
            return inflictedDmg;
        }

        public T GetType<T>()
        {
            if (this is T) return (T) Convert.ChangeType(this, typeof(T));
            return default;
        }

        public override string ToString()
        {
            return Name + " HP: " + Hp + "/" + Stats.MaxHp;
        }

        #region Events

        public delegate void OnHpValueChanged();

        public static OnHpValueChanged HpValueChanged;

        public delegate void OnSpValueChanged();

        public static OnSpValueChanged SpValueChanged;

        public delegate void OnUnitWaiting(Unit unit, bool waiting);

        public static OnUnitWaiting UnitWaiting;

        public delegate void OnUnitCanMove(Unit unit, bool canMove);

        public OnUnitCanMove UnitCanMove;

        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect UnitShowActiveEffect;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        #endregion
    }
}