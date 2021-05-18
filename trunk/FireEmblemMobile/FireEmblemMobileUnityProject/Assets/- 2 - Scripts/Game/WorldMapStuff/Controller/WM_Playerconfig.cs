using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class WM_Playerconfig : MonoBehaviour
    {
        [Header("Please specify players")] [SerializeField]
        private List<WM_Faction> factions;

        public List<WM_Faction> GetFactions()
        {
            var list = new List<WM_Faction>();
            foreach (var faction in factions)
            {
                list.Add(Instantiate(faction));
            }
            return list;
        }
    }

}