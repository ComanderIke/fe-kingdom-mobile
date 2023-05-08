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
        public string relic2Id;
        [SerializeField]
        public List<string> relic2slotIds;
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
            relic2slotIds = new List<string>();
            if (unit.EquippedRelic1 != null)
            {
                relic1Id = unit.EquippedRelic1.Name;
               
                foreach (var slot in unit.EquippedRelic1.slots)
                {
                    if (slot != null&&slot.gem!=null)
                        relic1slotIds.Add(slot.gem.Name);
                }
            }

            if (unit.EquippedRelic1 != null)
            { 
                relic2Id = unit.EquippedRelic2.Name;
                foreach (var slot in unit.EquippedRelic2.slots)
                {
                    if (slot != null&&slot.gem!=null)
                        relic2slotIds.Add(slot.gem.Name);
                }
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
            unit.TurnStateManager = TurnStateManager;
            unit.Hp = hp;
            if(blessingId!=""&&blessingId!=null)
                unit.ReceiveBlessing(GameBPData.Instance.GetBlessing(blessingId));
            if(curseId!=""&&curseId!=null)
                unit.ReceiveCurse(GameBPData.Instance.GetCurse(curseId));
       
            unit.equippedWeapon = GameBPData.Instance.GetWeapon(weaponId);
            unit.equippedWeapon.weaponLevel = weaponLvl;
            if(relic1Id!=""&&relic1Id!=null)
                unit.EquippedRelic1 = GameBPData.Instance.GetRelic(relic1Id);
            if(relic2Id!=""&&relic2Id!=null)
                unit.EquippedRelic2 = GameBPData.Instance.GetRelic(relic2Id);
            unit.ClassUpgraded = classUpgraded;
            if (unit.EquippedRelic1 != null)
            {
                int index = 0;
                foreach (var gemId in relic1slotIds)
                {
                    unit.EquippedRelic1.InsertGem(GameBPData.Instance.GetGem(gemId),index);
                    index++;
                }
            }
            if (unit.EquippedRelic2 != null)
            {
                
                int index = 0;
                foreach (var gemId in relic2slotIds)
                {
                    unit.EquippedRelic2.InsertGem(GameBPData.Instance.GetGem(gemId),index);
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