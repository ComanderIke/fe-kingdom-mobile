using System.Collections;
using System.Collections.Generic;
using Game.SerializedData;
using Game.Utility;
using UnityEngine;

namespace LostGrace
{
    public class GridBattleDayTimeController : MonoBehaviour
    {
        [SerializeField] private DynamicAmbientLight lightController;
        [SerializeField] private float startHour = 12;
        void Start()
        {
            
            if(SaveGameManager.currentSaveData!=null&&SaveGameManager.currentSaveData.EncounterAreaData!=null)
                lightController.UpdateHour(SaveGameManager.currentSaveData.EncounterAreaData.timeOfDaySaveData.hour);
            else 
                lightController.UpdateHour(startHour);
           
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
