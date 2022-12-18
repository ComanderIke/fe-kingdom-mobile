using System;
using System.Collections.Generic;
using System.Linq;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.GameActors.Units.Skills;
using Game.Map;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using GameEngine;
using LostGrace;
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
        public CharacterStateData CharacterStateData;
        

        // [SerializeField] private List<EquipableItem> armor = default;
        [SerializeField] private RelicBP[] relics = default;
        [SerializeField] private RelicBP[] rareRelics = default;
        [SerializeField] private RelicBP[] uncommonRelics = default;
        [SerializeField] private RelicBP[]superRareRelics = default;
        [SerializeField] private WeaponBP[] allWeapons;
        [SerializeField] private GemBP[] allGems;
        [SerializeField] private GemBP[] allSmallGems;
        [SerializeField] private GemBP[] allMediumGems;
        [SerializeField] private GemBP[] allLargeGems;
        [SerializeField] private ItemBP[] allItems;
        [SerializeField] private ConsumableItemBp[] allConsumables;
        [SerializeField] private AttributePotionBP[] allAttributePotions;
        [SerializeField] private BuffPotionBP[] allBuffPotions;
        [SerializeField] private BombBP[] allBombs;
        [SerializeField] private List<WeaponBP> staffs = default;
        [SerializeField] private List<WeaponBP> spears = default;
        [SerializeField] private List<WeaponBP> bows = default;
        [SerializeField] private List<WeaponBP> swords = default;
        [SerializeField] private List<WeaponBP> magic = default;
        [SerializeField] private List<MetaUpgradeBP> metaUpgradeBps = default;
        [FormerlySerializedAs("humans")] [SerializeField] private List<UnitBP> humanBlueprints = default;
        [SerializeField] private List<Party> playerStartingParties = default;
        [SerializeField] public List<CampaignConfig> campaigns;
        [SerializeField] private ItemBP smithingStone;
        [SerializeField] private ItemBP dragonScale;
        [SerializeField] private ItemBP memberCard;
        [SerializeField] private List<SkillBP> relicSkillPool;
        [SerializeField] private EventData eventData;
        [SerializeField] BlessingBP[] allBlessings;
        [SerializeField] BlessingBP[]  tier0Blessings;
        [SerializeField] BlessingBP[]  tier1Blessings;
        [SerializeField] BlessingBP[]  tier2Blessings;
        [SerializeField] BlessingBP[]  tier3Blessings;
   
        
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitialize()
        {
            Debug.Log("Before Awake!");
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
            allBlessings = GetAllInstances<BlessingBP>();
            tier0Blessings = Array.FindAll(allBlessings,a => a.tier == 0);
            tier1Blessings = Array.FindAll(allBlessings,a => a.tier == 1);
            tier2Blessings = Array.FindAll(allBlessings,a => a.tier == 2);
            tier3Blessings = Array.FindAll(allBlessings,a => a.tier == 3);
            allGems = GetAllInstances<GemBP>();
            allSmallGems = Array.FindAll(allGems,a => a.GetRarity() == 1);
            allMediumGems = Array.FindAll(allGems,a => a.GetRarity() == 2);
            allLargeGems = Array.FindAll(allGems,a => a.GetRarity() == 3);
            allWeapons = GetAllInstances<WeaponBP>();
            allBombs = GetAllInstances<BombBP>();
            allBuffPotions = GetAllInstances<BuffPotionBP>();
            allAttributePotions = GetAllInstances<AttributePotionBP>();
            allConsumables = GetAllInstances<ConsumableItemBp>();
            allItems = Array.FindAll(GetAllInstances<ItemBP>(), a => !(a is WeaponBP));
            relics = GetAllInstances<RelicBP>();
            uncommonRelics = Array.FindAll(relics, a => a.rarity == 1);
            rareRelics = Array.FindAll(relics, a => a.rarity == 2);
            superRareRelics = Array.FindAll(relics, a => a.rarity == 3);

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
            weaponBp = swords.Find(a => ((Object)a).name == name);
            if(weaponBp==null)
                weaponBp = bows.Find(a => ((Object)a).name == name);
            if(weaponBp==null)
                weaponBp = magic.Find(a => ((Object)a).name == name);
            if(weaponBp==null)
                weaponBp = spears.Find(a => ((Object)a).name == name);
            if (weaponBp == null)
                return null;
            return (Weapon)weaponBp.Create();
        }
        public Unit GetHumanFromBlueprint(string name)
        {
            Debug.Log("name: " + name);
            return humanBlueprints.Find(a => a.bluePrintID == name).Create();
        }
        public MetaUpgradeBP GetMetaUpgradeBlueprints(string name)
        {
            Debug.Log("name: " + name);
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
            return relicSkillPool[Random.Range(0, relicSkillPool.Count)].Create();
        }
    }
}
