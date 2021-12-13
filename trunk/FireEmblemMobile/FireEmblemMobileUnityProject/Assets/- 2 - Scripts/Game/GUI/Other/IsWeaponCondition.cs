using Game.GameActors.Items.Weapons;

namespace Game.GUI
{
    public class IsWeaponCondition:DropCondition
    {
        public override bool Check(UIDragable dragable)
        {
            return dragable.item is Weapon;
        }
    }
}