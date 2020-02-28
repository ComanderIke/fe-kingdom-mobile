using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class WeaponMixin : ScriptableObject
{
    public new string name;
    public abstract void OnAttack(Unit attacker, Unit defender);
}

