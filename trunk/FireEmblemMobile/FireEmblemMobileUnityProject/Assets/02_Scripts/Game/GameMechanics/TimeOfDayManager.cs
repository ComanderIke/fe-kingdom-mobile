using System.Collections;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace LostGrace
{
    public class TimeOfDayManager : MonoBehaviour
    {
        [SerializeField] private DynamicAmbientLight lightController;
        [SerializeField] private int timeStep = 6;
        [SerializeField] private UIDebugTimeSlider timeSlider;
        private float hour;
        public TimeCircleUI circleUI;
        void Start()
        {
            lightController.UpdateHour(hour);
        }

        void Update()
        {
        
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
            UpdateHour(hour + 6);
        }
    }
}
