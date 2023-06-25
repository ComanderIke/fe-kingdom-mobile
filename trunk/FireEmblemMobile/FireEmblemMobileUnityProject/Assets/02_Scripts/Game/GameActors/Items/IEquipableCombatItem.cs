using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.GameActors.Items
{
    public interface IEquipableCombatItem
    {
        object Clone();
        Sprite GetIcon();

        string GetName();
    }
}