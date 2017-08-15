using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class DoorKey :Item
	{
		public DoorKey(String name, String description, int useage,Sprite sprite, Sprite hovered, Sprite pressed, GameObject go, GameObject go3d, bool droppedOnDeath):base(name, description, useage, sprite, hovered, pressed, go,go3d, droppedOnDeath) {

		}

		public override void use(Character character) {

		}

	}
}

