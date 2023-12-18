using System;
using System.Collections.Generic;
using Game.GameActors.Items.Gems;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class Relic:EquipableItem
    {
        public GemSlot gemSlot;
     
        public Unit user;
        public Relic(string name, string description, int cost, int rarity, int maxStack,Sprite sprite,Skill skill, Gem gem) : 
            base(name, description, cost,rarity, maxStack,sprite, skill)
        {

            gemSlot = new GemSlot();
            if (gem != null)
                gemSlot.gem = gem;
        }

       

        

        void UpdateUser()
        {
            if (user != null)
            {
                Debug.Log("TODO add Bonus Attributes on Equip and only remove on unequip");
                Debug.Log("TODO add Skill to bonus skill list in skillmanager and remove on unequip");
                Debug.Log("TODO look at gems and see how they handly gemtype effects do also on relic");
                Debug.Log("TODO on insert remove slot update everything");
                Debug.Log("Subscribe to right event for each gemstone type/ passive skill effect");
            }
        }

        public void InsertGem(Gem gem)
        {
            gemSlot.gem = gem;
            gem.Insert();
            UpdateUser();

        }
        public Gem RemoveGem()
        {
            var gem = gemSlot.gem;
            gem.Remove();
            gemSlot.gem = null;
            UpdateUser();
            return gem;

        }

        public bool HasEmptySlot()
        {
            return gemSlot.IsEmpty();
        }
        

        public Gem GetGem(int index)
        {
            if (!gemSlot.IsEmpty())
                return gemSlot.gem;
            return null;
        }

        public void Unequip(Unit unit)
        {
            Debug.Log("UNEQUIP RELIC");
            user = null;
            if (Skill != null)
            {
                Skill.UnbindSkill(unit);
            }

            if (!gemSlot.IsEmpty())
            {
                gemSlot.gem.gemEffect.UnbindSkill(unit);

            }
        }

        public void Equip(Unit unit)
        {
            Debug.Log("EQUIP RELIC");
            user = unit;
            if (Skill != null)
            {
                Skill.BindSkill(unit);
            }
            if (!gemSlot.IsEmpty())
            {
                gemSlot.Bind(this);
                
            }
        }
    }
}