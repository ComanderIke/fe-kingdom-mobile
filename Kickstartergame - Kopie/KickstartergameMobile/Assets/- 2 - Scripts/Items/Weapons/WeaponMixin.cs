using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Items.Weapons
{
    
    public abstract class WeaponMixin : ScriptableObject
    {
        public new string name;
        public abstract void OnAttack(LivingObject attacker, LivingObject defender);
    }
}
