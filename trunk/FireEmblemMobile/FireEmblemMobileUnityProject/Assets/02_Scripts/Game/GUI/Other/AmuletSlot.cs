using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.GUI
{
    public class AmuletSlot:EquipmentSlot
    {
        

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("TODO? DropAreaOnlyRelicsCondition");
         //   DropArea.dropConditions.Add(new IsWeaponCondition());
        }

       
    }
}