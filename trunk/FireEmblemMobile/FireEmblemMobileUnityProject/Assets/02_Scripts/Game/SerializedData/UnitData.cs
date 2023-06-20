using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class UnitData
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public string weaponId;
        [SerializeField]
        public int weaponLvl;
        [SerializeField]
        public string relic1Id;
        [SerializeField]
        public List<string> relic1slotIds;
        [SerializeField]
        public string combatItem1Id;
        [SerializeField]
        public int combatItem1Stock;

        [SerializeField]
        public string combatItem2Id;
        [SerializeField]
        public int combatItem2Stock;


       
        [SerializeField]
        public int hp;
        [SerializeField]
        private StatsData statsData;
        [SerializeField]
        private Attributes growthsData;
        [SerializeField]
        public ExperienceManager ExperienceManager;
        [SerializeField]
        public TurnStateManager TurnStateManager;
        [SerializeField]
        public string unitBlueprintID;
        [SerializeField]
        public string blessingId;
        [SerializeField]
        public string curseId;

        [SerializeField] public bool classUpgraded;
        [SerializeField]
        public List<string> skillIds;
        [SerializeField]
        public List<int> skillLevels;
        public UnitData(Unit unit)
        {
            this.name = unit.name;
            ExperienceManager = unit.ExperienceManager;
            TurnStateManager = unit.TurnStateManager;
            this.growthsData = unit.Growths;
            this.statsData = unit.Stats.GetSaveData();
            this.unitBlueprintID = unit.bluePrintID;
            this.hp = unit.Hp;
            weaponId = unit.equippedWeapon.Name;
            weaponLvl = unit.equippedWeapon.weaponLevel;
          
           
            if(unit.Blessing!=null)
                blessingId = unit.Blessing.Name;
            if(unit.Curse!=null)
                curseId = unit.Curse.Name;
            classUpgraded = unit.ClassUpgraded;
           
            relic1slotIds = new List<string>();
            if (unit.EquippedRelic != null)
            {
                relic1Id = unit.EquippedRelic.Name;
               
                foreach (var slot in unit.EquippedRelic.slots)
                {
                    if (slot != null&&slot.gem!=null)
                        relic1slotIds.Add(slot.gem.Name);
                }
            }

            if (unit.CombatItem1 != null)
            {
                combatItem1Id = unit.CombatItem1.item.Name;
                combatItem1Stock = unit.CombatItem1.stock;
            }
            if (unit.CombatItem2 != null)
            {
                combatItem2Id = unit.CombatItem2.item.Name;
                combatItem2Stock = unit.CombatItem2.stock;
            }

      
            skillIds = new List<string>();
            skillLevels = new List<int>();
            foreach (var skill in unit.SkillManager.Skills)
            {
                skillIds.Add(skill.Name);
                skillLevels.Add(skill.Level);
            }
           
        }

        public virtual Unit Load()
        {
           // Debug.Log("TODO MAKE CLEAN AND CONSISTENT");
            Unit unit = GameBPData.Instance.GetHumanFromBlueprint(unitBlueprintID);
            unit.name = name;
            unit.Stats = new Stats();//ScriptableObject.CreateInstance<Stats>();
            unit.Stats.LoadData(statsData);
            unit.Growths = new Attributes(growthsData);//ScriptableObject.CreateInstance<Growths>();
            // unit.Growths.LoadData(growthsData);
            unit.ExperienceManager = new ExperienceManager(ExperienceManager);
            unit.ExperienceManager.Exp = ExperienceManager.Exp;
            unit.ExperienceManager.Level = ExperienceManager.Level;
            unit.InitExperienceManager();
            unit.TurnStateManager = TurnStateManager;
            unit.Hp = hp;
            if(blessingId!=""&&blessingId!=null)
                unit.ReceiveBlessing(GameBPData.Instance.GetBlessing(blessingId));
            if(curseId!=""&&curseId!=null)
                unit.ReceiveCurse(GameBPData.Instance.GetCurse(curseId));
       
            unit.equippedWeapon = GameBPData.Instance.GetWeapon(weaponId);
            unit.equippedWeapon.weaponLevel = weaponLvl;
            if(!string.IsNullOrEmpty(relic1Id))
                unit.EquippedRelic = GameBPData.Instance.GetRelic(relic1Id);
            if (!string.IsNullOrEmpty(combatItem1Id))
            {
                unit.CombatItem1 = new StockedItem(GameBPData.Instance.GetItemByName(combatItem1Id), combatItem1Stock);
            }
            if (!string.IsNullOrEmpty(combatItem2Id))
            {
                unit.CombatItem2 = new StockedItem(GameBPData.Instance.GetItemByName(combatItem2Id), combatItem2Stock);
            }
            unit.ClassUpgraded = classUpgraded;
            if (unit.EquippedRelic != null)
            {
                int index = 0;
                foreach (var gemId in relic1slotIds)
                {
                    unit.EquippedRelic.InsertGem(GameBPData.Instance.GetGem(gemId),index);
                    index++;
                }
            }
           

            int skillIndex = 0;
            foreach (var skillId in skillIds)
            {
                if (skillId != "")
                {
                    Skill skill = GameBPData.Instance.GetSkill(skillId);
                    skill.Level = skillLevels[skillIndex];
                    unit.SkillManager.Skills.Add(skill);
                }

                skillIndex++;
                

            }
            return unit;
            // unit.visuals = new UnitVisual();
            //
            // unit.visuals.CharacterSpriteSet = GameAssets.Instance.visuals.LoadCharacterSpriteSet(spriteID);
            //  unit.visuals.UnitEffectVisual = GameAssets.Instance.visuals.LoadUnitEffectVisual(vfxID);
            // unit.MoveType = GameData.Instance.UnitData.GetMoveType(moveTypeId);

        }
    }
}