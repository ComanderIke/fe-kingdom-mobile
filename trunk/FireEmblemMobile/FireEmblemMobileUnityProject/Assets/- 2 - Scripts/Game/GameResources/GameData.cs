﻿using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Map;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using GameEngine;
using UnityEngine;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameData/Config/GameData")]
    public class GameData : SingletonScriptableObject<GameData>    {
        public UiData UiData;
        public DialogData DialogTexts;
        public UnitData UnitData;
        public CharacterStateData CharacterStateData;
        

   
        [SerializeField] private List<Weapon> weapons = default;
        [SerializeField] private List<Weapon> magic = default;
        [SerializeField] private List<EquipableItem> consumables = default;
        [SerializeField] private List<Human> humans = default;
        [SerializeField] private List<Monster> monster = default;
        [SerializeField] private List<Party> playerStartingParties = default;
        [SerializeField] public List<CampaignConfig> campaigns;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitialize()
        {
            Debug.Log("Before Awake!");
        }

        public List<Weapon> Weapons
        {
            get
            {
                var ret = new List<Weapon>();
                foreach (var weapon in weapons)
                {
                    var weaponGameObject = Instantiate(weapon);
                    weaponGameObject.name = weapon.name;
                    ret.Add(weaponGameObject);
                }
                return ret;
            }
        }

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
            return Instantiate(weapons.Find(a => a.name == name));
        }
        public Human GetHuman(string name)
        {
            return Instantiate(humans.Find(a => a.name == name));
        }
        public Monster GetMonster(string name)
        {
            return Instantiate(monster.Find(a => a.name == name));
        }

        public EquipableItem GetRandomPotion()
        {
            return Instantiate(consumables[Random.Range(0, consumables.Count-1)]);
        }

        public EquipableItem GetRandomMagic()
        {
            return Instantiate(magic[Random.Range(0, magic.Count-1)]);
        }

        public EquipableItem GetRandomWeapon()
        {
            return Instantiate(weapons[Random.Range(0, weapons.Count-1)]);
        }
    }
}
