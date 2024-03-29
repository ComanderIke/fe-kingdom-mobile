﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameActors.Players;
using Assets.GameActors.Units.Attributes;
using Assets.GameActors.Units.CharStateEffects;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameResources;
using Assets.Grid;
using Assets.Mechanics;
using Assets.Mechanics.Battle;
using Assets.Mechanics.Dialogs;
using Assets.SerializedData;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public abstract class Unit : ScriptableObject, ISpeakableObject, ICloneable
    {
        [HideInInspector] private int hp;

        public string Name;

        [HideInInspector] private int sp;
        [HideInInspector] private int ap;
        public delegate void OnUnitLevelUpEvent(string name, int levelBefore, int levelAfter, int[] stats, int[] statIncreases);
        public static OnUnitLevelUpEvent OnUnitLevelUp;
        public delegate void OnExpGainedEvent(Unit unit, int expBefore, int expGained);
        public static event OnExpGainedEvent OnExpGained;
        public delegate void OnBuffEvent(Buff buff);
        public delegate void OnDebuffEvent(Debuff debuff);
        public event OnBuffEvent OnBuffAdded;
        public event OnDebuffEvent OnDebuffAdded;
        public event OnBuffEvent OnBuffRemoved;
        public event OnDebuffEvent OnDebuffRemoved;

        public Stats Stats;
        public Growths Growths;

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
        public int Ap
        {
            get => ap;
            set
            {
                ap = value > Stats.Mov ? Stats.Mov : value;
                if (ap <= 0) ap = 0;
                ApValueChanged?.Invoke();
                OnUnitActiveStateUpdated?.Invoke(this, ap != 0, false);
            }
        }

        public AIAgent Agent { get; private set; }
        public Faction Faction { get; set; }
        private List<Debuff> Debuffs { get; set; }
        private List<Buff> Buffs { get; set; }

        public GridPosition GridPosition { get; set; }

        public MoveActions MoveActions { get; private set; }
        public Motivation Motivation { get; internal set; }



        //Interface SpeakableObject
        public void ShowSpeechBubble(string text)
        {
            Debug.Log("TODO ADD GAME FEATURE???");
        }
        public void AddBuff(Buff buff)
        {
            Buffs.Add(buff);
            OnBuffAdded?.Invoke(buff);
        }
        public void AddDebuff(Debuff debuff)
        {
            Debuffs.Add(debuff);
            OnDebuffAdded?.Invoke(debuff);
        }
        public void RemoveBuff(Buff buff)
        {
            Buffs.Remove(buff);
            OnBuffRemoved?.Invoke(buff);
        }
        public void RemoveDebuff(Debuff debuff)
        {
            Debuffs.Remove(debuff);
            OnDebuffRemoved?.Invoke(debuff);
        }
        public void OnEnable()
        {
           
        }
        void OnDestroy()
        {
            ExperienceManager.OnLevelUp -= LevelUp;
        }
        public void Initialize()
        {
           
            ExperienceManager = new ExperienceManager();
            ExperienceManager.OnLevelUp += LevelUp;
            BattleStats = new BattleStats(this);
            UnitTurnState = new UnitTurnState(this);
            GridPosition = new GridPosition(this);
            MoveActions = new MoveActions(this);
            GameTransform = new GameTransform();
            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
            Agent = new AIAgent();
            Stats = Stats == null ? CreateInstance<Stats>() : Instantiate(Stats);
            Growths = Growths == null ? CreateInstance<Growths>() : Instantiate(Growths);
            Hp = Stats.MaxHp;
            Sp = Stats.MaxSp;
            Ap = Stats.Mov;
            ExperienceManager.OnExpGained += ExpGained;
            UnitTurnState.OnHasAttacked += HasAttacked;
            
        }
        private void HasAttacked(bool value)
        {
            if(value)
                AddDebuff(DataScript.Instance.CharacterStateData.CantAttack);
            else
                RemoveDebuff(DataScript.Instance.CharacterStateData.CantAttack);
        }
        private void ExpGained(int expBefore, int expGained)
        {
            OnExpGained?.Invoke(this, expBefore, expGained);
        }
       
        private void LevelUp(int levelBefore, int levelAfter)
        {
           
            int[] statIncreases=CalculateStatIncreases();
            OnUnitLevelUp?.Invoke(Name, levelBefore, levelAfter, Stats.GetStatArray(), statIncreases);
            Stats.MaxHp += statIncreases[0];
            Stats.MaxSp += statIncreases[1];
            Stats.Str += statIncreases[2];
            Stats.Mag += statIncreases[3];
            Stats.Spd += statIncreases[4];
            Stats.Skl += statIncreases[5];
            Stats.Def += statIncreases[6];
            Stats.Res += statIncreases[7];
        }
        private int[] CalculateStatIncreases()
        {
            int[] growths = Growths.GetGrowthsArray();
            int[] increaseAmount = new int[growths.Length];
            for (int i = 0; i < growths.Length; i++)
            {
                increaseAmount[i] = Method(growths[i]);
            }
            return increaseAmount;
        }
        private int Method(int Growth)
        {
            int rngNumber = (int)(UnityEngine.Random.value * 100f);
            if (Growth > 100)
            {
                return 1 + Method(Growth - 100);
            }
            if (rngNumber <= Growth)
            {
                return 1;
            }
            return 0;
            
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
            Ap = Stats.Mov;
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
        public virtual void SetGameTransformPosition(int x, int y)
        {
            GameTransform.SetPosition(x, y);
        }
        public virtual Vector2 GetGameTransformPosition()
        {
            return GameTransform.GetPosition();
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

        public object Clone()
        {
            var clone = (Unit) this.MemberwiseClone();
            HandleCloned(clone);
            return clone;
        }

        protected virtual void HandleCloned(Unit clone)
        {
            clone.ExperienceManager = new ExperienceManager();
            clone.BattleStats = new BattleStats(clone);
            clone.UnitTurnState = new UnitTurnState(clone);
            clone.GridPosition = new GridPosition(clone);
            clone.MoveActions = new MoveActions(clone);
            clone.GameTransform = new GameTransform();
            clone.Buffs = new List<Buff>();
            clone.Debuffs = new List<Debuff>();
            clone.Agent = new AIAgent();

            clone.hp = this.hp;
            clone.sp = this.sp;
            clone.ap = this.ap;
          
            clone.Stats = (Stats)Stats.Clone();
            clone.Growths = (Growths)Growths.Clone();
            clone.Motivation = Motivation;
            //Only for
        }

        #region Events


        public static event Action HpValueChanged;
        public static event Action SpValueChanged;
        public static event Action ApValueChanged;

        public delegate void OnUnitWaiting(Unit unit, bool waiting);

        public static OnUnitWaiting UnitWaiting;

        public delegate void OnUnitCanMove(Unit unit, bool canMove);

        public OnUnitCanMove UnitCanMove;

        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect OnUnitActiveStateUpdated;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        #endregion
    }
}