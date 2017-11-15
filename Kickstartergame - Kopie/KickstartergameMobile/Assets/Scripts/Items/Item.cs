
using System;
using UnityEngine;

[System.Serializable]
public abstract class Item {

    public String Name { get; set; }
    public Sprite Sprite { get; private set; }
    public int NumberOfUses {  get;  protected set; }
	public string Description { get; private set; }

	public Item(String name, String description, int useage, Sprite sprite){
		Name = name;
		Description = description;
        NumberOfUses = useage;
        Sprite = sprite;
	}

	public abstract void use(Human character);
}


