using Assets.Scripts.Characters.SpecialAttacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SpecialAttackManager
{
    public List<SpecialAttack> specials;
    public SpecialAttack equippedSpecial;

    public SpecialAttackManager()
    {
        specials = new List<SpecialAttack>();
    }
    public void AddSpecial(SpecialAttack special)
    {
        specials.Add(special);
        if (specials.Count == 1)
            equippedSpecial = special;
    }
    public bool HasSpecial<T>()
    {
        foreach (SpecialAttack s in specials)
        {
            if (s is T)
            {
                return true;
            }
        }
        return false;
    }

    public T GetSpecial<T>()
    {
        foreach (SpecialAttack s in specials)
        {
            if (s is T)
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
        }
        return default(T);
    }
}
