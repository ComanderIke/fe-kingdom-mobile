using System;
using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.Grid;
using Game.Manager;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units
{
    [Serializable]
    public class Unit : IActor, IGridActor, IBattleActor, ICloneable, IAIAgent, IDialogActor, IEquatable<Unit>
    {
        public static event Action OnEquippedWeapon;
        public static event Action<Unit> OnUnitDataChanged;
        public static event Action<Relic> OnEquippedRelic1;
        public static event Action<Relic> OnEquippedRelic2;
        
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

        public static OnUnitDamagedEvent OnUnitDamaged;
        public static OnUnitHealedEvent OnUnitHealed;
        public delegate void LevelupEvent(Unit unit);
        public LevelupEvent OnLevelUp;
        #endregion
        public UnitBP unitBP;
        public string bluePrintID;
        [FormerlySerializedAs("equippedWeaponBp")] [FormerlySerializedAs("EquippedWeapon")] public Weapon equippedWeapon;
        public Relic EquippedRelic1;
        public Relic EquippedRelic2;
        public string name;
        [HideInInspector] private int hp=-1;
        
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

        public Party Party { get; set; }
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
        public RpgClass rpgClass;
        [SerializeField] public UnitVisual visuals;

        public Unit(string name,RpgClass rpgClass,Stats stats, Attributes growths, MoveType moveType, Weapon equippedWeapon, Relic equippedRelic1,
            Relic equippedRelic2, UnitVisual visuals, SkillManager skillManager, ExperienceManager experienceManager)
        {
            Fielded = false;
            this.rpgClass = rpgClass;
            this.stats = stats;
            this.growths = growths;
            this.moveType = moveType;
            this.equippedWeapon = equippedWeapon;
            EquippedRelic1 = equippedRelic1;
            EquippedRelic2 = equippedRelic2;
            this.visuals = visuals;
            this.name = name;
            Debug.Log("Create: "+name);
            Debug.Log("UnitVisual: "+visuals);
            Debug.Log("CSS: "+visuals.CharacterSpriteSet);
            Debug.Log("FS: "+visuals.CharacterSpriteSet.FaceSprite);
            SkillManager = skillManager;
            SkillManager.SkillPointsUpdated += SkillPointsUpdated;
            SkillManager.Init();
            ExperienceManager = experienceManager;
            ExperienceManager.LevelUp = null;
            ExperienceManager.LevelUp += LevelUp;
            ExperienceManager.ExpGained = null;
            TurnStateManager = new TurnStateManager();
            GridComponent = new GridActorComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            MaxHp = stats.Attributes.CON*Attributes.CON_HP_Mult;
            hp = MaxHp;
            Stats.AttackRanges.Clear();
            if (equippedWeapon != null)
            {
                foreach (int r in equippedWeapon.AttackRanges)
                    Stats.AttackRanges.Add(r);
            }
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
        

  
        public TurnStateManager TurnStateManager { get; set; }

        public StatusEffectManager StatusEffectManager { get; set; }


        public AIComponent AIComponent { get; private set; }


        public Faction Faction { get; set; }
        public List<int> AttackRanges => stats.AttackRanges;
        public int MovementRange => stats.Mov;
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
        

        
        void OnDestroy()
        {
            ExperienceManager.LevelUp -= LevelUp;
        }
        
        void SkillPointsUpdated(int skillPoints)
        {
            Debug.Log("SkillPoints changed");
            OnUnitDataChanged?.Invoke(this);
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
            UnitDied?.Invoke(this);
            
            Faction?.RemoveUnit(this);
            GameTransformManager?.Die();
            
        }
        
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
            int dmg=damage-BattleComponent.BattleStats.GetFaithResistance();
            Debug.Log("TODO Crits and EFF Dmg!");
            Hp -= dmg;
            OnUnitDamaged?.Invoke(this,dmg, DamageType.Magic,false, false);
           
        }

        public string Name => name;

        public Sprite FaceSprite => visuals.CharacterSpriteSet.FaceSprite;
        public bool Fielded { get; set; }
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
            return EquippedRelic1 == item || (EquippedRelic2 == item || equippedWeapon == item);
        }

        public void UnEquip(EquipableItem item)
        {
            if (equippedWeapon == item)
            {
                Stats.AttackRanges.Clear();
                equippedWeapon = null;
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
            if (EquippedRelic1 == null)
            {
                EquippedRelic1 = r;
                OnEquippedRelic1?.Invoke(r);
            }
            else if (EquippedRelic2 == null)
            {
                EquippedRelic2 = r;
                OnEquippedRelic2?.Invoke(r);
               
            }
            else return;
           
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
            clone.EquippedRelic1= (Relic)EquippedRelic1?.Clone();
            clone.EquippedRelic2=(Relic)EquippedRelic2?.Clone();
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
    }
}