using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.GUI
{
    public class BootsSlot:EquipmentSlot
    {
        

        protected override void Awake()
        {
            base.Awake();
            DropArea.dropConditions.Add(new IsWeaponCondition(EquipmentSlotType.Boots));
        }

       
    }
}