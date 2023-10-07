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
using Game.GameActors.Units.Skills;
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
        public static event Action<StockedCombatItem> OnUnequippedCombatItem;
        public static event Action<StockedCombatItem> OnEquippedCombatItem;

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
        public static event Action<Unit, int, int> OnExpGained;
        public static LevelupEvent OnLevelUp;
        #endregion
        [NonSerialized]public UnitBP unitBP;
        public string bluePrintID;
        [NonSerialized]public Weapon equippedWeapon;
        [NonSerialized]
        public Relic EquippedRelic;
        [NonSerialized]
        public StockedCombatItem CombatItem1;
        // [NonSerialized]
        // public StockedCombatItem CombatItem2;
     
        public string name;
        [HideInInspector] private int hp=-1;

        // private int maxBlessings = 3;
        // private int maxCurses = 3;
        [NonSerialized]
        private Stats stats;
       
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

        public Blessing Blessing => null;//SkillManager.Skills==null?null:SkillManager.Skills.OfType<Blessing>()?.First();
        public List<Curse> Curses => SkillManager.GetCurses();

       
        public RpgClass rpgClass;
        [NonSerialized] public UnitVisual visuals;

        public UnitVisual Visuals
        {
            get { return visuals; }
        }

        public Unit(string bluePrintID, string name,RpgClass rpgClass,Stats stats, MoveType moveType,
             UnitVisual visuals, SkillManager skillManager, ExperienceManager experienceManager)
        {
            this.bluePrintID = bluePrintID;
            HealingMultipliers = new List<float>();
            Fielded = false;
            encounterBuffs = new List<EncounterBasedBuff>();
            this.rpgClass = rpgClass;
            this.stats = stats;
            this.moveType = moveType;
            this.visuals = visuals;
            this.name = name;
            SkillManager = skillManager;
            SkillManager.SkillPointsUpdated += SkillPointsUpdated;
           
            ExperienceManager = experienceManager;
            InitExperienceManager();
            TurnStateManager = new TurnStateManager();
            GridComponent = new GridActorComponent(this);
            BattleComponent = new BattleComponent(this);
            GameTransformManager = new GameTransformManager();
            StatusEffectManager = new StatusEffectManager(this);
            AIComponent = new AIComponent();
            MaxHp = Attributes.BASE_HP+stats.BaseAttributes.MaxHp*Attributes.CON_HP_Mult;
            tags = new List<UnitTags>();
            hp = MaxHp;
            Stats.AttackRanges.Clear();
            if (equippedWeapon != null)
            {
                foreach (int r in equippedWeapon.AttackRanges)
                    Stats.AttackRanges.Add(r);
            }
            SkillManager.Init(this);

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
            OnExpGained?.Invoke(this, expGained, expBefore);
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

                int tmpHP = hp;
                hp = value > MaxHp ? MaxHp : value;
                if(tmpHP>1&& hp <=0)
                    OnAboutToDie?.Invoke(this);
                if (hp <= 0) hp = 0;
                // Debug.Log("HP VALUE CHANGED ON UNIT: "+name);
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

        public void Die(Unit damageSource)
        {
            Debug.Log("Die: " + name);
            KilledBy = damageSource;
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
        public Unit KilledBy { get; private set; }

        public Sprite FaceSprite => visuals.CharacterSpriteSet.FaceSprite;
        public bool Fielded { get; set; }
      
        public float BonusSkillProcChance { get; set; }
        public bool ClassUpgraded { get; set; }
        public bool IsBoss { get; set; }
        public EncounterComponent EncounterComponent { get; set; }
        public List<float> HealingMultipliers { get; set; }
        public List<int> BonusAttackRanges { get; set; }
        public Bonds Bonds { get; set; }

       
       

        public void Equip(Weapon w)
        {
            if (w == equippedWeapon)
                return;
            if (equippedWeapon != null)
            {
                stats.BonusStatsFromWeapon.Attack -= equippedWeapon.GetDamage();
                stats.BonusStatsFromWeapon.Hit -= equippedWeapon.GetHit();
                stats.BonusStatsFromWeapon.Crit -= equippedWeapon.GetCrit();
            }
            Stats.AttackRanges.Clear();
            equippedWeapon = w;
            foreach (int r in w.AttackRanges) Stats.AttackRanges.Add(r);
            Debug.Log("Equip " + w.Name + " on " + name + " " + w.AttackRanges.Length+" "+ Stats.AttackRanges.Count);
            stats.BonusStatsFromWeapon.Attack += equippedWeapon.GetDamage();
            stats.BonusStatsFromWeapon.Hit += equippedWeapon.GetHit();
            stats.BonusStatsFromWeapon.Crit += equippedWeapon.GetCrit();

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

        public void UnEquip(StockedCombatItem item)
        {
            if (item == null)
                return;
            if (CombatItem1 == item)
            {
                CombatItem1 = null;
                OnUnequippedCombatItem?.Invoke(item);
            }
            // else if (CombatItem2 == item)
            // {
            //     CombatItem2 = null;
            //   
            //     OnUnequippedCombatItem?.Invoke(item);
            //     
            // }
            OnUnitDataChanged?.Invoke(this);
        }
        
        public bool CanUseWeapon(Weapon w)
        {
            return true;
        }

        public void Equip(StockedCombatItem combatItem, int slot, bool triggerEvents = true)
        {
            Debug.Log("Equip Combat Item on Slot: "+slot);
            if (slot==1)
            {
                if (CombatItem1 == null)
                {
                    CombatItem1=combatItem;
                }
                else
                {
                    UnEquip(CombatItem1);
                    CombatItem1 = combatItem;
                }

                if (triggerEvents)
                {
                    OnEquippedCombatItem?.Invoke(CombatItem1);
                }
            }
            // else if (slot==2)
            // {
            //     if (CombatItem2 == null)
            //     {
            //         CombatItem2=combatItem;
            //     }
            //     else
            //     {
            //         UnEquip(CombatItem2);
            //         CombatItem2=combatItem;
            //     }
            //
            //     if (triggerEvents)
            //     {
            //         OnEquippedCombatItem?.Invoke(CombatItem2);
            //     }
            // }

            if (triggerEvents)
            {
                OnUnitDataChanged?.Invoke(this);
            }

        }
      
        public void Equip(Relic r, bool triggerEvents=true)
        {
            if (EquippedRelic == null)
            {
                EquippedRelic = r;
                EquippedRelic.Equip(this);
               
            }
            else
            {
                UnEquipRelic();
                EquippedRelic = r;
                EquippedRelic.Equip(this);
            }

            if (triggerEvents)
            {
                OnEquippedRelic?.Invoke(r);
                OnUnitDataChanged?.Invoke(this);
            }

        }
      
        protected virtual void HandleCloned(Unit clone)
        {
            clone.experienceManager = new ExperienceManager(experienceManager);
            clone.BattleComponent = new BattleComponent(BattleComponent);
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
            clone.MoveType = MoveType;
            clone.Faction = Faction;
            clone.equippedWeapon = (Weapon)equippedWeapon?.Clone();
            clone.EquippedRelic= (Relic)EquippedRelic?.Clone();
            clone.tags = new List<UnitTags>(tags);
            if(CombatItem1!=null)
                clone.CombatItem1 = new StockedCombatItem((IEquipableCombatItem)CombatItem1.item.Clone(),CombatItem1.stock);
            // if(CombatItem2!=null)
            //     clone.CombatItem2 = new StockedCombatItem((IEquipableCombatItem)CombatItem2.item.Clone(),CombatItem2.stock);
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
            Debug.Log(name+" HEALED: "+ heal);
            float tmpHeal = heal;
            foreach (var mult in HealingMultipliers)
            {
                tmpHeal *= mult;
            }

            heal = (int) tmpHeal;
            Hp += heal;
            OnUnitHealed?.Invoke(this, heal);
            
        }

        public bool IsAlive()
        {
            return Hp > 0;
        }

        private void UpdateTerrainBonuses(Tile oldTile, Tile newTile)
        {
            if (oldTile != null)
            {
              
                var oldTileData = oldTile.TileData;
                Stats.BonusStatsFromTerrain.Avoid -= oldTileData.avoBonus;
                Stats.BonusStatsFromTerrain.Armor -= oldTileData.defenseBonus;
                Stats.BonusStatsFromTerrain.MagicResistance -= oldTileData.defenseBonus;
                Stats.BonusStatsFromTerrain.AttackSpeed -= oldTileData.speedMalus;
              //  Debug.Log("RemovedTerrainBonuses: "+ Stats.BonusStatsFromTerrain.Avoid+" "+oldTileData.avoBonus);
            }
            var tileData = newTile.TileData;
            Stats.BonusStatsFromTerrain.Avoid += tileData.avoBonus;
            Stats.BonusStatsFromTerrain.Armor += tileData.defenseBonus;
            Stats.BonusStatsFromTerrain.MagicResistance += tileData.defenseBonus;
            Stats.BonusStatsFromTerrain.AttackSpeed += tileData.speedMalus;
        //    Debug.Log("AddedTerrainBonuses: "+ Stats.BonusStatsFromTerrain.Avoid+" "+tileData.avoBonus);
        }
      
        public void SetGridPosition(Tile newTile, bool moveGameobject=true)//Just use this from GridSystem to avoid not setting old gridobject to null
        {
            
            UpdateTerrainBonuses(GridComponent.Tile, newTile);
            GridComponent.SetPosition(newTile, moveGameobject);
        }

        public void SetToOriginPosition()
        {
            Debug.Log("SetUnitToOriginPosition: "+GridComponent.OriginTile.X+" "+GridComponent.OriginTile.Y);
            UpdateTerrainBonuses(GridComponent.Tile, GridComponent.OriginTile);
            GridComponent.SetToOriginPosition();
        }
        public void SetInternGridPosition(Tile newTile)
        {
            //Debug.Log("Set Intern Position: "+newTile.X+" "+newTile.Y+" OldTile: "+GridComponent.Tile.X+" "+GridComponent.Tile.Y);
            UpdateTerrainBonuses(GridComponent.Tile, newTile);
            GridComponent.SetInternPosition(newTile);
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
            SkillManager.LearnSkill(blessing);
            Debug.Log("TODO Receive Blessing");
        }

        public void ReceiveCurse(Curse curse)
        {
            if(SkillManager.IsFull())
                SkillManager.RemoveRandomSkill();
            SkillManager.LearnSkill(curse);
            Debug.Log("TODO Receive Curse");
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
        


        public Relic GetRelicSlot(int equipmentControllerSelectedSlotNumber)
        {
            if (equipmentControllerSelectedSlotNumber == 1)
                return EquippedRelic;
            return null;
        }

        public void UnEquipRelic()
        {
            var relic = EquippedRelic;
            relic.Unequip(this);
            EquippedRelic = null;
            OnUnequippedRelic?.Invoke(relic);
        }


        public void UnEquipCombatItem(StockedCombatItem getCombatItem)
        {
            if(CombatItem1==getCombatItem)
                UnEquip(CombatItem1);
            // else if(CombatItem2==getCombatItem)
            //     UnEquip(CombatItem2);
        }


        public void RemoveCurse()
        {
            // if(Curse!=null)
            //     SkillManager.RemoveSkill(Curse);
        }

        public event Action<Unit> OnAboutToDie;

        private List<UnitTags> tags;
        public void AddTag(UnitTags tag)
        {
            if(!tags.Contains(tag))
                tags.Add(tag);
        }
        public void RemoveTag(UnitTags tag)
        {
            if(tags.Contains(tag))
                tags.Remove(tag);
        }
    }
}