using System.Collections.Generic;
using Game.DataAndReferences.Data;
using Game.EncounterAreas.Model;
using Game.GUI.EncounterUI.Church;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Church
{
    public class ChurchEncounterNode : EncounterNode
    {
        public Church church;
   
        public ChurchEncounterNode(List<EncounterNode> parents,int depth, int childIndex, string label, string description, Sprite sprite) : base(parents, depth, childIndex, label, description, sprite)
        {
            church = new Church(GameBPData.Instance);
        
            //church.AddItem(new ShopItem(GameData.Instance.GetRandomStaff()));
       
        }
   
        public override void Activate(Party party)
        {
            base.Activate(party);
            GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        
        }
    }
}