using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Monsters;
using Game.Map;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameResources
{
    public class DataScript : MonoBehaviour
    {
        public UiData UiData;
        public DialogData DialogTexts;
        public UnitData UnitData;
        public CharacterStateData CharacterStateData;

        public static DataScript Instance;
        [SerializeField] private List<Weapon> weapons = default;
        [SerializeField] private List<Human> humans = default;
        [SerializeField] private List<Monster> monster = default;
        [SerializeField] private List<Party> parties = default;


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
            var tmpMembers = parties[campaignIndex].members;
            var party =  Instantiate((parties[campaignIndex]));
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

        private void OnEnable()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }


    }
}
