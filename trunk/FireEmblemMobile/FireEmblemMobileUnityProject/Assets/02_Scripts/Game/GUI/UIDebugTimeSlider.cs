using Game.GameMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIDebugTimeSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TimeOfDayManager timeManager;
        void Start()
        {
            gameObject.SetActive(GameConfig.Instance.ConfigProfile.debugModeEnabled);
        }

        void Update()
        {
        
        }
        public void OnSliderValueChanged(float slider)
        {
            timeManager.UpdateHour(slider);
        }

        public void SetValue(float hour)
        {
            slider.value = hour;
        }
    }
}
