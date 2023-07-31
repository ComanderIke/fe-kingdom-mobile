using System;
using System.Collections.Generic;
using Game.GameActors.Items.Gems;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class GemSlot
    {
        [SerializeReference]
        public Gem gem;

        public GemSlot()
        {
            gem = null;
        }
        public bool IsEmpty()
        {
            return gem == null;
        }
    }
    
    [Serializable]
    public class Relic:EquipableItem
    {
        [Header("RelicAttributes")]
        public int slotCount = 0;
        public GemSlot[] slots;
        public Skill skill;
        private Unit user;
        public Relic(string name, string description, int cost, int rarity, int maxStack,Sprite sprite,Skill skill,int slotCount) : base(name, description, cost,rarity, maxStack,sprite)
        {
            this.slotCount = slotCount;
            this.skill = skill;
            
            slots = new GemSlot[slotCount];
            for (int i=0; i < slotCount; i++)
            {
                slots[i] = new GemSlot();
            }
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

        public void InsertGem(Gem gem, int slotindex)
        {
            slots[slotindex].gem = gem;
            gem.Insert();
            UpdateUser();

        }
        public Gem RemoveGem(int slotindex)
        {
            var gem = slots[slotindex].gem;
            gem.Remove();
            slots[slotindex].gem = null;
            UpdateUser();
            return gem;

        }

        public bool HasEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty())
                    return true;
            }

            return false;
        }


        public int GetSlotCount()
        {
            return slotCount;
        }

        public Gem GetGem(int index)
        {
            if (slots == null)
                return null;
            if (slots.Length > index)
                return slots[index].gem;
            return null;
        }

        public void Unequip(Unit unit)
        {
            if (skill != null)
            {
                skill.UnbindSkill(unit);
            }
        }

        public void Equip(Unit unit)
        {
            if (skill != null)
            {
                skill.BindSkill(unit);
            }
        }
    }
}