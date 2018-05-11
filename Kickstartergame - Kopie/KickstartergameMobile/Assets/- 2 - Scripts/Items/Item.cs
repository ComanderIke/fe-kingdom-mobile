
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item :ScriptableObject{

    [Header("ItemAttributes")]
    public String Name;
    public Sprite Sprite;
    public int NumberOfUses;
    public String Description;
    public List<ItemMixin> mixins;

	public Item(String name, String description, int useage, Sprite sprite){
		Name = name;
		Description = description;
        NumberOfUses = useage;
        Sprite = sprite;
        mixins = new List<ItemMixin>();
	}

	public virtual void Use(Human character)
    {
        foreach(ItemMixin mixin in mixins)
        {
            mixin.Use(character);
        }
    }
}


