using Game.EncounterAreas.DayNightCycle;
using Game.GameActors.Player;
using Game.GUI;
using Game.GUI.ToolTips;
using Game.Utility;
using TMPro;
using UnityEngine;

namespace Game.GameMechanics
{
    public class TimeOfDayManager : MonoBehaviour
    {
        [SerializeField] private DynamicAmbientLight lightController;
        [SerializeField] private int timeStep = 6;
        [SerializeField] private UIDebugTimeSlider timeSlider;
        [SerializeField] private float hour;
        [SerializeField] private TextMeshProUGUI timeText;
        public TimeCircleUI circleUI;
         private TimeOfDayBonuses nightBonuses;
         private TimeOfDayBonuses dayBonuses;
      
        void Start()
        {
            nightBonuses = Player.Instance.Modifiers.NightBonuses;
            dayBonuses = Player.Instance.Modifiers.DayBonuses;
            
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
            timeText.text = hour + ":00";
            if(circleUI!=null)
                circleUI.Rotate(hour);
        }

        public void ElapseTimeStep()
        {
            // Debug.Log("Current hour "+hour);
            UpdateHour(hour + 6);
            MyDebug.LogLogic("New Time: "+hour+":00");
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
                if(circleUI!=null)
                    circleUI.RotateFixed(currentHour);
            }).setEaseInOutQuad();
            hour = 6;
            // lightController.UpdateHour(hour);
            //  circleUI.RotateFull(hour);
        }
    }
}
