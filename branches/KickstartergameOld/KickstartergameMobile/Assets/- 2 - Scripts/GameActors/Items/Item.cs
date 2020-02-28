
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item :ScriptableObject{

    [Header("ItemAttributes")]
    public Sprite Sprite;
    public int NumberOfUses;
    public String Description;
    public List<ItemMixin> mixins;

	public virtual void Use(Human character)
    {
        foreach(ItemMixin mixin in mixins)
        {
            mixin.Use(character);
        }
    }
}


