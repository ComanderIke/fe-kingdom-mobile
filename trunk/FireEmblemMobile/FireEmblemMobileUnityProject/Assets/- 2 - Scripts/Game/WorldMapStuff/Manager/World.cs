using System;
using System.Collections.Generic;
using System.Linq;
using Game.WorldMapStuff.Controller;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public class World:MonoBehaviour
    {
        public List<WorldMapPosition> worldPositions;

        public List<LocationController> Locations;

        private void Awake()
        {
            worldPositions = FindObjectsOfType<WorldMapPosition>().ToList();
            Locations = new List<LocationController>();
            foreach (WorldMapPosition pos in worldPositions)
            {
                foreach (LocationController loc in pos.locationControllers)
                {
                    Locations.Add(loc);
                }
            }
        }
    }
}