using System.Collections.Generic;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Model;
using Game.GameActors.Player;
using Game.GUI.EncounterUI.Inn;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Inn
{
    public class InnEncounterNode : EncounterNode
    {
    

        public InnEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
        {

        }

        public override void Activate(Party party)
        {
            MyDebug.LogLogic("Visiting Inn");
            base.Activate(party);
            if (Player.Instance.Flags.InnSupplies)
            {
                party.AddSupplies(50);
            }
            if (Player.Instance.Flags.InnBonds)
            {
                foreach(var unit in party.members)
                    unit.Bonds.Increase(GameBPData.Instance.GetGod("Hestia"),10);

            }
            GameObject.FindObjectOfType<UIInnController>().Show(this, party);
       
        }
    }
}