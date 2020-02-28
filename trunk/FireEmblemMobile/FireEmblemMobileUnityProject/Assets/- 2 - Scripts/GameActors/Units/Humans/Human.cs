

using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.SpecialAttacks;

[System.Serializable]
[CreateAssetMenu(menuName ="GameData/Human", fileName ="Human")]
public class Human : Unit{

    public Inventory Inventory;
    public SkillManager SkillManager { get; set; }
    public SpecialAttackManager SpecialAttackManager { get; set; }
    public Weapon EquipedWeapon;
    public List<AttackType> AttackTypes;
    public List<DefenseType> DefenseTypes;

    public new void OnEnable()
    {
        base.OnEnable();
        if (Inventory == null)
        {
            Inventory = Instantiate(CreateInstance<Inventory>());
        }
        else
            Inventory.owner = this;
        SkillManager = new SkillManager();
        SpecialAttackManager = new SpecialAttackManager();
        SpecialAttackManager.AddSpecial(new LightningStrike());
    }
}



