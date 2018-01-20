
using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Skills;
using Assets.Scripts.Characters.Debuffs;
using Assets.Scripts.Characters.Attributes;
using Assets.Scripts.Battle;


[System.Serializable]
public class Human : LivingObject{
    
    public Inventory Inventory { get; set; }
    public SkillManager SkillManager { get; set; }
    public Weapon EquipedWeapon { get; set; }

    public Human(string name, Sprite sprite):base(name, sprite)
    {
        Inventory = new Inventory(this);
        SkillManager = new SkillManager();
		Sprite = GameObject.FindObjectOfType<SpriteScript>().swordActiveSprite;
        List<int> attackRanges = new List<int>();
        attackRanges.Add(1);
        Stats = new Stats(20, 5, 5, 10, 8, 5, 5, 3, attackRanges);
    }
}



