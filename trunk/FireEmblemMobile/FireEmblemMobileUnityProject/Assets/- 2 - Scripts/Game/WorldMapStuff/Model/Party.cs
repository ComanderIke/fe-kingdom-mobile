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
        public static int MaxSize = 4;

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
            Debug.Log("TODO AttackTarget Visuals?");
        }

   

        public override void ResetPosition()
        {
            Debug.Log("TODOResetLocation?");
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
            //newParty.location.worldMapPosition.SetActor(newParty);
            return newParty;
        }

        public void Join(Party otherParty)
        {
            foreach (var member in otherParty.members)
            {
                members.Add(member);
            }

            otherParty.location.Actor = null;
            otherParty.GameTransformManager.Destroy();
            GameObject.Destroy(otherParty);
        }
    }
}