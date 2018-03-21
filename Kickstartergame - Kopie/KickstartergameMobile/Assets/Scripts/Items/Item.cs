
using System;
using UnityEngine;

[System.Serializable]
public abstract class Item {

    public String Name;
    public Sprite Sprite;
    public int NumberOfUses;
    public String Description;

	public Item(String name, String description, int useage, Sprite sprite){
		Name = name;
		Description = description;
        NumberOfUses = useage;
        Sprite = sprite;
	}

	public abstract void use(Human character);
}


