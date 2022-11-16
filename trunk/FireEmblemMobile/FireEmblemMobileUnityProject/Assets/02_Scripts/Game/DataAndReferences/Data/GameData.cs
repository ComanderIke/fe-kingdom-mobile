﻿using System;
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
using Random = UnityEngine.Random;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData/Config/GameData")]
    public class GameData : SingletonScriptableObject<GameData>    {
        public DialogData DialogTexts;
        public UnitData UnitData;
        public CharacterStateData CharacterStateData;
        

        // [SerializeField] private List<EquipableItem> armor = default;
        [SerializeField] private List<EquipableItem> relics = default;
        [SerializeField] private Weapon[] allWeapons;
        [SerializeField] private List<Weapon> staffs = default;
        [SerializeField] private List<Weapon> spears = default;
        [SerializeField] private List<Weapon> bows = default;
        [SerializeField] private List<Weapon> swords = default;
        [SerializeField] private List<Weapon> magic = default;
        [SerializeField] private List<Item> consumables = default;
        [SerializeField] private List<MetaUpgradeBP> metaUpgradeBps = default;
        [FormerlySerializedAs("humans")] [SerializeField] private List<Unit> humanBlueprints = default;
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
            allWeapons = GetAllInstances<Weapon>();
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

        public Party GetCampaignParty(int campaignIndex)
        {
            var tmpMembers = playerStartingParties[campaignIndex].members;
            var party =  (playerStartingParties[campaignIndex]);
            party.members = new List<Unit>();
            foreach (var member in tmpMembers)
            {
                Unit unit = Instantiate(member);
                unit.Initialize();
                party.members.Add(unit);
            }

            return party;
        }
      
        public Weapon GetWeapon(string name)
        {
            Weapon weapon = null;
            weapon = swords.Find(a => a.name == name);
            if(weapon==null)
                weapon = bows.Find(a => a.name == name);
            if(weapon==null)
                weapon = magic.Find(a => a.name == name);
            if(weapon==null)
                weapon = spears.Find(a => a.name == name);
            return Instantiate(weapon);
        }
        public Unit GetHumanBlueprint(string name)
        {
            Debug.Log("name: " + name);
            var human = Instantiate(humanBlueprints.Find(a => a.bluePrintID == name));
            human.InitEquipment();
            return human;
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


        public Item GetRandomPotion()
        {
            return Instantiate(consumables[Random.Range(0, consumables.Count-1)]);
        }
        

        public EquipableItem GetRandomMagic()
        {
            return Instantiate(magic[Random.Range(0, magic.Count-1)]);
        }

        public EquipableItem GetRandomSword()
        {
            return Instantiate(swords[Random.Range(0, swords.Count-1)]);
        }

        // public EquipableItem GetRandomArmor()
        // {
        //     return Instantiate(armor[Random.Range(0, armor.Count-1)]);
        // }

        public EquipableItem GetRandomRelic()
        {
            return Instantiate(relics[Random.Range(0, relics.Count-1)]);
        }

        public EquipableItem GetRandomStaff()
        {
            return Instantiate(staffs[Random.Range(0, staffs.Count-1)]);
        }

        public Item GetHealthPotion()
        {
            return Instantiate(consumables[0]);
        }

        public Item GetSPotion()
        {
            return Instantiate(consumables[1]);
        }

        public EquipableItem GetRandomBow()
        {
            return Instantiate(bows[Random.Range(0, bows.Count-1)]);
        }

        public EquipableItem GetRandomSpear()
        {
            return Instantiate(spears[Random.Range(0, spears.Count-1)]);
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
