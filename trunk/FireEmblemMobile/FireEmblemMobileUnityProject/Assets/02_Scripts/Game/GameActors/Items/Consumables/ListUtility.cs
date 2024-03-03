using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Items.Consumables
{
    public static class ListUtility
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count - 1)];
        }
    }
}