using Game.GameActors.Units;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class MinotaurRageMeterUI : MonoBehaviour
    {
        [SerializeField] private Transform ragePointContainer;
        [SerializeField] private GameObject ragePointPrefab;

        [SerializeField] private Sprite ragePointFullSprite;
        // Start is called before the first frame update
        private MinotaurAIBehaviour aiBehaviour;
        public void Show(MinotaurAIBehaviour minotaurBehaviour)
        {
            this.aiBehaviour = minotaurBehaviour;
            
            minotaurBehaviour.OnRageMeterChanged -= UpdateValues;
            minotaurBehaviour.OnRageMeterChanged += UpdateValues;
            UpdateValues();
        }

        private void OnDisable()
        { 
            if(aiBehaviour!=null)
                aiBehaviour.OnRageMeterChanged -= UpdateValues;
        }

        private void UpdateValues()
        {
            ragePointContainer.DeleteAllChildren();
            for (int i = 0; i < aiBehaviour.GetMaxRageMeter(); i++)
            {
                var go = Instantiate(ragePointPrefab, ragePointContainer);
                if (i < aiBehaviour.GetRageMeter())
                {
                    go.GetComponent<Image>().sprite = ragePointFullSprite;
                }
            }
        }
    }
}
