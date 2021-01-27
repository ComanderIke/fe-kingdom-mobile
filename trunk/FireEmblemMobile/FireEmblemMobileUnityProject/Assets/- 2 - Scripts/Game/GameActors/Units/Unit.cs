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
    public abstract class Unit : ScriptableObject, IGridActor, IBattleActor
    {
        public new string name;

        [HideInInspector] private int hp;
        [HideInInspector] private int sp;

        public Stats Stats;
        public Growths Growths;
        public MoveType MoveType;

        [SerializeField] public UnitVisual visuals;

        [SerializeField] public List<IUnitEffectVisual> unitEffectVisuals;
        
        public GameTransformManager GameTransformManager { get; set; }

        public ExperienceManager ExperienceManager { get; private set; }

        public BattleComponent BattleComponent { get; set; }

        public GridComponent GridComponent { get; set; }

 

        public TurnStateManager TurnStateManager { get; set; }

        public StatusEffectManager StatusEffectManager { get; set; }

   

        public AIAgent Agent { get; private set; }
        public Faction Faction { get; set; }
        public Motivation Motivation { get; internal set; }

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

            Agent = new AIAgent();
            Stats = Stats == null ? CreateInstance<Stats>() : Instantiate(Stats);
            Growths = Growths == null ? CreateInstance<Growths>() : Instantiate(Growths);
            var tmp = unitEffectVisuals.Select(unitEffectVisual => Instantiate(unitEffectVisual)).ToList();
            unitEffectVisuals = tmp;
            Hp = Stats.MaxHp;
            Sp = Stats.MaxSp;
            ExperienceManager.ExpGained += ExpGained;
        }

        private void ExpGained(int expBefore, int expGained)
        {
            OnExpGained?.Invoke(this, expBefore, expGained);
        }

        private void LevelUp(int levelBefore, int levelAfter)
        {
            int[] statIncreases = CalculateStatIncreases();
            OnUnitLevelUp?.Invoke(name, levelBefore, levelAfter, Stats.GetStatArray(), statIncreases);
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

        public bool IsDragable()
        {
            return !TurnStateManager.IsWaiting && IsAlive() &&
                   Faction.Id == GridGameManager.Instance.FactionManager.ActivePlayerNumber;
        }
        private int Method(int Growth)
        {
            int rngNumber = (int) (UnityEngine.Random.value * 100f);
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
            return Faction.Id != unit.FactionId;
        }

        public override string ToString()
        {
            return name + " HP: " + Hp + "/" + Stats.MaxHp;
        }

        // public object Clone()
        // {
        //     var clone = (Unit) MemberwiseClone();
        //     HandleCloned(clone);
        //     return clone;
        // }
        //
        // protected virtual void HandleCloned(Unit clone)
        // {
        //     clone.ExperienceManager = new ExperienceManager();
        //     clone.BattleComponent = new BattleComponent(clone);
        //     clone.TurnStateManager = new TurnStateManager(clone);
        //     clone.GridComponent = new GridComponent(clone);
        //     clone.GameTransformManager = new GameTransformManager();
        //     clone.StatusEffectManager = new StatusEffectManager(clone);
        //     clone.Agent = new AIAgent();
        //
        //     clone.hp = hp;
        //     clone.sp = sp;
        //
        //     clone.Stats = (Stats) Stats.Clone();
        //     clone.Growths = (Growths) Growths.Clone();
        //     clone.Motivation = Motivation;
        //     //Only for
        // }

        #region Events

        public static event Action HpValueChanged;
        public static event Action SpValueChanged;



        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect OnUnitActiveStateUpdated;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        #endregion

        public delegate void OnUnitLevelUpEvent(string name, int levelBefore, int levelAfter, int[] stats,
            int[] statIncreases);

        public static OnUnitLevelUpEvent OnUnitLevelUp;

        public delegate void OnExpGainedEvent(Unit unit, int expBefore, int expGained);

        public static event OnExpGainedEvent OnExpGained;

        public delegate void OnUnitDamagedEvent(Unit unit, int damage);

        public static event OnUnitDamagedEvent OnUnitDamaged;
    }

    public class VisualComponent
    {
    }
}