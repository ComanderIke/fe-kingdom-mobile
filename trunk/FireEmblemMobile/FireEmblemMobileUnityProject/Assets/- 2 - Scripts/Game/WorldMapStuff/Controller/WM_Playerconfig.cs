using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    public class WM_Playerconfig : MonoBehaviour
    {
        [Header("Please specify players")] [SerializeField]
        public List<WM_Faction> factions;
    }

}