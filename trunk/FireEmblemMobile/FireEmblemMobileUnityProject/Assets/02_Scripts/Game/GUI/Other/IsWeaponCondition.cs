using Game.GameActors.Items.Weapons;

namespace Game.GUI
{
    public class IsWeaponCondition:DropCondition
    {
        private EquipmentSlotType slotType;

        public IsWeaponCondition(EquipmentSlotType itemSlot)
        {
            slotType = itemSlot;
        }

        public override bool Check(UIDragable dragable)
        {
            if (dragable.item is EquipableItem equipableItem)
            {
                return equipableItem.EquipmentSlotType == slotType;
            }
            return false;
        }
    }
}