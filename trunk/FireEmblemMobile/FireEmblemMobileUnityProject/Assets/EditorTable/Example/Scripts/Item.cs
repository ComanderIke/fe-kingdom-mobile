using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cost
{
	public int gold;
	public int silver;
}

public class Item : ScriptableObject {
	public Color color;
	public string fullName;
	public string description;
	public Cost cost;
}
