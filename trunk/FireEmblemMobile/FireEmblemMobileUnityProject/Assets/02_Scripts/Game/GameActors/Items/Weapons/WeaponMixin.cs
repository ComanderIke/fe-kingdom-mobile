using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public abstract class WeaponMixin : ScriptableObject
    {
        public string Name;
        public abstract void OnAttack(Unit attacker, Unit defender);
    }
}