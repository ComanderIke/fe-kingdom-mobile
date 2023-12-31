using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using Game.GameResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField]
        public bool dead;
        [SerializeField]
        public string name;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private RelicData relicData;
        [SerializeField]
        public string combatItem1Id;
        [SerializeField]
        public int combatItem1Stock;
        [SerializeField]
        public int hp;

        [SerializeField] public bool specialState;
        [SerializeField]
        private StatsData statsData;
        [SerializeField]
        public ExperienceManager ExperienceManager;
        [SerializeField]
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public string unitBlueprintID;
        [SerializeField]
        public string blessingId;
        [SerializeField]
        public List<string> curseIds;
        [SerializeField]
        public List<UnitTags> unitTags;
        [FormerlySerializedAs("survivalStones")] [SerializeField]
        public int revivalStones;
        [SerializeField]
        public BondData bonds;

        [SerializeField] public bool classUpgraded;
        [SerializeField] private SkillData skillData;
        [SerializeField] private List<EncounterBasedBuffData> encounterBasedBuffs;
        [SerializeField] private string uniqueIdentifier;
        [SerializeField] private bool fielded;
        public UnitData(Unit unit, bool dead = false)
        {
            this.dead = dead;
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.statsData = unit.Stats.GetSaveData();
            this.unitBlueprintID = unit.bluePrintID;
            this.hp = unit.Hp;
            this.weaponData = new WeaponData(unit.equippedWeapon);
            if(unit.Blessing!=null)
                blessingId = unit.Blessing.Name;
            curseIds = new List<string>();
            if (unit.Curses != null)
            {
                foreach (var curse in unit.Curses)
                {
                    curseIds.Add(curse.Name);
                }
            }
                
            classUpgraded = unit.ClassUpgraded;
            if(unit.EquippedRelic!=null)
                relicData = new RelicData(unit.EquippedRelic);
            if (unit.CombatItem1 != null)
            {
                combatItem1Id = unit.CombatItem1.item.GetName();
                combatItem1Stock = unit.CombatItem1.stock;
            }
            skillData = new SkillData(unit.SkillManager);
            fielded = unit.Fielded;
            Debug.Log("Save uniqueIdentifier: "+unit.uniqueIdentifier);
            uniqueIdentifier = unit.uniqueIdentifier.ToString();
            encounterBasedBuffs = new List<EncounterBasedBuffData>();
            foreach (var encounterBuff in unit.encounterBuffs)
            {
                encounterBasedBuffs.Add(new EncounterBasedBuffData(encounterBuff));
            }

            revivalStones = unit.RevivalStones;
            unitTags = unit.tags;
            bonds = new BondData(unit.Bonds, unit.Bonds.Experiences.Keys.ToList());
            specialState = unit.SpecialState;

        }

        public virtual Unit Load()
        {
           // Debug.Log("TODO MAKE CLEAN AND CONSISTENT");
            Unit unit = GameBPData.Instance.GetHumanFromBlueprint(unitBlueprintID);
            unit.name = name;
            unit.Stats = new Stats();//ScriptableObject.CreateInstance<Stats>();
            unit.Stats.LoadData(statsData);
            // unit.Growths.LoadData(growthsData);
            unit.ExperienceManager = new ExperienceManager(ExperienceManager);
            unit.ExperienceManager.Exp = ExperienceManager.Exp;
            unit.ExperienceManager.Level = ExperienceManager.Level;
            unit.InitExperienceManager();
            unit.TurnStateManager = TurnStateManager;
            unit.Hp = hp;
            if(blessingId!=""&&blessingId!=null)
                unit.ReceiveBlessing(GameBPData.Instance.GetBlessing(blessingId));
            foreach (var curseId in curseIds)
            {
                unit.ReceiveCurse(GameBPData.Instance.GetCurse(curseId));
            }


            unit.equippedWeapon = weaponData.Load();
            unit.EquippedRelic = relicData.Load();
          
            if (!string.IsNullOrEmpty(combatItem1Id))
            {
                unit.CombatItem1 = new StockedCombatItem((IEquipableCombatItem)GameBPData.Instance.GetItemByName(combatItem1Id), combatItem1Stock);
            }
            unit.ClassUpgraded = classUpgraded;

            skillData.Load(unit.SkillManager);
            unit.SpecialState = specialState;
            unit.RevivalStones = revivalStones;
            unit.tags = unitTags;
            unit.uniqueIdentifier = new Guid(uniqueIdentifier);
            Debug.Log("Load uniqueIdentifier: "+unit.uniqueIdentifier);
            foreach (var encounterBuff in encounterBasedBuffs)
            {
                unit.encounterBuffs.Add(encounterBuff.Load());
            }

            unit.Fielded = fielded;
            unit.Bonds = bonds.Load();
            return unit;

        }
    }
}