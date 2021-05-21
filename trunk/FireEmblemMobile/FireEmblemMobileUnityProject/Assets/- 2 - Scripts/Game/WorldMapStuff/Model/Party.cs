using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
    public class Party:WM_Actor
    {

        [SerializeField] public List<Unit> members;
        public static Action<Party> PartyDied;
        public WM_ActorRenderer WmActorRenderer { get; set; }

        public Party():base()
        {
            members = new List<Unit>();
        }
    

        public bool IsAlive()
        {
            return members.Count(a => a.IsAlive()) != 0;
        }

        public override void SetAttackTarget(bool b)
        {
            Debug.Log("TODO AttackTarget Visuals");
        }

   

        public override void ResetPosition()
        {
            Debug.Log("ResetLocation");
        }

        public Party Split()
        {
            var newParty = Instantiate(this);
            int half = members.Count / 2;
            newParty.members = new List<Unit>();
            for(int i=0; i < half; i++)
            {
               newParty.members.Add(members[members.Count-1-i]);
               members.RemoveAt(members.Count-1-i);
            }

            newParty.Faction = Faction;
            return newParty;
        }

        public void Join(Party otherParty)
        {
            throw new NotImplementedException();
        }
    }
}