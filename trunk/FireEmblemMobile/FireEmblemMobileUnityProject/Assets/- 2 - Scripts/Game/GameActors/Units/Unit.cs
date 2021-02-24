using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units.Attributes;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GameResources;
using Game.Graphics;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units
{
    public abstract class Unit : ScriptableObject, IGridActor, IBattleActor, ICloneable, IAIAgent
    {
        public new string name;

        [HideInInspector] private int hp;
        [HideInInspector] private int sp;

        [SerializeField]
        private Stats stats;
        [SerializeField]
        private Growths growths;
        [SerializeField]
        private MoveType moveType;

        public MoveType MoveType
        {
            get => moveType;
            set => moveType = value;
        }
        public Stats Stats
        {
            get => stats;
            set => stats = value;
        }
        public Growths Growths
        {
            get => growths;
            set => growths = value;
        }

        [SerializeField] public UnitVisual visuals;

        [SerializeField] public List<IUnitEffectVisual> unitEffectVisuals;
        
        public GameTransformManager GameTransformManager { get; set; }

        public ExperienceManager ExperienceManager { get; private set; }

        public BattleComponent BattleComponent { get; set; }

        public GridComponent GridComponent { get; set; }
        
        public TurnStateManager TurnStateManager { get; set; }

        public StatusEffectManager StatusEffectManager { get; set; }
        
        public AIComponent AIComponent { get; private set; }
        public Faction Faction { get; set; }
        public Motivation Motivation { get; internal set; }

        public int Hp
        {
            get => hp;
            set
            {
                hp = value > stats.MaxHp ? stats.MaxHp : value;

                if (hp <= 0) hp = 0;
                HpValueChanged?.Invoke();
            }
        }

        public int Sp
        {
            get => sp;
            set
            {
                sp = value > stats.MaxSp ? stats.MaxSp : value;
                if (sp <= 0) sp = 0;
                SpValueChanged?.Invoke();
            }
        }

        public IEnumerable<int> AttackRanges => stats.AttackRanges;
        public int MovementRange => stats.Mov;
        

        void OnDestroy()
        {
            ExperienceManager.LevelUp -= LevelUp;
        }

        public void Initialize()
        {
            ExperienceManager = new ExperienceManager();
            ExperienceManager.LevelUp += LevelUp;
            TurnStateManager = new TurnStateManager(this);
            GridComponent = new GridComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            stats = stats == null ? CreateInstance<Stats>() : Instantiate(stats);
            growths = growths == null ? CreateInstance<Growths>() : Instantiate(growths);
            var tmp = unitEffectVisuals.Select(unitEffectVisual => Instantiate(unitEffectVisual)).ToList();
            unitEffectVisuals = tmp;
            Hp = stats.MaxHp;
            Sp = stats.MaxSp;
            ExperienceManager.ExpGained += ExpGained;
        }

        private void ExpGained(int expBefore, int expGained)
        {
            OnExpGained?.Invoke(this, expBefore, expGained);
        }

        private void LevelUp()
        {
            OnLevelUp?.Invoke(this);
        }

      

        public bool IsDragable()
        {
            return !TurnStateManager.IsWaiting && IsAlive() &&
                   Faction.Id == GridGameManager.Instance.FactionManager.ActivePlayerNumber;
        }
       

        public void SetAttackTarget(bool selected)
        {
            foreach (var unitEffectVisual in unitEffectVisuals)
            {
                if (selected)
                    unitEffectVisual.ShowAttackable(this);
                else
                    unitEffectVisual.HideAttackable();
            }
        }

        public void Die()
        {
            UnitDied(this);
            GameTransformManager.Destroy();
        }

        public void Heal(int heal)
        {
            Hp += heal;
        }

        public bool IsAlive()
        {
            return Hp > 0;
        }

        public bool IsEnemy(IGridActor unit)
        {
            return Faction.Id != unit.Faction.Id;
        }

        public override string ToString()
        {
            return name + " HP: " + Hp + "/" + stats.MaxHp;
        }

        public object Clone()
        {
            var clone = (Unit) MemberwiseClone();
            HandleCloned(clone);
            return clone;
        }
        
        protected virtual void HandleCloned(Unit clone)
        {
            clone.ExperienceManager = new ExperienceManager();
            clone.BattleComponent = new BattleComponent(clone);
            clone.TurnStateManager = new TurnStateManager(clone);
            clone.GridComponent = new GridComponent(clone);
            clone.GameTransformManager = new GameTransformManager();
            clone.StatusEffectManager = new StatusEffectManager(clone);
            clone.AIComponent = new AIComponent();
            clone.unitEffectVisuals = unitEffectVisuals;
            clone.visuals = visuals;
        
            clone.hp = hp;
            clone.sp = sp;
        
            clone.Stats = (Stats) Stats.Clone();
            clone.Growths = (Growths) Growths.Clone();
            clone.Motivation = Motivation;
            clone.MoveType = MoveType;
            clone.Faction = Faction;
            //Only for
        }

        #region Events

        public static event Action HpValueChanged;
        public static event Action SpValueChanged;



        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect OnUnitActiveStateUpdated;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        #endregion
        



        public delegate void OnExpGainedEvent(Unit unit, int expBefore, int expGained);

        public static event OnExpGainedEvent OnExpGained;

        public delegate void OnUnitDamagedEvent(Unit unit, int damage);

        public static event OnUnitDamagedEvent OnUnitDamaged;
        public delegate void LevelupEvent(Unit unit);
        public LevelupEvent OnLevelUp;
    }
}