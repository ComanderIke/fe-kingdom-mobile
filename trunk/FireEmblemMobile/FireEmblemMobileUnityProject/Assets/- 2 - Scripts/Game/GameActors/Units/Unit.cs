using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
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
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Human", fileName = "Human")]
    public class Unit : ScriptableObject, IActor, IGridActor, IBattleActor, ICloneable, IAIAgent
    {
        public static event Action OnEquippedWeapon;
        public RpgClass Class;

        public Weapon EquippedWeapon;
        public Relic EquippedRelic1;
        public Relic EquippedRelic2;
        public new string name;
        public string jobClass;
        [HideInInspector] private int hp=-1;
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
        public string bluePrintID;
        public MoveType MoveType
        {
            get => moveType;
            set => moveType = value;
        }
        [HideInInspector][SerializeField]
        public int MaxHp { get; set; }
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

        public GridActorComponent GetActorGridComponent()
        {
            return (GridActorComponent)GridComponent;
        }
        


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
              
                hp = value > MaxHp ? MaxHp : value;
              
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
 
            experienceManager ??= new ExperienceManager();
            ExperienceManager.LevelUp = null;
            ExperienceManager.LevelUp += LevelUp;
            TurnStateManager ??= new TurnStateManager();
            GridComponent = new GridActorComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            // stats = stats == null ? CreateInstance<Stats>() : Instantiate(stats);
            // growths = growths == null ? CreateInstance<Growths>() : Instantiate(growths);
            // if (visuals.UnitEffectVisual != null)
            // {
            //     visuals.UnitEffectVisual = Instantiate(visuals.UnitEffectVisual);
            // }

           
            MaxHp = stats.Attributes.CON*Attributes.CON_HP_Mult;
            
            if(hp==-1)//hp has never been set
                hp = MaxHp;
            // sp = stats.MaxSp;
            // spBars = Sp / SP_PER_BAR;
            ExperienceManager.ExpGained = null;
            ExperienceManager.ExpGained += ExpGained;
            Stats.AttackRanges.Clear();
            if (EquippedWeapon != null)
            {
                foreach (int r in EquippedWeapon.AttackRanges)
                    Stats.AttackRanges.Add(r);
            }
           
        }

        

        private void ExpGained(Vector3 drainPos, int expBefore, int expGained)
        {
            Debug.Log("Unit Exp Gained!" +expBefore+" "+expGained);
            OnExpGained?.Invoke(this, expBefore, expGained);
            GameObject.FindObjectOfType<ExpParticleSystem>().Play(this, drainPos+new Vector3(0.5f,0.5f,0), expGained);
        }

        private void LevelUp()
        {
            Debug.Log("On LevelUp Called!");
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
            Debug.Log("Die: " + name);
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
            OnUnitHealed?.Invoke(this, heal);
            
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
            return name + " HP: " + Hp + "/" + MaxHp+"Level: "+experienceManager.Level+ " Exp: "+experienceManager.Exp;
        }

        public Weapon GetEquippedWeapon()
        {
            return EquippedWeapon;
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
            clone.TurnStateManager = new TurnStateManager();
            clone.GridComponent = new GridActorComponent(clone);
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
            clone.EquippedWeapon = (Weapon)EquippedWeapon?.Clone();
            clone.EquippedRelic1= (Relic)EquippedRelic1?.Clone();
            clone.EquippedRelic2=(Relic)EquippedRelic2?.Clone();
            //human.Inventory = (Inventory)Inventory.Clone();
            clone.Class = Class;
            //Only for
        }
        public bool CanUseWeapon(Weapon w)
        {
            return true;
        }
         public void Equip(EquipableItem e)
        {

            switch (e.EquipmentSlotType)
            {
                //case EquipmentSlotType.Armor: Debug.LogError("TODO Equip Armor!"); break;
                case EquipmentSlotType.Weapon: 
                    Equip((Weapon) e);break;
                case EquipmentSlotType.Relic: Debug.LogError("TODO Equip Relic!"); break;
            }
        }
        public void Equip(Weapon w)
        {
            
            Stats.AttackRanges.Clear();
            EquippedWeapon = w;
            foreach (int r in w.AttackRanges) Stats.AttackRanges.Add(r);
            Debug.Log("Equip " + w.name + " on " + name + " " + w.AttackRanges.Length+" "+ Stats.AttackRanges.Count);
            OnEquippedWeapon?.Invoke();
        }
        public void AutoEquip()
        {
           
            // if (EquippedWeapon == null)
            // {
            //     Equip((Weapon)Inventory.Items.First(a=> a is Weapon weapon && CanUseWeapon(weapon)));
            // }
        }

        public bool CanEquip(EquipableItem eitem)
        {
            Debug.Log("TODO Check if item is equipable");
            return true;
        }

        public void InitEquipment()
        {
            if(EquippedWeapon!=null)
                EquippedWeapon=Instantiate(EquippedWeapon);
            if(EquippedRelic1!=null)
                EquippedRelic1=Instantiate(EquippedRelic1);
            if(EquippedRelic2!=null)
                EquippedRelic2=Instantiate(EquippedRelic2);
        }

        public bool HasEquipped(Item item)
        {
            return EquippedRelic1 == item || (EquippedRelic2 == item || EquippedWeapon == item);
        }

        public void UnEquip(EquipableItem item)
        {
            if (EquippedWeapon == item)
            {
                Stats.AttackRanges.Clear();
                EquippedWeapon = null;
            }
            else if (EquippedRelic1 == item)
            {
                EquippedRelic1 = null;
            }
            else if (EquippedRelic2 == item)
            {
                EquippedRelic2 = null;
            }
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

        public delegate void OnUnitDamagedEvent(Unit unit, int damage,DamageType damageType, bool crit, bool eff);
        public delegate void OnUnitHealedEvent(Unit unit, int damage);

        public static OnUnitDamagedEvent OnUnitDamaged;
        public static OnUnitHealedEvent OnUnitHealed;
        public delegate void LevelupEvent(Unit unit);
        public LevelupEvent OnLevelUp;
    

        // public void OnEnable()
        // {
        //     Initialize();
        // }
        public void InflictFaithDamage(Unit attacker)
        {
            int dmg=attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(this);
            
            Hp -= dmg;
            Debug.Log("TODO Crits and EFF Dmg!");
            OnUnitDamaged?.Invoke(this,dmg, DamageType.Faith,false, false);

        }
        public void InflictFixedFaithDamage(int damage)
        {
            int dmg=damage-BattleComponent.BattleStats.GetFaithResistance();
            
            Hp -= dmg;
            Debug.Log("TODO Crits and EFF Dmg!");
            OnUnitDamaged?.Invoke(this,dmg, DamageType.Faith,false, false);

        }
        public void InflictMagicDamage(Unit attacker)
        {
            int dmg=attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(this);
            Debug.Log("TODO Crits and EFF Dmg!");
            Hp -= dmg;
            OnUnitDamaged?.Invoke(this,dmg, DamageType.Magic,false, false);
           
        }
        public void InflictFixedMagicDamage(int damage)
        {
            int dmg=damage-BattleComponent.BattleStats.GetMagicResistance();
            Debug.Log("TODO Crits and EFF Dmg!");
            Hp -= dmg;
            OnUnitDamaged?.Invoke(this,dmg, DamageType.Magic,false, false);
           
        }
    }
}