using Game.SerializedData;
using UnityEngine;

namespace Game.Utility
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
