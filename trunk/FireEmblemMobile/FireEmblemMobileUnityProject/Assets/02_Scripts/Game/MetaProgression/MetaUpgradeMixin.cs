using System.Collections.Generic;
using LostGrace;
using UnityEngine;

public abstract class MetaUpgradeMixin : ScriptableObject
{
    public abstract void Activate(int level);

    public abstract IEnumerable<EffectDescription> GetEffectDescriptions(int level);
}