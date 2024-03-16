using System;
using Game.Manager;
using Game.SerializedData;
using Game.Systems;
using Game.Utility;
using GameEngine;
using UnityEngine;

namespace Game.GameMechanics
{
    public class TimeOfDayGridManager : MonoBehaviour
    {
        [SerializeField] private TimeOfDayManager timeOfDayManager;
        [SerializeField]private float defaultStartHour = 12;

        public void Start()
        {
            if(SaveGameManager.currentSaveData!=null&&SaveGameManager.currentSaveData.EncounterAreaData!=null)
                timeOfDayManager.UpdateHour(SaveGameManager.currentSaveData.EncounterAreaData.timeOfDaySaveData.hour);
            else 
                timeOfDayManager.UpdateHour(defaultStartHour);
            ServiceProvider.Instance.GetSystem<TurnSystem>().OnStartTurn -= TimePasses;
            ServiceProvider.Instance.GetSystem<TurnSystem>().OnStartTurn += TimePasses;
        }

        private void OnDestroy()
        {
            ServiceProvider.Instance.GetSystem<TurnSystem>().OnStartTurn -= TimePasses;
        }

        void TimePasses()
        {
            timeOfDayManager.UpdateHour(timeOfDayManager.GetCurrentHour()+1);
        }
    }
}