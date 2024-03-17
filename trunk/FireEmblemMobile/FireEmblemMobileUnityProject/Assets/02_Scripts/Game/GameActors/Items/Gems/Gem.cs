using System;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Items.Gems
{
    [System.Serializable]
    public class Gem : Item
    { 
        [SerializeField]
        private GemType gemType;

        [SerializeField] public Skill gemEffect;
        private bool inserted = false;
        private Gem upgradeTo;

        public string effectText;

        // private int soulCapacity;
        // private int currentSouls;
        public event Action onSoulsIncreased;
        public Gem(string name, string description, int cost, int maxStack,Sprite sprite, int rarity, Skill gemEffect, Gem upgradeTo, string effectText) : base(name, description, cost, rarity,maxStack,sprite)
        {
            this.gemEffect = gemEffect;
            this.upgradeTo = upgradeTo;
            this.effectText = effectText;
            // this.soulCapacity = soulCapacity;
            // currentSouls = startSouls;
        }

        // public GemType GetGemType(GemType gemType)
        // {
        //     return gemType;
        // }
        public void Insert()
        {
            inserted = true;
        }
        public void Remove()
        {
            inserted = false;
        }
        public bool IsInserted()
        {
            return inserted;
        }

        public Gem GetUpgradedGem()
        {
            return upgradeTo;
        }

        // public void IncreaseSouls()
        // {
        //     currentSouls++;
        //     if (currentSouls > soulCapacity)
        //         currentSouls = soulCapacity;
        //     else
        //     {
        //         
        //         
        //         Rebind();
        //         onSoulsIncreased?.Invoke();
        //     }
        //     
        // }

        public void Rebind()
        {
            // Debug.Log("REBIND GEM");
            // gemEffect.skillTransferData.data= (float)currentSouls;
            // Debug.Log("TransferData: "+ gemEffect.skillTransferData.data);
            // Debug.Log("GUID:" +gemEffect.skillTransferData.guid);
            gemEffect.Rebind();
        }

        public bool HasUpgrade()
        {
            return upgradeTo != null;
        }

        // public int GetCurrentSouls()
        // {
        //     return currentSouls;
        // }

        // public void SetSouls(int souls)
        // {
        //     this.currentSouls = souls;
        // }
        public void Bind(Unit user)
        {
            gemEffect.Level = rarity-1;
            gemEffect.BindSkill(user);
        }

        public void Unbind(Unit user)
        {
            gemEffect.UnbindSkill(user);
        }
    }
}