using System;
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
        public RelicBP EquippedRelic2;
        public string Name;

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
        
        [SerializeField] public UnitVisual visuals;

        [SerializeField] public ExperienceManager experienceManager;

        public override string ToString()
        {
            return name;// + " HP: " + Hp + "/" + MaxHp+"Level: "+experienceManager.Level+ " Exp: "+experienceManager.Exp;
        }

        private void OnEnable()
        {
            if(visuals.CharacterSpriteSet==null)
                Debug.LogWarning("No Character Sprite Set on Blueprint: "+name);
            if(moveType==null)
                Debug.LogWarning("No moveType on Blueprint: "+name);
            if(visuals.UnitEffectVisual==null)
                Debug.LogWarning("No UnitEffectVisual on Blueprint: "+name);
            if(visuals.Prefabs.EncounterAnimatedSprite==null)
                Debug.LogWarning("No EncounterAnimatedSprite on Blueprint: "+name);
            if(visuals.Prefabs.UIAnimatorController==null)
                Debug.LogWarning("No UIAnimatorController on Blueprint: "+name);
            if(visuals.Prefabs.MapAnimatedSprite==null)
                Debug.LogWarning("No MapAnimatedSprite on Blueprint: "+name);
            
        }

        public Unit Create()
        {
            Debug.Log("Name: "+name+" "+"Skills: "+SkillManager.Skills);
            Weapon weapon = null;
            if(equippedWeaponBp!=null)
                weapon = (Weapon)equippedWeaponBp.Create();
            Relic relic1 = null;
            if(EquippedRelic1!=null)
                relic1 = (Relic)EquippedRelic1.Create();
            Relic relic2 = null;
            if(EquippedRelic2!=null)
                relic2 = (Relic)EquippedRelic2.Create();
            return new Unit(Name, rpgClass, (Stats)stats.Clone(), growths, moveType, weapon, relic1,relic2, visuals, new SkillManager(SkillManager),
                new ExperienceManager(experienceManager));
        }
    }
}