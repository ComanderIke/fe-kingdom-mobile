using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class GameConfig : MonoBehaviour
    {
        public static GameConfig Instance;

        public GameConfigFile config;
        private void Awake()
        {
            Instance = this;
        }
    }
}
