using System.Collections;
using System.Collections.Generic;
using Game.GameMechanics;
using UnityEngine;

namespace LostGrace
{
    public class SanctuaryTimeManager : MonoBehaviour
    {
        [SerializeField] private TimeOfDayManager timeOfDayManager;

        [SerializeField]private float HoursPerSecond = 1f;

        private float hours = 0;
        // Start is called before the first frame update
        void Start()
        {
            hours = timeOfDayManager.GetCurrentHour();
        }

        // Update is called once per frame
        void Update()
        {
            hours += Time.deltaTime * HoursPerSecond;
            if (hours >= 24)
                hours = 0;
            timeOfDayManager.UpdateHourFixed(hours);
            MyDebug.LogTest("Hours: "+hours);
        }
    }
}
