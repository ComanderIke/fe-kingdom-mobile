using UnityEngine;
using Assets.Scripts.Characters;

[System.Serializable]
public abstract class ItemMixin : ScriptableObject {
    public new string name;
	public abstract void Use(Unit character);
		
}


