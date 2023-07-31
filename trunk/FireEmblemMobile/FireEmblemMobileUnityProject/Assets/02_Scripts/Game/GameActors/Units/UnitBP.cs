﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.GameResources;
using Game.Graphics;
using Game.Grid;
using Game.GUI;
using Game.Mechanics;
using Game.Mechanics.Battle;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Human", fileName = "Human")]
    public class UnitBP : ScriptableObject
    {

        [FormerlySerializedAs("EquippedWeapon")] public WeaponBP equippedWeaponBp;
        public RelicBP EquippedRelic1;
        public string Name;

        [SerializeField] public StockedItemBP combatItem1;
        [SerializeField] public StockedItemBP combatItem2;
        
        [SerializeField]
        public Stats stats;
        [SerializeField]
        public Attributes growths;
        [SerializeField]
        public MoveType moveType;
        [SerializeField]
        public SkillManager SkillManager;
        [SerializeField]
        public RpgClass rpgClass;
        public string bluePrintID;
        [SerializeField] private DialogActor dialogActor;
        [SerializeField] public UnitVisual visuals;
        [SerializeField]
        public CharacterAudioSet audio;
        [SerializeField] public ExperienceManager experienceManager;

        public override string ToString()
        {
            return name;// + " HP: " + Hp + "/" + MaxHp+"Level: "+experienceManager.Level+ " Exp: "+experienceManager.Exp;
        }

        private void OnEnable()
        {
            if (visuals == null)
            {
                Debug.LogWarning("No Visuals on Blueprint: "+name);
                return;
            }
              
            if(visuals.CharacterSpriteSet==null)
                Debug.LogWarning("No Character Sprite Set on Blueprint: "+name);
            if(moveType==null)
                Debug.LogWarning("No moveType on Blueprint: "+name);
            if(visuals.UnitEffectVisual==null)
                Debug.LogWarning("No UnitEffectVisual on Blueprint: "+name);

            if(visuals.Prefabs.UIAnimatorController==null)
                Debug.LogWarning("No UIAnimatorController on Blueprint: "+name);

        }

        public virtual Unit Create()
        {
            Weapon weapon = null;
            if(equippedWeaponBp!=null)
                weapon = (Weapon)equippedWeaponBp.Create();
            var skillManager = new SkillManager(SkillManager);
            skillManager.AddStartSkills();
            var unit = new Unit(bluePrintID, Name, rpgClass, (Stats)stats.Clone(), growths, moveType, weapon, 
                new UnitVisual(visuals), skillManager,
                new ExperienceManager(experienceManager));
            Relic relic1 = null;
            if (EquippedRelic1 != null)
            {
                relic1 = (Relic)EquippedRelic1.Create();
                unit.Equip(relic1);
            }

            StockedCombatItem cItem1= null;
            if (this.combatItem1 != null && this.combatItem1.item != null && combatItem1.stock > 0)
            {
                var tmp = combatItem1.Create();
                cItem1 = new StockedCombatItem((IEquipableCombatItem)tmp.item, tmp.stock);
                unit.Equip(cItem1, 1);
            }
               
            StockedCombatItem cItem2= null;
            if (this.combatItem2 != null && this.combatItem2.item != null && combatItem2.stock > 0)
            {
                var tmp = combatItem2.Create();
                cItem2 = new StockedCombatItem((IEquipableCombatItem)tmp.item, tmp.stock);
                unit.Equip(cItem2, 2);
            }

           
           
           
          
           
           
            return unit;
        }
    }
}