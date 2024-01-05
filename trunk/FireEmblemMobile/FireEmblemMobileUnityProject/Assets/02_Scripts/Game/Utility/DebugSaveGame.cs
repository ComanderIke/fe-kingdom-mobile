using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class DebugSaveGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            if (SaveGameManager.currentSaveData == null)
            {
                MyDebug.LogLogic("Starting with Hard Difficulty from Sanctuary");
                SaveGameManager.NewGame(0, "DebugStartSanctuary", "Hard");
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
