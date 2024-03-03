using UnityEngine;

namespace Game.Audio
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
