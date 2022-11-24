using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Map;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using GameEngine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData/Config/GameData")]
    public class GameData : SingletonScriptableObject<GameData>    {
        public DialogData DialogTexts;
        public UnitData UnitData;
        public CharacterStateData CharacterStateData;
        

        // [SerializeField] private List<EquipableItem> armor = default;
        [SerializeField] private List<EquipableItemBP> relics = default;
        [SerializeField] private WeaponBP[] allWeapons;
        [SerializeField] private List<WeaponBP> staffs = default;
        [SerializeField] private List<WeaponBP> spears = default;
        [SerializeField] private List<WeaponBP> bows = default;
        [SerializeField] private List<WeaponBP> swords = default;
        [SerializeField] private List<WeaponBP> magic = default;
        [SerializeField] private List<ItemBP> consumables = default;
        [SerializeField] private List<MetaUpgradeBP> metaUpgradeBps = default;
        [FormerlySerializedAs("humans")] [SerializeField] private List<UnitBP> humanBlueprints = default;
        [SerializeField] private List<Party> playerStartingParties = default;
        [SerializeField] public List<CampaignConfig> campaigns;
        [SerializeField] private BlessingData blessingData;
        [SerializeField] private EventData eventData;
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
            allWeapons = GetAllInstances<WeaponBP>();
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
            return (Weapon)magic[Random.Range(0, magic.Count-1)].Create();
        }

        public Weapon GetRandomSword()
        {
            return (Weapon)swords[Random.Range(0, swords.Count-1)].Create();
        }

        // public EquipableItem GetRandomArmor()
        // {
        //     return Instantiate(armor[Random.Range(0, armor.Count-1)]);
        // }

        public EquipableItem GetRandomRelic()
        {
            return (EquipableItem)relics[Random.Range(0, relics.Count-1)].Create();
        }

        public Weapon GetRandomStaff()
        {
            return (Weapon)staffs[Random.Range(0, staffs.Count-1)].Create();
        }

        public HealthPotion GetHealthPotion()
        {
            return (HealthPotion)consumables[0].Create();
        }
        

        public Weapon GetRandomBow()
        {
            return (Weapon)bows[Random.Range(0, bows.Count-1)].Create();
        }

        public Weapon GetRandomSpear()
        {
            return (Weapon)spears[Random.Range(0, spears.Count-1)].Create();
        }

        public IBlessingData GetBlessingData()
        {
            return blessingData;
        }
        public IEventData GetEventData()
        {
            return eventData;
        }
    }
}
