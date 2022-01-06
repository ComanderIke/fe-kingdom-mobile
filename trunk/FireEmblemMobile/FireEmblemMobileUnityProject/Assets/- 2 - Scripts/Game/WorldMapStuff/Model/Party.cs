using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
    public class Party:ScriptableObject
    {

        [SerializeField] public List<Unit> members;
        public static Action<Party> PartyDied;
        public static int MaxSize = 4;

        public Party():base()
        {
            members = new List<Unit>();
        }

        public EncounterNode EncounterNode { get; set; }
        public GameObject GameObject { get; set; }

        public bool IsAlive()
        {
            return members.Count(a => a.IsAlive()) != 0;
        }

        public void Initialize()
        {
            Debug.Log("Init Party");
            foreach (var member in members)
            {
                member.Initialize();
            }
        }
    }
}