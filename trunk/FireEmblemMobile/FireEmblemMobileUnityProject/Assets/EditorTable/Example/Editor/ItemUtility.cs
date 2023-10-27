using UnityEngine;
using UnityEditor;
using System.Collections;

public class ItemUtility : MonoBehaviour {
	[MenuItem("Assets/Create/Table Example/Weapon")]
	static void CreateWeapon()
	{
		ETUtility.CreateAsset<Weapon>();
	}
	
	[MenuItem("Assets/Create/Table Example/Armor")]
	static void CreateArmor()
	{
		ETUtility.CreateAsset<Armor>();
	}
	
	[MenuItem("Assets/Create/Table Example/Potion")]
	static void CreatePotion()
	{
		ETUtility.CreateAsset<Potion>();
	}
}
