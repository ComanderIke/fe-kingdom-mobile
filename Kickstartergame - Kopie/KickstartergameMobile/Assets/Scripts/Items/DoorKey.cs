using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class DoorKey :Item
	{
		public DoorKey(String name, String description, int useage,Sprite sprite, GameObject go ):base(name, description, useage, sprite, go) {

		}

		public override void use(Character character) {

		}

	}
}

