using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace LostGrace
{
    public class MainMenuEffectsController : MonoBehaviour
    {
        void Start()
        {
            AudioSystem.Instance.PlayMusic("MainMenuTheme");
        }

        void Update()
        {
        
        }
    }
}
