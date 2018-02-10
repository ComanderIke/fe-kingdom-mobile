
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
    public List<AttackType> AttackTypes { get; set; }
    public List<DefenseType> DefenseTypes { get; set; }
    public Human(string name, Sprite sprite):base(name, sprite)
    {
        Inventory = new Inventory(this);
        SkillManager = new SkillManager();
        Sprite = sprite;// GameObject.FindObjectOfType<SpriteScript>().swordActiveSprite;
        List<int> attackRanges = new List<int>();
        AttackTypes = new List<AttackType>();
        AttackTypes.Add(new AttackType("StrongAttack", 1.5f, -20,0));
        AttackTypes.Add(new AttackType("FastAttack", 1.0f, 0,0));
        DefenseTypes = new List<DefenseType>();
        DefenseTypes.Add(new DefenseType("Guard", 0.7f, 0));
        DefenseTypes.Add(new DefenseType("Dodge", 1.0f, -20));
        DefenseTypes.Add(new DefenseType("Counter", 1.0f, 0));
        attackRanges.Add(1);
        Stats = new Stats(20, 5, 4, 10, 8, 5, 5, 3, attackRanges);
    }
}



