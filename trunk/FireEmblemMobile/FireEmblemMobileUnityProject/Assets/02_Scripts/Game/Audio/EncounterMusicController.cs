using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

namespace LostGrace
{
    public class EncounterMusicController : MonoBehaviour
    {
        void Start()
        {
            AudioSystem.Instance.ChangeAllMusic("EncounterAreaTheme");
        }

        void Update()
        {
        
        }
    }
}
