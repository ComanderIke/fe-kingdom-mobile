using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.Dialog.DialogSystem;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Items;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameActors.Units.Skills;
using Game.Map;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using GameEngine;
using LostGrace;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData/Config/GameData")]
    public class GameBPData : SingletonScriptableObject<GameBPData> , IBlessingData   {
        public DialogData DialogTexts;
        public UnitData UnitData;
        

        // [SerializeField] private List<EquipableItem> armor = default;
        [SerializeField] private RelicBP[] allRelics = default;
        [SerializeField] private RelicBP[] rareRelics = default;
        [SerializeField] private RelicBP[] uncommonRelics = default;
        [SerializeField] private RelicBP[]superRareRelics = default;
        [SerializeField] private WeaponBP[] allWeapons;
        [SerializeField] private SkillBp[] allSkills;
        [SerializeField] private God[] allGods;
        [SerializeField] private GemBP[] allGems;
        [SerializeField] private GemBP[] allSmallGems;
        [SerializeField] private GemBP[] allMediumGems;
        [SerializeField] private GemBP[] allLargeGems;
        [SerializeField] private ItemBP[] allItems;
        [SerializeField] private ConsumableItemBp[] allConsumables;
        [SerializeField] private AttributePotionBP[] allAttributePotions;
        [SerializeField] private BuffPotionBP[] allBuffPotions;
    
        [SerializeField] private BombBP[] allBombs;
        [SerializeField] private List<ConsumableItemBp> commonPotions;
        [SerializeField] private List<ConsumableItemBp> rarePotions;
        [SerializeField] private List<WeaponBP> staffs = default;
        [SerializeField] private List<WeaponBP> spears = default;
        [SerializeField] private List<WeaponBP> bows = default;
        [SerializeField] private List<WeaponBP> swords = default;
        [SerializeField] private List<WeaponBP> magic = default;
        [SerializeField] private List<MetaUpgradeBP> metaUpgradeBps = default;
        [FormerlySerializedAs("humans")] [SerializeField] UnitBP []allUnits = default;
        [SerializeField] public List<CampaignConfig> campaigns;
        [SerializeField] private StoneBP smithingStone;
        [SerializeField] private StoneBP dragonScale;
        [SerializeField] private ItemBP memberCard;
        [SerializeField] private List<SkillBp> relicSkillPool;
        [SerializeField] private EventData eventData;
        [SerializeField] private StatusEffectData statusEffectData;
        [SerializeField] BlessingBP[] allBlessings;
        [SerializeField] CurseBP[] allCurses;
        [SerializeField] BlessingBP[]  tier0Blessings;
        [SerializeField] BlessingBP[]  tier1Blessings;
        [SerializeField] BlessingBP[]  tier2Blessings;
        [SerializeField] BlessingBP[]  tier3Blessings;
        [SerializeField] BattleMap[] battleEncounterMapsArea1;
        [SerializeField] BattleMap[] battleEncounterMapsArea2;
        [SerializeField] BattleMap[] eliteBattleEncounterMapsArea1;
        [SerializeField] BattleMap[] eliteBattleEncounterMapsArea2;
        [SerializeField] BattleMap[] allBattleMaps;
   
        
        public BlessingBP[]  GetBlessingPool(int tier)
        {
            switch (tier)
            {
                case 0: return tier0Blessings;
                case 1: return tier1Blessings;
                case 2: return tier2Blessings;
                case 3: return tier3Blessings;
                default: return null;
            }
        }

        
        [field:SerializeField]public BattleRewardConfig BattleRewardConfig { get; set; }
        [field:SerializeField] public SkillGenerationConfiguration SkillGenerationConfig { get; set; }
        [field:SerializeField] public Sprite DefaultMerchantSprite { get; set; }
        [field:SerializeField] public string DefaultMerchantName { get; set; }
        [field:SerializeField]  public BattleMap TutorialMap { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitialize()
        {
            Debug.Log("First initialization before scene loads!");
        }

        // public List<Weapon> Weapons
        // {
        //     get
        //     {
        //         var ret = new List<Weapon>();
        //         foreach (var weapon in weapons)
        //         {
        //             var weaponGameObject = Instantiate(weapon);
        //             weaponGameObject.name = weapon.name;
        //             ret.Add(weaponGameObject);
        //         }
        //         return ret;
        //     }
        // }
        #if UNITY_EDITOR
        private void OnValidate()
        {
            eventData.OnValidate();
            statusEffectData.OnValidate();
            allUnits = GetAllInstances<UnitBP>();
            allGods = GetAllInstances<God>();
            allBattleMaps = GetAllInstances<BattleMap>();
            allBlessings = GetAllInstances<BlessingBP>();
            tier0Blessings = Array.FindAll(allBlessings,a => a.Tier == 0);
            tier1Blessings = Array.FindAll(allBlessings,a => a.Tier == 1);
            tier2Blessings = Array.FindAll(allBlessings,a => a.Tier == 2);
            tier3Blessings = Array.FindAll(allBlessings,a => a.Tier == 3);
            allWeapons = GetAllInstances<WeaponBP>();
            allCurses = GetAllInstances<CurseBP>();
            
            if (GameConfig.Instance.ConfigProfile.overWriteSkills)
            {
                var demoSkills = GameConfig.Instance.ConfigProfile.OverwritenSkills;
                allSkills = GetAllInstances<SkillBp>().Intersect(demoSkills).ToArray();
            }
            else
            {
                allSkills = GetAllInstances<SkillBp>();
            }
            
            
            if (GameConfig.Instance.ConfigProfile.overWriteItems)
            {
                var demoItems = GameConfig.Instance.ConfigProfile.OverwritenItems;
                allItems = Array.FindAll(GetAllInstances<ItemBP>().Intersect(demoItems).ToArray(), a => !(a is WeaponBP));
            }
            else
            {
                allItems = Array.FindAll(GetAllInstances<ItemBP>(), a => !(a is WeaponBP));
            }

          
            int cnt = allItems.Count(a => a is BombBP);
            allBombs = new BombBP[cnt];
            Array.Copy( allItems.Where(a => a is BombBP).ToArray(), allBombs,cnt );
            cnt = allItems.Count(a => a is BuffPotionBP);
            allBuffPotions = new BuffPotionBP[cnt];
            Array.Copy( allItems.Where(a => a is BuffPotionBP).ToArray(), allBuffPotions,  cnt);
            cnt = allItems.Count(a => a is AttributePotionBP);
            allAttributePotions = new AttributePotionBP[cnt];
            Array.Copy( allItems.Where(a => a is AttributePotionBP).ToArray(), allAttributePotions,cnt);
            cnt = allItems.Count(a => a is ConsumableItemBp);
            allConsumables = new ConsumableItemBp[cnt];
            Array.Copy(  allItems.Where(a => a is ConsumableItemBp).ToArray(),allConsumables,cnt);
            cnt = allItems.Count(a => a is RelicBP);
            allRelics = new RelicBP[cnt];
            Array.Copy(  allItems.Where(a => a is RelicBP).ToArray(),allRelics,cnt);
            cnt = allItems.Count(a => a is GemBP);
            allGems = new GemBP[cnt];
            Array.Copy(   allItems.Where(a => a is GemBP).ToArray(),allGems,cnt);
          

            uncommonRelics = Array.FindAll(allRelics, a => a.rarity == 1);
            rareRelics = Array.FindAll(allRelics, a => a.rarity == 2);
            superRareRelics = Array.FindAll(allRelics, a => a.rarity == 3);
            allSmallGems = Array.FindAll(allGems,a => a.GetRarity() == 1);
            allMediumGems = Array.FindAll(allGems,a => a.GetRarity() == 2);
            allLargeGems = Array.FindAll(allGems,a => a.GetRarity() == 3);
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for(int i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
 
        }
#endif
        
      
        public Weapon GetWeapon(string name)
        {
            WeaponBP weaponBp = null;
            weaponBp = allWeapons.First(a => a.name == name);

            return (Weapon)weaponBp.Create();
        }
        public Relic GetRelic(string name)
        {
            RelicBP weaponBp = null;
            weaponBp = allRelics.First(a => a.name == name);

            return (Relic)weaponBp.Create();
        }
        public Skill GetSkill(string name)
        {
            SkillBp weaponBp = null;
            weaponBp = allSkills.First(a => a.Name == name);

            return (Skill)weaponBp.Create();
        }
        public Gem GetGem(string name)
        {
            GemBP weaponBp = null;
            weaponBp = allGems.First(a => a.name == name);

            return (Gem)weaponBp.Create();
        }
        public Blessing GetBlessing(string name)
        {
            Debug.Log(name);
            BlessingBP weaponBp = null;
            weaponBp = allBlessings.First(a => a.Name == name);

            return (Blessing)weaponBp.Create();
        }
        public Curse GetCurse(string name)
        {
            CurseBP weaponBp = null;
            weaponBp = allCurses.First(a => a.name == name);

            return (Curse)weaponBp.Create();
        }
        public Unit GetHumanFromBlueprint(string name)
        {
            return allUnits.First(a => a.bluePrintID == name).Create(Guid.NewGuid());
        }
        public MetaUpgradeBP GetMetaUpgradeBlueprints(string name)
        {
            var find = metaUpgradeBps.Find(a => a.name == name);
            if (find == null)
            {
                Debug.LogError("Did not find: " + name + " in metaupgrade collection");
                return null;
            }
            else
            {
                Debug.Log("Found: " + find.name+" "+find.label);
            }
            //Dont Instantiate use original
            return find;
        }
        
        public Weapon GetRandomMagic()
        {
            return (Weapon)magic[Random.Range(0, magic.Count)].Create();
        }

        public Weapon GetRandomSword()
        {
            return (Weapon)swords[Random.Range(0, swords.Count)].Create();
        }

        // public EquipableItem GetRandomArmor()
        // {
        //     return Instantiate(armor[Random.Range(0, armor.Count-1)]);
        // }

        public Relic GetRandomRelic(int rarity)
        {
            switch (rarity)
            {
                case 1: return (Relic)uncommonRelics[Random.Range(0, uncommonRelics.Length)].Create();
                case 2: return (Relic)rareRelics[Random.Range(0, rareRelics.Length)].Create();
                case 3: return (Relic)superRareRelics[Random.Range(0, superRareRelics.Length)].Create();
            }

            return null;
        }
        
        public Weapon GetRandomStaff()
        {
            return (Weapon)staffs[Random.Range(0, staffs.Count)].Create();
        }

        public Item GetItemByName(string name)
        {
            var item = Array.Find(allItems, a => a.name == name);
            if(item!=null)
                return item.Create();
            Debug.LogError("No Item found with name: "+name);
            return null;
        }
        

        public Weapon GetRandomBow()
        {
            return (Weapon)bows[Random.Range(0, bows.Count)].Create();
        }

        public Weapon GetRandomSpear()
        {
            return (Weapon)spears[Random.Range(0, spears.Count)].Create();
        }
        
        public IEventData GetEventData()
        {
            return eventData;
        }

        public Item GetRandomConsumeables()
        {
            return allConsumables[Random.Range(0,allConsumables.Length)].Create();
        }

        public Item GetRandomGem()
        {
          
            return allGems[Random.Range(0, allGems.Length)].Create();
        }

        public Item GetRandomBomb()
        {
            return allBombs[Random.Range(0, allBombs.Length)].Create();
        }

        public Item GetRandomCommonConsumeables()
        {
            var commonConsumables = Array.FindAll(allConsumables, a => a.rarity == 1);
            return commonConsumables[Random.Range(0, commonConsumables.Length)].Create();
        }
        public Item GetRandomRareConsumeable()
        {
            var commonConsumables = Array.FindAll(allConsumables, a => a.rarity == 2);
            return commonConsumables[Random.Range(0, commonConsumables.Length)].Create();
        }
        public Item GetRandomEpicConsumeable()
        {
            var commonConsumables = Array.FindAll(allConsumables, a => a.rarity == 3);
            return commonConsumables[Random.Range(0, commonConsumables.Length)].Create();
        }


        public Item GetRandomItem()
        {
            return allItems[Random.Range(0, allItems.Length)].Create();
        }

        public Item GetSmithingStone()
        {
            return smithingStone.Create();
        }

        public Item GetDragonScale()
        {
            return dragonScale.Create();
        }
        public Item GetMemberCard()
        {
            return memberCard.Create();
        }

        public Skill GetRandomRelicSkill()
        {
            return null;
            //return relicSkillPool[Random.Range(0, relicSkillPool.Count)].Create();
        }

        public Item GetRandomPotion(float chanceForRare)
        {
            List<ConsumableItemBp> potions;
            if (Random.value <= chanceForRare)
            {
                potions = rarePotions;
            }
            else
            {
                potions = commonPotions;
            }

            
            return potions[Random.Range(0, potions.Count)].Create();
        }


        public IEnumerable<UnitBP> GetAllPlayableUnits()
        {
            return allUnits.Where(u => !u.isEnemy);
        }
        public IEnumerable<UnitBP> GetAllSins()
        {
            return allUnits.Where(u => u.isBoss);
        }
        public IEnumerable<God> GetAllGods()
        {
            return allGods.ToList();
        }
        public IEnumerable<UnitBP> GetAllEnemies()
        {
            return allUnits.Where(u => u.isEnemy&&!u.isBoss);
        }

        public BattleMap GetRandomMap(BattleType battleType)
        {
            int areaIndex = Player.Instance.Party.AreaIndex;
            switch (battleType)
            {
                case BattleType.Boss:
                    return null;
                case BattleType.Normal:
                    switch (areaIndex)
                    {
                        case 1:   return battleEncounterMapsArea1  [Random.Range(0, battleEncounterMapsArea1.Length)];
                        case 2:   return battleEncounterMapsArea2  [Random.Range(0, battleEncounterMapsArea2.Length)];
                    }

                    break;
                  
                case BattleType.Elite: 
                    switch (areaIndex)
                    {
                        case 1:   return eliteBattleEncounterMapsArea1
                            [Random.Range(0, eliteBattleEncounterMapsArea1.Length)];
                        case 2:   return eliteBattleEncounterMapsArea2
                            [Random.Range(0, eliteBattleEncounterMapsArea2.Length)];
                    }

                    break;
            }

            return null;
        }

        public BattleMap GetMapById(string id)
        {
            return allBattleMaps.First(a => a.name == id);
        }

        [SerializeField] private List<DifficultyProfile> difficultyProfiles;
        public DifficultyProfile GetDifficultyProfile(string name)
        {
            return difficultyProfiles.First(d => d.name == name);
        }
    }
}
