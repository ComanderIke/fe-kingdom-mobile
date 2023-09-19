using System;
using System.Collections;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace LostGrace
{
    [Serializable]
    public struct TimeOfDayBonuses
    {
        public int curseResistance;
        public int enemylevelsPerArea;
        // public int enemyCritical;
        public string other;
    }
   
  
    public class TimeOfDayManager : MonoBehaviour
    {
        [SerializeField] private DynamicAmbientLight lightController;
        [SerializeField] private int timeStep = 6;
        [SerializeField] private UIDebugTimeSlider timeSlider;
        private float hour;
        public TimeCircleUI circleUI;
        [SerializeField] private TimeOfDayBonuses nightBonuses;
        [SerializeField] private TimeOfDayBonuses dayBonuses;
      
        void Start()
        {
            lightController.UpdateHour(hour);
        }

        void Update()
        {
        
        }

        public void ShowTooltip()
        {
            Debug.Log("Time of Day: "+hour+ " "+(hour>=20||hour<=4));
            ToolTipSystem.ShowTimeOfDay(hour, hour>=20||hour<=4?nightBonuses:dayBonuses);
        }
        public float GetCurrentHour()
        {
            return hour;
        }

        public void InitHour(float hour)
        {
            timeSlider.SetValue(hour);
            UpdateHour(hour);
        }
        public void UpdateHour(float hour)
        {
            if (hour >= 24)
                hour = 0;
            this.hour = hour;
            lightController.UpdateHour(hour);
            circleUI.Rotate(hour);
        }

        public void ElapseTimeStep()
        {
            Debug.Log("Current hour "+hour);
            UpdateHour(hour + 6);
            Debug.Log("new hour "+hour);
        }

        public void SetNoon()
        {
           
            float newTime = 6;
            float tmphour = hour;
            float diff =  newTime-hour;
            if (diff <= 0)
                diff += 24;
      
            LeanTween.value(0, diff, 2.5f).setOnUpdate((val) =>
            {
                float currentHour =(tmphour + val);
                if (currentHour >= 24)
                    currentHour -= 24;
               
                
                lightController.UpdateHourFixed(currentHour);
                circleUI.RotateFixed(currentHour);
            }).setEaseInOutQuad();
            hour = 6;
            // lightController.UpdateHour(hour);
            //  circleUI.RotateFull(hour);
        }
    }
}
