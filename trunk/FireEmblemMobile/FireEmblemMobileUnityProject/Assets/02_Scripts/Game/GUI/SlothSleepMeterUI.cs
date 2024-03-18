using Game.GameActors.Units;
using Game.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GUI
{
    public class SlothSleepMeterUI : MonoBehaviour
    {
        [FormerlySerializedAs("ragePointContainer")] [SerializeField] private Transform sleepPointContainer;
        [SerializeField] private GameObject  sleepPointPrefab;

        //[SerializeField] private Sprite sleepPointFullSprite;
        // Start is called before the first frame update
        private SlothAIBehaviour aiBehaviour;
        public void Show(SlothAIBehaviour slothAIBehaviour)
        {
            this.aiBehaviour = slothAIBehaviour;
            gameObject.SetActive(true);
            slothAIBehaviour.OnSleepMeterChanged -= UpdateValues;
            slothAIBehaviour.OnSleepMeterChanged += UpdateValues;
            UpdateValues();
        }

        private void OnDisable()
        { 
            if(aiBehaviour!=null)
                aiBehaviour.OnSleepMeterChanged -= UpdateValues;
        }

        private int currentSleepMeter = 0;
        private void UpdateValues()
        {
            sleepPointContainer.DeleteAllChildren();
            for (int i = 0; i < aiBehaviour.GetMaxSleepMeter(); i++)
            {
                var go = Instantiate(sleepPointPrefab, sleepPointContainer);
                if (i < aiBehaviour.GetSleepMeter())
                {
                    if (i >= currentSleepMeter)
                    {
                        go.GetComponent<UIMeterPoint>().Activate();
                    }
                    else
                    {
                        go.GetComponent<UIMeterPoint>().Fill();
                    }
                    
                  
                }
            }

            currentSleepMeter = aiBehaviour.GetSleepMeter();
            if (currentSleepMeter >= aiBehaviour.GetMaxSleepMeter())
            {
                MonoUtility.DelayFunction(()=>Hide(),1.0f);
            }
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}