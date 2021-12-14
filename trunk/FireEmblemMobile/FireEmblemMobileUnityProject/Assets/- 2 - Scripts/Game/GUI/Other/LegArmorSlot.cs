using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.GUI
{
    public class LegArmorSlot:EquipmentSlot
    {
        

        protected override void Awake()
        {
            base.Awake();
            DropArea.dropConditions.Add(new IsWeaponCondition(EquipmentSlotType.Legarmor));
        }

       
    }
}