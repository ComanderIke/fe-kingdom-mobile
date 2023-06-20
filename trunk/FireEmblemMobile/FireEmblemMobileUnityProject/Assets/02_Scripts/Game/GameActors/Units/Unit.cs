using System;
using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.GameActors.Units
{
    [Serializable]
    public class Unit : IActor, IGridActor, IBattleActor, ICloneable, IAIAgent, IDialogActor, IEquatable<Unit>
    {
        public static event Action OnEquippedWeapon;
        public static event Action<Unit> OnUnitDataChanged;
        public static event Action<Relic> OnEquippedRelic;
        public static event Action<Relic> OnUnequippedRelic;

        #region Events

        public event Action HpValueChanged;

        public delegate void OnUnitShowActiveEffect(Unit unit, bool canMove, bool disableOthers);

        public static OnUnitShowActiveEffect OnUnitActiveStateUpdated;

        public delegate void OnUnitDied(Unit unit);

        public static OnUnitDied UnitDied;

        // public delegate void OnExpGainedEvent(Unit unit, Vector3 drainPos, int expBefore, int expGained);
        //
        // public static event OnExpGainedEvent OnExpGained;

        public delegate void OnUnitDamagedEvent(Unit unit, int damage,DamageType damageType, bool crit, bool eff);
        public delegate void OnUnitHealedEvent(Unit unit, int damage);

        public delegate void OnUnitStatsChanged(Unit unit);
        public static event Action<Unit, int> OnDealingDamage;
        public static OnUnitDamagedEvent OnUnitDamaged;
        public static OnUnitHealedEvent OnUnitHealed;
        public delegate void LevelupEvent(Unit unit);
        public static LevelupEvent OnLevelUp;
        #endregion
        [NonSerialized]public UnitBP unitBP;
        public string bluePrintID;
        [NonSerialized]public Weapon equippedWeapon;
        [NonSerialized]
        public Relic EquippedRelic;
        [NonSerialized]
        public StockedItem CombatItem1;
        [NonSerialized]
        public StockedItem CombatItem2;
     
        public string name;
        [HideInInspector] private int hp=-1;
        [NonSerialized]
        private Blessing blessing;
        [NonSerialized]
        private Curse curse;

        // private int maxBlessings = 3;
        // private int maxCurses = 3;
        [NonSerialized]
        private Stats stats;
        [SerializeField]
        private Attributes growths;
        [SerializeField]
        private MoveType moveType;
        [NonSerialized]
        public SkillManager SkillManager;
        private List<EncounterBasedBuff> encounterBuffs;
        public MoveType MoveType
        {
            get => moveType;
            set => moveType = value;
        }

        [field:NonSerialized]public Party Party { get; set; }
        [HideInInspector][SerializeField]
        public int MaxHp { get; set; }
        public Stats Stats
        {
            get => stats;
            set => stats = value;
        }

        public Blessing Blessing => blessing;
        public Curse Curse => curse;

        public Attributes Growths
        {
            get => growths;
            set => growths = value;
        }
        public RpgClass rpgClass;
        [NonSerialized] public UnitVisual visuals;

        public UnitVisual Visuals
        {
            get { return visuals; }
        }

        public Unit(string bluePrintID, string name,RpgClass rpgClass,Stats stats, Attributes growths, MoveType moveType, Weapon equippedWeapon, Relic equippedRelic,
            StockedItem combatItem1, StockedItem combatItem2, UnitVisual visuals, SkillManager skillManager, ExperienceManager experienceManager)
        {
            this.bluePrintID = bluePrintID;
            Fielded = false;
            encounterBuffs = new List<EncounterBasedBuff>();
            this.rpgClass = rpgClass;
            this.stats = stats;
            this.growths = growths;
            this.moveType = moveType;
            this.equippedWeapon = equippedWeapon;
            if (equippedRelic == null)
            {
                EquippedRelic = null;
            }
            else
            {
                EquippedRelic = equippedRelic;
            }

            if (combatItem1 != null && combatItem1.item != null)
            {
                CombatItem1 = combatItem1;
            }
            else
            {
                CombatItem1 = null;
            }
            if (combatItem2 != null && combatItem2.item != null)
            {
                CombatItem2 = combatItem2;
            }
            else
            {
                CombatItem2 = null;
            }

            
            this.visuals = visuals;
            this.name = name;
            SkillManager = skillManager;
            SkillManager.SkillPointsUpdated += SkillPointsUpdated;
            SkillManager.Init();
            ExperienceManager = experienceManager;
            InitExperienceManager();
            TurnStateManager = new TurnStateManager();
            GridComponent = new GridActorComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            MaxHp = Attributes.BASE_HP+stats.BaseAttributes.CON*Attributes.CON_HP_Mult;
            hp = MaxHp;
            Stats.AttackRanges.Clear();
            if (equippedWeapon != null)
            {
                foreach (int r in equippedWeapon.AttackRanges)
                    Stats.AttackRanges.Add(r);
            }

        }

        public void InitExperienceManager()
        {
            ExperienceManager.LevelUp = null;
            ExperienceManager.LevelUp += LevelUp;
            ExperienceManager.ExpGained = null;
            ExperienceManager.ExpGained += ExpGained;
        }

        void ExpGained(int expBefore, int expGained)
        {
            OnExpGained?.Invoke(this, expGained);
        }
        public GameTransformManager GameTransformManager { get; set; }
        public AnimatedCombatCharacter BattleGO { get; set; }
        


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
        

  
        
        [field:NonSerialized] public TurnStateManager TurnStateManager { get; set; }

        [field:NonSerialized]  public StatusEffectManager StatusEffectManager { get; set; }


        [field:NonSerialized]  public AIComponent AIComponent { get; private set; }


        [field:NonSerialized] public Faction Faction { get; set; }
        public List<int> AttackRanges => stats.AttackRanges;
        public int MovementRange => stats.Mov;
        public int Hp
        {
            get => hp;
            set
            {
              
                hp = value > MaxHp ? MaxHp : value;
              
                if (hp <= 0) hp = 0;
                Debug.Log("HP VALUE CHANGED ON UNIT: "+name);
                HpValueChanged?.Invoke();
            }
        }
        

        
        
        void SkillPointsUpdated(int skillPoints)
        {
          
            OnUnitDataChanged?.Invoke(this);
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
            Debug.Log("Die: " + name);
            UnitDied?.Invoke(this);
            
            Faction?.RemoveUnit(this);
            GameTransformManager?.Die();
            
        }

        public void InflictDirectDamage(Unit attacker, int dmg, DamageType damageType, bool callDamagedEvent=true)
        {
            Hp -= dmg;
            OnDealingDamage?.Invoke(attacker, dmg);
            if(callDamagedEvent)
                OnUnitDamaged?.Invoke(this,dmg, damageType,false, false);
        }
        public void InflictDamage(Unit attacker, DamageType damageType)
        {
            int dmg=attacker.BattleComponent.BattleStats.GetDamageAgainstTarget(this);
            
            Hp -= dmg;
            Debug.Log("TODO Crits and EFF Dmg!");
            OnDealingDamage?.Invoke(attacker, dmg);
            OnUnitDamaged?.Invoke(this,dmg, damageType,false, false);

        }
        public void InflictFixedDamage(Unit attacker,int damage, DamageType damageType)
        {
            int defense = 0;
            switch (damageType)
            {
                case DamageType.Faith:defense=BattleComponent.BattleStats.GetFaithResistance();
                    break;
                case DamageType.Magic:defense=BattleComponent.BattleStats.GetFaithResistance();
                    break;
                case DamageType.Physical:defense=BattleComponent.BattleStats.GetPhysicalResistance();
                    break;
            }
            int dmg = damage - defense;
            Hp -= dmg;
            Debug.Log("TODO Crits and EFF Dmg!");
            OnDealingDamage?.Invoke(attacker, dmg);
            OnUnitDamaged?.Invoke(this,dmg, damageType,false, false);

        }
       
        public void InflictNonLethalTrueDamage(Unit attacker,int damage)
        {
            //Debug.Log("Inflict True Damage: "+damage);
            if (damage >= Hp)
                damage = Hp - 1;
            Hp -= damage;
            OnDealingDamage?.Invoke(attacker, damage);
            OnUnitDamaged?.Invoke(this,damage, DamageType.True,false, false);
        }

        public string Name => name;

        public Sprite FaceSprite => visuals.CharacterSpriteSet.FaceSprite;
        public bool Fielded { get; set; }
        public float HealingMultiplier { get; set; }
        public float BonusSkillProcChance { get; set; }
        public bool ClassUpgraded { get; set; }


        public void Equip(Weapon w)
        {
            
            Stats.AttackRanges.Clear();
            equippedWeapon = w;
            foreach (int r in w.AttackRanges) Stats.AttackRanges.Add(r);
            Debug.Log("Equip " + w.Name + " on " + name + " " + w.AttackRanges.Length+" "+ Stats.AttackRanges.Count);
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
        

        public bool HasEquipped(Item item)
        {
            return EquippedRelic == item || equippedWeapon == item;
        }

       
        public void UnEquip(EquipableItem item)
        {
            if (item == null)
                return;
            if (equippedWeapon == item)
            {
                Stats.AttackRanges.Clear();
                equippedWeapon = null;
            }
            else if (EquippedRelic == item)
            {
                
                Debug.Log("Unequip: "+item);
                EquippedRelic = null;
                OnUnequippedRelic?.Invoke((Relic)item);
                
            }
            OnUnitDataChanged?.Invoke(this);
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
                    Equip((Weapon)e);
                    break;
                case EquipmentSlotType.Relic:
                    Equip((Relic)e);
                    break;
            }
           
        }

        public void Equip(Relic r)
        {
            if (EquippedRelic == null)
            {
                EquippedRelic = r;
                OnEquippedRelic?.Invoke(r);
            }
            OnUnitDataChanged?.Invoke(this);
           
        }
        public void Equip(Relic r, int slot)
        {
            if (slot==1)
            {
                EquippedRelic = r;
                OnEquippedRelic?.Invoke(r);
            }
            OnUnitDataChanged?.Invoke(this);
           
        }
        protected virtual void HandleCloned(Unit clone)
        {
            clone.experienceManager = new ExperienceManager(experienceManager);
            clone.BattleComponent = new BattleComponent(clone);
            clone.TurnStateManager = new TurnStateManager();
            clone.GridComponent = new GridActorComponent(clone);
            if (GridComponent!=null&&GridComponent.GridPosition != null)
            {
                clone.GridComponent.GridPosition =
                    new GridPosition(GridComponent.GridPosition.X, GridComponent.GridPosition.Y);
                clone.GridComponent.Tile = GridComponent.Tile;
            }

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
            clone.MoveType = MoveType;
            clone.Faction = Faction;
            clone.equippedWeapon = (Weapon)equippedWeapon?.Clone();
            clone.EquippedRelic= (Relic)EquippedRelic?.Clone();
            if(CombatItem1!=null)
                clone.CombatItem1 = new StockedItem((Item)CombatItem1.item.Clone(),CombatItem1.stock);
            if(CombatItem2!=null)
                clone.CombatItem2 = new StockedItem((Item)CombatItem2.item.Clone(),CombatItem2.stock);
            //human.Inventory = (Inventory)Inventory.Clone();
            //Only for
        }
        public object Clone()
        {
            var clone = (Unit) MemberwiseClone();
            HandleCloned(clone);
            return clone;
        }
        public void Heal(int heal)
        {
            Debug.Log("Unit HEALED: "+ heal);
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
            return name;// + " HP: " + Hp + "/" + MaxHp+"Level: "+experienceManager.Level+ " Exp: "+experienceManager.Exp;
        }
        public Tile GetTile()
        {
            return GridComponent.Tile;
        }

        public bool IsPlayerControlled()
        {
            if(Faction!=null)
                return Faction.IsPlayerControlled;
            else if(Party!=null)
            {
                return true;
            }

            return false;
        }
        public Weapon GetEquippedWeapon()
        {
            return equippedWeapon;
        }


        public bool Equals(Unit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return name == other.name && Equals(Party, other.Party);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Unit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ (Party != null ? Party.GetHashCode() : 0);
            }
        }


        public void ReceiveBlessing(Blessing blessing)
        {
       
                this.blessing=blessing;
                blessing.BlessUnit(this);
            
        }

        public void ReceiveCurse(Curse curse)
            {
               
                this.curse = curse;
                curse.BlessUnit(this);
                
            }

        
        public static event Action<Unit, int> OnExpGained;

        public void RemoveBlessing(Blessing blessing)
        {
            this.blessing = null;
        }
        public void RemoveCurse()
        {
            this.curse = null;
        }

        public void EncounterTick()
        {
            // if(blessing!=null)
            //     blessing.DecreaseDuration();
        }

        
        public void ApplyEncounterBuff(EncounterBasedBuff buff)
        {
            encounterBuffs.Add(buff);
            buff.Apply(this);
            
        }

        

        public void RemoveDebuffs()
        {
            Debug.Log("TODO Remove Debuffs");
        }

        public void RemoveCurseBless(CurseBlessBase curseBlessBase)
        {
            if (curseBlessBase is Curse curse)
            {
                RemoveCurse();
            }
            else  if (curseBlessBase is Blessing blessing)
            {
                RemoveBlessing(blessing);
            }
        }


        public Relic GetRelicSlot(int equipmentControllerSelectedSlotNumber)
        {
            if (equipmentControllerSelectedSlotNumber == 1)
                return EquippedRelic;
            return null;
        }

        public void UnEquipRelic(int equipmentControllerSelectedSlotNumber)
        {
            if (equipmentControllerSelectedSlotNumber == 1)
                UnEquip(EquippedRelic);
        }



    }
}