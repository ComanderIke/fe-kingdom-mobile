using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors.Items.Weapons
{
    public abstract class WeaponMixin : ScriptableObject
    {
        public string Name;
        public abstract void OnAttack(Unit attacker, Unit defender);
    }
}