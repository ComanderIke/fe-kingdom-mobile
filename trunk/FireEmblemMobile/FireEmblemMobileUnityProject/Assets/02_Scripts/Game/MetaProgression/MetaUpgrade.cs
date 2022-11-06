using System;
using UnityEngine;

[Serializable]
public class MetaUpgrade:IEquatable<MetaUpgrade>
{
    public MetaUpgradeBP blueprint;
    public int level;
    [HideInInspector]public bool locked = false;
   

    public MetaUpgrade(MetaUpgradeBP bluePrint)
    {
        this.blueprint = bluePrint;
        level = 0;
        
    }

    public bool IsMaxed()
    {
        return level == blueprint.maxLevel;
    }

    public bool Equals(MetaUpgrade other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(blueprint, other.blueprint);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MetaUpgrade)obj);
    }

    public override int GetHashCode()
    {
        return (blueprint != null ? blueprint.GetHashCode() : 0);
    }
}