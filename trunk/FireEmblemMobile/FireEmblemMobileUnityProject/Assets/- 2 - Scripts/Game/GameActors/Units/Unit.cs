using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
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
    public abstract class Unit : ScriptableObject, IActor, IGridActor, IBattleActor, ICloneable, IAIAgent
    {
        public new string name;
        public string jobClass;
        [HideInInspector] private int hp;
        // [HideInInspector] private int sp;
        // [HideInInspector] private int spBars;
        // private const int SP_PER_BAR = 5;
        [SerializeField]
        private Stats stats;
        [SerializeField]
        private Attributes growths;
        [SerializeField]
        private MoveType moveType;
        [SerializeField]
        public SkillManager SkillManager;
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
        public Attributes Growths
        {
            get => growths;
            set => growths = value;
        }

        [SerializeField] public UnitVisual visuals;

   
        
        public GameTransformManager GameTransformManager { get; set; }

        [SerializeField] private ExperienceManager experienceManager;

        public ExperienceManager ExperienceManager
        {
            get =>  experienceManager;
            set => experienceManager = value;
        }

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


        // public int SpBars
        // {
        //     get => spBars;
        //     set
        //     {
        //         spBars = value > stats.MaxSp/SP_PER_BAR ? stats.MaxSp/SP_PER_BAR : value;
        //
        //         if (spBars <= 0)
        //         {
        //             spBars = 0;
        //            
        //                 visuals.UnitEffectVisual.ShowNoStamina(this);
        //             
        //         }
        //         else
        //         {
        //             
        //             visuals.UnitEffectVisual.HideNoStamina();
        //             
        //         }
        //         SpBarsValueChanged?.Invoke();
        //     }
        // }

        // public int Sp
        // {
        //     get => sp;
        //     set
        //     {
        //         sp = value > stats.MaxSp ? stats.MaxSp : value;
        //         if (sp <= 0) sp = 0;
        //         SpValueChanged?.Invoke();
        //     }
        // }

        public List<int> AttackRanges => stats.AttackRanges;
        public int MovementRange => stats.Mov;
        // public int MaxSpBars
        // {
        //     get => stats.MaxSp / SP_PER_BAR;
        // }
        //

        void OnDestroy()
        {
            ExperienceManager.LevelUp -= LevelUp;
        }

        


        public virtual void Initialize()
        {

            SkillManager = new SkillManager();
            experienceManager ??= new ExperienceManager();
            ExperienceManager.LevelUp = null;
            ExperienceManager.LevelUp += LevelUp;
            TurnStateManager ??= new TurnStateManager(this);
            GridComponent = new GridComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            // stats = stats == null ? CreateInstance<Stats>() : Instantiate(stats);
            // growths = growths == null ? CreateInstance<Growths>() : Instantiate(growths);
            if (visuals.UnitEffectVisual != null)
            {
                visuals.UnitEffectVisual = Instantiate(visuals.UnitEffectVisual);
            }

            stats.Initialize();
           
            hp = stats.MaxHp;
            // sp = stats.MaxSp;
            // spBars = Sp / SP_PER_BAR;
            ExperienceManager.ExpGained = null;
            ExperienceManager.ExpGained += ExpGained;
           
        }

        private void ExpGained(Vector3 drainPos, int expBefore, int expGained)
        {
            Debug.Log("Unit Exp Gained!" +expBefore+" "+expGained);
            OnExpGained?.Invoke(this, expBefore, expGained);
            GameObject.FindObjectOfType<DeathParticleController>().Play(this, drainPos+new Vector3(0.5f,0.5f,0), expGained);
        }

        private void LevelUp()
        {
            OnLevelUp?.Invoke(this);
        }

      

        public bool IsDragable()
        {
            return !TurnStateManager.IsWaiting && IsAlive() && GridGameManager.Instance.FactionManager.IsActiveFaction(Faction);
        }
       

        public void SetAttackTarget(bool selected)
        {
        
                if (selected)
                    visuals.UnitEffectVisual.ShowAttackable(this);
                else
                    visuals.UnitEffectVisual.HideAttackable();
          
        }

        public void Die()
        {
            UnitDied(this);
            Faction.RemoveUnit(this);
            GameTransformManager.Die();
            
        }

      
        public Tile GetTile()
        {
            return GridComponent.Tile;
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
            clone.experienceManager = new ExperienceManager();
            clone.BattleComponent = new BattleComponent(clone);
            clone.TurnStateManager = new TurnStateManager(clone);
            clone.GridComponent = new GridComponent(clone);
            clone.GridComponent.GridPosition =
                new GridPosition(GridComponent.GridPosition.X, GridComponent.GridPosition.Y);
            clone.GridComponent.Tile = GridComponent.Tile;
           // clone.GridComponent.previousTile = GridComponent.previousTile;
            clone.GameTransformManager = new GameTransformManager();
            clone.StatusEffectManager = new StatusEffectManager(clone);
            clone.AIComponent = new AIComponent();
            clone.visuals.UnitEffectVisual = visuals.UnitEffectVisual;
            clone.visuals = visuals;
            clone.SkillManager = new SkillManager(clone.SkillManager);
            clone.hp = hp;
            //clone.sp = sp;
        
            clone.Stats = (Stats) Stats.Clone();
            clone.Growths = (Attributes) Growths.Clone();
            clone.Motivation = Motivation;
            clone.MoveType = MoveType;
            clone.Faction = Faction;
            //Only for
        }

        #region Events

        public static event Action HpValueChanged;
        public static event Action SpValueChanged;
        public static event Action SpBarsValueChanged;




        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect OnUnitActiveStateUpdated;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        #endregion
        



        public delegate void OnExpGainedEvent(Unit unit, int expBefore, int expGained);

        public static event OnExpGainedEvent OnExpGained;

        public delegate void OnUnitDamagedEvent(Unit unit, int damage,bool magic, bool crit, bool eff);

        public static OnUnitDamagedEvent OnUnitDamaged;
        public delegate void LevelupEvent(Unit unit);
        public LevelupEvent OnLevelUp;

        // public void OnEnable()
        // {
        //     Initialize();
        // }
    }
}